using System;
using System.Collections.Generic;
using Tederean.FastIOW.Internal;

namespace Tederean.FastIOW
{

  public class IOWarrior24 : IOWarriorBase
  {

    public override string Name => "IOWarrior24";

    public override IOWarriorType Type => IOWarriorType.IOWarrior24;

    internal override int StandardReportSize => 3;

    internal override int SpecialReportSize => 8;

    internal override Pipe[] SupportedPipes => new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE };

    internal int[] TimerPins => new[] { Timer_1, Timer_2 };

   
    public const int P0_0 = 1 * 8 + 0;
    public const int P0_1 = 1 * 8 + 1;
    public const int P0_2 = 1 * 8 + 2;
    public const int P0_3 = 1 * 8 + 3;
    public const int P0_4 = 1 * 8 + 4;
    public const int P0_5 = 1 * 8 + 5;
    public const int P0_6 = 1 * 8 + 6;
    public const int P0_7 = 1 * 8 + 7;

    public const int P1_0 = 2 * 8 + 0;
    public const int P1_1 = 2 * 8 + 1;
    public const int P1_2 = 2 * 8 + 2;
    public const int P1_3 = 2 * 8 + 3;
    public const int P1_4 = 2 * 8 + 4;
    public const int P1_5 = 2 * 8 + 5;
    public const int P1_6 = 2 * 8 + 6;
    public const int P1_7 = 2 * 8 + 7;

    public const int RC5_IR = P0_0;

    public const int I2C_SCL = P0_1;
    public const int I2C_SDA = P0_2;

    public const int SPI_DRDY = P0_3;
    public const int SPI_SS = P0_4;
    public const int SPI_MOSI = P0_5;
    public const int SPI_MISO = P0_6;
    public const int SPI_SCK = P0_7;

    public const int Timer_1 = P0_0;
    public const int Timer_2 = P0_1;


    internal IOWarrior24(IntPtr handle) : base(handle)
    {
      InterfaceList.Add(new GPIOImplementation(this));
      InterfaceList.Add(new I2CImplementation(this, Pipe.SPECIAL_MODE, 6));
      InterfaceList.Add(new SPIImplementation(this, 6));

      if (Revision >= 0x1030)
      {
        InterfaceList.Add(new TimerImplementation(this, TimerPins));
      }
    }


    internal override bool IsValidDigitalPin(int pin)
    {
      return pin >= P0_0 && pin <= P1_7;
    }
  }
}
