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
  /// Represents a I2C interface of an IOWarrior.
  /// </summary>
  public interface I2CInterface
  {

    /// <summary>
    /// Returns true if I2C interface is enabled, otherwise false.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Enable I2C interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Enable();

    /// <summary>
    /// Disable I2C interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Disable();

    /// <summary>
    /// Write bytes to given 7bit I2C address.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void WriteBytes(byte address, params byte[] data);

    /// <summary>
    /// Read in bytes from given 7bit I2C address. Size of
    /// returned read in array equals parameter length.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    byte[] ReadBytes(byte address, int length);

    /// <summary>
    /// Returns read in two bytes from given 7bit I2C address.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    ushort Read2Bytes(byte address);
  }
}
