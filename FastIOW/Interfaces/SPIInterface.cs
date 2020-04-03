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
using System;
using System.IO;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Represents a SPI interface of an IOWarrior.
  /// </summary>
  public interface SPIInterface
  {

    /// <summary>
    /// Returns true if SPI interface is enabled, otherwise false.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Enable SPI interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Enable();

    /// <summary>
    /// Disable SPI interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Disable();

    /// <summary>
    /// Writes given bytes to SPI slave - msb first.
    /// Returns the recieved bytes from slave - msb first.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    byte[] TransferBytes(params byte[] data);
  }
}
