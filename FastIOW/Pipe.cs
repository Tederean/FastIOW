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

  /// <summary>
  /// A pipe that can be used in IOWarrior ecosystem.
  /// </summary>
  public class Pipe : Enumeration
  {

    /// <summary>
    /// Pipe used for addressing special mode functions.
    /// </summary>
    public static readonly Pipe IO_PINS = new Pipe(0, "IO_PINS");

    /// <summary>
    /// Pipe used for addressing input output pins.
    /// </summary>
    public static readonly Pipe SPECIAL_MODE = new Pipe(1, "SPECIAL_MODE");

    /// <summary>
    /// Pipe used for addressing hardware I2C interface. Pipe only supported by IOWarrior28.
    /// </summary>
    public static readonly Pipe I2C_MODE = new Pipe(2, "I2C_MODE");

    /// <summary>
    /// Pipe used for addressing buildin analog to digital converter. Pipe only supported by IOWarrior28.
    /// </summary>
    public static readonly Pipe ADC_MODE = new Pipe(3, "ADC_MODE");


    internal Pipe(uint Id, string Name) : base(Id, Name) { }
  }
}