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
  /// Represents a ADC peripheral of an IOWarrior.
  /// </summary>
  public interface ADC : Peripheral
  {

    /// <summary>
    /// Returns true if internal analog to digital converter is enabled, otherwise false.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Returns all ADC capable pins on this IOWarrior.
    /// </summary>
    int[] AnalogPins { get; }

    /// <summary>
    /// Enable ADC interface on this IOWarrior device.
    /// Set the channels that should be used.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="IOException"/>
    void Enable(ADCConfig config);

    /// <summary>
    /// Disable ADC interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Disable();

    /// <summary>
    /// Returns analog value of given pin. Value ranges from 0 to 65535, equals GND to VCC.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="IOException"/>
    ushort AnalogRead(int pin);
  }

  /// <summary>
  /// Represents a configuation of ADC channels.
  /// </summary>
  public enum ADCConfig
  {
    Channel_0 = 1,
    Channel_0To1 = 2,
    Channel_0To3 = 4,
    Channel_0To7 = 8,
  }
}
