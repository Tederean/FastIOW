using System;
using System.Threading;
using Tederean.FastIOW;

namespace I2C_LiquidCrystal
{

	public class LiquidCrystalI2C
	{

		// commands
		public const int LCD_CLEARDISPLAY = 0x01;
		public const int LCD_RETURNHOME = 0x02;
		public const int LCD_ENTRYMODESET = 0x04;
		public const int LCD_DISPLAYCONTROL = 0x08;
		public const int LCD_CURSORSHIFT = 0x10;
		public const int LCD_FUNCTIONSET = 0x20;
		public const int LCD_SETCGRAMADDR = 0x40;
		public const int LCD_SETDDRAMADDR = 0x80;

		// flags for display entry mode
		public const int LCD_ENTRYRIGHT = 0x00;
		public const int LCD_ENTRYLEFT = 0x02;
		public const int LCD_ENTRYSHIFTINCREMENT = 0x01;
		public const int LCD_ENTRYSHIFTDECREMENT = 0x00;

		// flags for display on/off control
		public const int LCD_DISPLAYON = 0x04;
		public const int LCD_DISPLAYOFF = 0x00;
		public const int LCD_CURSORON = 0x02;
		public const int LCD_CURSOROFF = 0x00;
		public const int LCD_BLINKON = 0x01;
		public const int LCD_BLINKOFF = 0x00;

		// flags for display/cursor shift
		public const int LCD_DISPLAYMOVE = 0x08;
		public const int LCD_CURSORMOVE = 0x00;
		public const int LCD_MOVERIGHT = 0x04;
		public const int LCD_MOVELEFT = 0x00;

		// flags for function set
		public const int LCD_8BITMODE = 0x10;
		public const int LCD_4BITMODE = 0x00;
		public const int LCD_2LINE = 0x08;
		public const int LCD_1LINE = 0x00;
		public const int LCD_5x10DOTS = 0x04;
		public const int LCD_5x8DOTS = 0x00;

		// flags for backlight control
		public const int LCD_BACKLIGHT = 0x08;
		public const int LCD_NOBACKLIGHT = 0x00;

		public const int En = 0b00000100;  // Enable bit
		public const int Rw = 0b00000010;  // Read/Write bit
		public const int Rs = 0b00000001;  // Register select bit


		private I2CInterface I2C { set; get; }
		private int Addr { set; get; }
		private int Displayfunction { set; get; }
		private int Displaycontrol { set; get; }
		private int Displaymode { set; get; }
		private int Cols { set; get; }
		private int Rows { set; get; }
		private int Charsize { set; get; }
		private int Backlightval { set; get; }


		public LiquidCrystalI2C(I2CInterface i2c, int lcd_addr, int lcd_cols, int lcd_rows, int charsize = LCD_5x8DOTS)
		{
			I2C = i2c;
			Addr = lcd_addr;
			Cols = lcd_cols;
			Rows = lcd_rows;
			Charsize = charsize;
			Backlightval = LCD_BACKLIGHT;
		}

		void DelayMicroseconds(int time)
		{
			Thread.Sleep((int)Math.Ceiling(time / 1000.0));
		}

		public void Begin()
		{
			Displayfunction = LCD_4BITMODE | LCD_1LINE | LCD_5x8DOTS;

			if (Rows > 1)
			{
				Displayfunction |= LCD_2LINE;
			}

			// for some 1 line displays you can select a 10 pixel high font
			if ((Charsize != 0) && (Rows == 1))
			{
				Displayfunction |= LCD_5x10DOTS;
			}

			// SEE PAGE 45/46 FOR INITIALIZATION SPECIFICATION!
			// according to datasheet, we need at least 40ms after power rises above 2.7V
			// before sending commands. Arduino can turn on way befer 4.5V so we'll wait 50
			DelayMicroseconds(50000);

			// Now we pull both RS and R/W low to begin commands
			ExpanderWrite(Backlightval); // reset expanderand turn backlight off (Bit 8 =1)
			DelayMicroseconds(1000000);

			//put the LCD into 4 bit mode
			// this is according to the hitachi HD44780 datasheet
			// figure 24, pg 46

			// we start in 8bit mode, try to set 4 bit mode
			Write4bits(0x03 << 4);

			// wait min 4.1ms
			DelayMicroseconds(4500);

			// second try
			Write4bits(0x03 << 4);

			// wait min 4.1ms
			DelayMicroseconds(4500);

			// third go!
			Write4bits(0x03 << 4);
			DelayMicroseconds(150);

			// finally, set to 4-bit interface
			Write4bits(0x02 << 4);

			// set # lines, font size, etc.
			Command(LCD_FUNCTIONSET | Displayfunction);

			// turn the display on with no cursor or blinking default
			Displaycontrol = LCD_DISPLAYON | LCD_CURSOROFF | LCD_BLINKOFF;
			Display();

			// clear it off
			Clear();

			// Initialize to default text direction (for roman languages)
			Displaymode = LCD_ENTRYLEFT | LCD_ENTRYSHIFTDECREMENT;

			// set the entry mode
			Command(LCD_ENTRYMODESET | Displaymode);

			Home();
		}

		/********** high level commands, for the user! */
		public void Clear()
		{
			Command(LCD_CLEARDISPLAY);// clear display, set cursor position to zero
			DelayMicroseconds(2000);  // this command takes a long time!
		}

		public void Home()
		{
			Command(LCD_RETURNHOME);  // set cursor position to zero
			DelayMicroseconds(2000);  // this command takes a long time!
		}

		public void SetCursor(int col, int row)
		{
			int[] row_offsets = new int[] { 0x00, 0x40, 0x14, 0x54 };
			if (row > Rows)
			{
				row = Rows - 1;    // we count rows starting w/0
			}
			Command(LCD_SETDDRAMADDR | (col + row_offsets[row]));
		}

		// Turn the display on/off (quickly)
		public void NoDisplay()
		{
			Displaycontrol &= ~LCD_DISPLAYON;
			Command(LCD_DISPLAYCONTROL | Displaycontrol);
		}
		public void Display()
		{
			Displaycontrol |= LCD_DISPLAYON;
			Command(LCD_DISPLAYCONTROL | Displaycontrol);
		}

		// Turns the underline cursor on/off
		public void NoCursor()
		{
			Displaycontrol &= ~LCD_CURSORON;
			Command(LCD_DISPLAYCONTROL | Displaycontrol);
		}
		public void Cursor()
		{
			Displaycontrol |= LCD_CURSORON;
			Command(LCD_DISPLAYCONTROL | Displaycontrol);
		}

		// Turn on and off the blinking cursor
		public void NoBlink()
		{
			Displaycontrol &= ~LCD_BLINKON;
			Command(LCD_DISPLAYCONTROL | Displaycontrol);
		}
		public void Blink()
		{
			Displaycontrol |= LCD_BLINKON;
			Command(LCD_DISPLAYCONTROL | Displaycontrol);
		}

		// These commands scroll the display without changing the RAM
		public void ScrollDisplayLeft()
		{
			Command(LCD_CURSORSHIFT | LCD_DISPLAYMOVE | LCD_MOVELEFT);
		}
		public void ScrollDisplayRight()
		{
			Command(LCD_CURSORSHIFT | LCD_DISPLAYMOVE | LCD_MOVERIGHT);
		}

		// This is for text that flows Left to Right
		public void LeftToRight()
		{
			Displaymode |= LCD_ENTRYLEFT;
			Command(LCD_ENTRYMODESET | Displaymode);
		}

		// This is for text that flows Right to Left
		public void RightToLeft()
		{
			Displaymode &= ~LCD_ENTRYLEFT;
			Command(LCD_ENTRYMODESET | Displaymode);
		}

		// This will 'right justify' text from the cursor
		public void Autoscroll()
		{
			Displaymode |= LCD_ENTRYSHIFTINCREMENT;
			Command(LCD_ENTRYMODESET | Displaymode);
		}

		// This will 'left justify' text from the cursor
		public void NoAutoscroll()
		{
			Displaymode &= ~LCD_ENTRYSHIFTINCREMENT;
			Command(LCD_ENTRYMODESET | Displaymode);
		}

		// Allows us to fill the first 8 CGRAM locations
		// with custom characters
		public void CreateChar(byte location, byte[] charmap)
		{
			location &= 0x7; // we only have 8 locations 0-7
			Command(LCD_SETCGRAMADDR | (location << 3));
			for (int i = 0; i < 8; i++)
			{
				Write(charmap[i]);
			}
		}

		// Turn the (optional) backlight off/on
		public void NoBacklight()
		{
			Backlightval = LCD_NOBACKLIGHT;
			ExpanderWrite(0);
		}

		public void Backlight()
		{
			Backlightval = LCD_BACKLIGHT;
			ExpanderWrite(0);
		}
		public bool GetBacklight()
		{
			return Backlightval == LCD_BACKLIGHT;
		}


		/*********** mid level commands, for sending data/cmds */

		public void Command(int value)
		{
			Send(value, 0);
		}

		public int Write(int value)
		{
			Send(value, Rs);
			return 1;
		}


		/************ low level data pushing commands **********/

		// write either command or data
		private void Send(int value, int mode)
		{
			int highnib = value & 0xf0;
			int lownib = (value << 4) & 0xf0;
			Write4bits((highnib) | mode);
			Write4bits((lownib) | mode);
		}

		private void Write4bits(int value)
		{
			ExpanderWrite(value);
			PulseEnable(value);
		}

		private void ExpanderWrite(int _data)
		{
			int data = (_data) | Backlightval;

			I2C.WriteBytes((byte)Addr, (byte)data);
		}

		private void PulseEnable(int _data)
		{
			ExpanderWrite(_data | En);  // En high
			DelayMicroseconds(1);   // enable pulse must be >450ns

			ExpanderWrite(_data & ~En); // En low
			DelayMicroseconds(50);    // commands need > 37us to settle
		}

		public void LoadCustomCharacter(byte char_num, byte[] rows)
		{
			CreateChar(char_num, rows);
		}

		public void SetBacklight(int new_val)
		{
			if (new_val > 0)
			{
				Backlight();    // turn backlight on
			}
			else
			{
				NoBacklight();    // turn backlight off
			}
		}

		public void Print(string text)
		{
			//This function is not identical to the function used for "real" I2C displays
			//it's here so the user sketch doesn't have to be changed

			foreach (char c in text)
			{
				Write(Convert.ToByte(c));
			}
		}
	}
}
