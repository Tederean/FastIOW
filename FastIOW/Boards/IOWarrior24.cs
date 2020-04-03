/*
 *   
 *   Copyright 2020 Florian Porsch <tederean@gmail.com>
 *   
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation; either version 3 of the License, or
 *   (at your option) any later version.
 *   
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Lesser General Public License for more details.
 *   
 *   You should have received a copy of the GNU Lesser General Public License
 *   along with this program; if not, write to the Free Software
 *   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 *   MA 02110-1301 USA.
 *
 */
using Tederean.FastIOW.Internal;

namespace Tederean.FastIOW
{

  public class IOWarrior24 : IOWarriorBase, I2CDevice, TimerDevice, SPIDevice
  {

    public override string Name => "IOWarrior24";

    public override IOWarriorType Type => IOWarriorType.IOWarrior24;

    protected override int StandardReportSize => 3;

    protected override int SpecialReportSize => 8;

    protected override Pipe[] SupportedPipes => new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE };

    private int[] TimerPins => new[] { Timer_1, Timer_2 };

    public I2CInterface I2C { get; private set; }

    public TimerInterface Timer { get; private set; }

    public SPIInterface SPI { get; private set; }


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

    public const int BUILDIN_LED = P0_3;


    internal IOWarrior24(int handle) : base(handle)
    {
      I2C = new I2CInterfaceImplementation(this, Pipe.SPECIAL_MODE, 6);
      Timer = new TimerInterfaceImplementation(this, TimerPins);
      SPI = new SPIInterfaceImplementation(this, 6);
    }


    protected override bool IsValidDigitalPin(int pin)
    {
      return pin >= P0_0 && pin <= P1_7;
    }
  }
}
