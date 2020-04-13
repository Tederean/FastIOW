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
namespace Tederean.FastIOW
{

  /// <summary>
  /// A report id represents the first byte in an IOWarrior report.
  /// </summary>
  public static class ReportId
  {

    public const byte ADC_SETUP = 0x1C;
    public const byte ADC_READ = 0x1D;

    public const byte I2C_SETUP = 0x01;
    public const byte I2C_WRITE = 0x02;
    public const byte I2C_READ = 0x03;

    public const byte PWM_SETUP = 0x20;

    public const byte SPI_SETUP = 0x08;
    public const byte SPI_TRANSFER = 0x09;

    public const byte TIMER_SETUP = 0x28;
    public const byte TIMER_DATA_A = 0x29;
    public const byte TIMER_DATA_B = 0x2A;

    public const byte GPIO_READ_WRITE = 0x00;
    public const byte GPIO_SPECIAL_READ = 0xFF;
  }
}