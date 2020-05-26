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
  /// Represents a pysical IOWarrior device.
  /// </summary>
  public interface IOWarrior
  {

    /// <summary>
    /// Returns the name of this device.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns IOWarrior model type of this device.
    /// </summary>
    IOWarriorType Type { get; }

    /// <summary>
    /// Returns the id of this device.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Returns unique serial number of this device.
    /// </summary>
    string SerialNumber { get; }

    /// <summary>
    /// Returns true if this device is connected, otherwise false.
    /// </summary>
    bool Connected { get; }

    /// <summary>
    /// Returns all supported peripherals of this IOWarrior device.
    /// </summary>
    T[] GetPeripherals<T>() where T : Peripheral;

    /// <summary>
    /// Returns first peripheral of this IOWarrior device, matching the given Type T or returns null if not supported.
    /// </summary>
    T GetPeripheral<T>() where T : Peripheral;
  }
}
