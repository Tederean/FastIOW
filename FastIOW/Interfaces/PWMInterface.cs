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
  /// Represents a PWM interface of an IOWarrior.
  /// </summary>
  public interface PWMInterface
  {

    /// <summary>
    /// Returns true if PWM interface is enabled, otherwise false.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Enable PWM interface on this IOWarrior device.
    /// Set the channels that should be used.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Enable(PWMConfig config);

    /// <summary>
    /// Disable PWM interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Disable();

    /// <summary>
    /// Write analog 16bit PWM value to given pin.
    /// Range is from 0 to 65535.
    /// Frequency is approximately 732 Hz.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="IOException"/>
    void AnalogWrite(int pin, ushort value);
  }

  /// <summary>
  /// Represents a configuation of ADC channels.
  /// </summary>
  public enum PWMConfig
  {
    PWM_1 = 1,
    PWM_1To2 = 2,
  }
}
