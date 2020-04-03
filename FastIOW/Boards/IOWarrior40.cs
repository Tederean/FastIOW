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
  public class IOWarrior40 : IOWarriorBase, I2CDevice
  {

    public override string Name => "IOWarrior40";

    public override IOWarriorType Type => IOWarriorType.IOWarrior40;

    protected override int StandardReportSize => 5;

    protected override int SpecialReportSize => 8;

    protected override Pipe[] SupportedPipes => new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE };

    public I2CInterface I2C { get; private set; }


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

    public const int P2_0 = 3 * 8 + 0;
    public const int P2_1 = 3 * 8 + 1;
    public const int P2_2 = 3 * 8 + 2;
    public const int P2_3 = 3 * 8 + 3;
    public const int P2_4 = 3 * 8 + 4;
    public const int P2_5 = 3 * 8 + 5;
    public const int P2_6 = 3 * 8 + 6;
    public const int P2_7 = 3 * 8 + 7;

    public const int P3_0 = 4 * 8 + 0;
    public const int P3_1 = 4 * 8 + 1;
    public const int P3_2 = 4 * 8 + 2;
    public const int P3_3 = 4 * 8 + 3;
    public const int P3_4 = 4 * 8 + 4;
    public const int P3_5 = 4 * 8 + 5;
    public const int P3_6 = 4 * 8 + 6;
    public const int P3_7 = 4 * 8 + 7;

    public const int I2C_SCL = P0_6;
    public const int I2C_SDA = P0_7;

    public const int BUILDIN_BUTTON = P0_0;

    public const int BUILDIN_LED0 = P3_0;
    public const int BUILDIN_LED1 = P3_1;
    public const int BUILDIN_LED2 = P3_2;
    public const int BUILDIN_LED3 = P3_3;
    public const int BUILDIN_LED4 = P3_4;
    public const int BUILDIN_LED5 = P3_5;
    public const int BUILDIN_LED6 = P3_6;
    public const int BUILDIN_LED7 = P3_7;


    internal IOWarrior40(int handle) : base(handle)
    {
      I2C = new I2CInterfaceImplementation(this, Pipe.SPECIAL_MODE, 6);
    }


    protected override bool IsValidDigitalPin(int pin)
    {
      return pin >= P0_0 && pin <= P3_7;
    }
  }
}
