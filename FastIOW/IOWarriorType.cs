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
  /// Represents an IOWarrior model.
  /// </summary>
  public class IOWarriorType : Enumeration
  {

    /// <summary>
    /// Returns standard report size of this IOWarrior model.
    /// </summary>
    public int StandardReportSize { get; private set; }

    /// <summary>
    /// Returns special report size of this IOWarrior model.
    /// </summary>
    public int SpecialReportSize { get; private set; }

    /// <summary>
    /// Returns an array containing all pipe modes supported by this IOWarrior model.
    /// </summary>
    public Pipe[] SupportedPipes { get; private set; }

    /// <summary>
    /// Returns pipe used for I2C interface on this IOWarrior model.
    /// </summary>
    public Pipe I2CPipe { get; private set; }

    /// <summary>
    /// Maximum I2C packet length.
    /// </summary>
    public int I2CPacketLength { get; private set; }


    /// <summary>
    /// Represents an IOWarrior model that cannot be identified.
    /// </summary>
    public static readonly IOWarriorType Unknown = new IOWarriorType("Unknown", 0x0000, 64, 64, new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE }, Pipe.SPECIAL_MODE, 6);

    /// <summary>
    /// Represents an IOWarrior40 model.
    /// </summary>
    public static readonly IOWarriorType IOWarrior40 = new IOWarriorType("IOWarrior40", 0x1500, 5, 8, new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE }, Pipe.SPECIAL_MODE, 6);

    /// <summary>
    /// Represents an IOWarrior24 model.
    /// </summary>
    public static readonly IOWarriorType IOWarrior24 = new IOWarriorType("IOWarrior24", 0x1501, 3, 8, new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE }, Pipe.SPECIAL_MODE, 6);

    /// <summary>
    /// Represents an IOWarrior56 model.
    /// </summary>
    public static readonly IOWarriorType IOWarrior56 = new IOWarriorType("IOWarrior56", 0x1503, 8, 64, new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE }, Pipe.SPECIAL_MODE, 62);

    /// <summary>
    /// Represents an IOWarrior28 model.
    /// </summary>
    public static readonly IOWarriorType IOWarrior28 = new IOWarriorType("IOWarrior28", 0x1504, 5, 64, new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE, Pipe.I2C_MODE, Pipe.ADC_MODE }, Pipe.I2C_MODE, 62);

    /// <summary>
    /// Represents an IOWarrior28L model.
    /// </summary>
    public static readonly IOWarriorType IOWarrior28L = new IOWarriorType("IOWarrior28L", 0x1505, 5, 64, new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE }, Pipe.SPECIAL_MODE, 6);


    internal IOWarriorType(string Name, int Id, int StandardReportSize, int SpecialReportSize, Pipe[] SupportedPipes, Pipe I2CPipe, int I2CPacketLength)
      : base(Id, Name) 
    {
      this.StandardReportSize = StandardReportSize;
      this.SpecialReportSize = SpecialReportSize;
      this.SupportedPipes = SupportedPipes;
      this.I2CPipe = I2CPipe;
      this.I2CPacketLength = I2CPacketLength;
    }
  }
}
