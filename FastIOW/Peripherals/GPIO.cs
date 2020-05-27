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
  /// Represents a GPIO peripheral of an IOWarrior.
  /// </summary>
  public interface GPIO : Peripheral
  {

    /// <summary>
    /// Returns all GPIO capable pins on this IOWarrior.
    /// </summary>
    int[] SupportedPins { get; }

    /// <summary>
    /// Event that gets triggered when a pin changes its state.
    /// </summary>
    event EventHandler<PinStateChangeEventArgs> PinStateChange;

    /// <summary>
    /// Set input output pin to given state.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void DigitalWrite(int pin, PinState state);

    /// <summary>
    /// Returns state of input output pin.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    PinState DigitalRead(int pin);
  }

  /// <summary>
  /// Represents a digital state of a GPIO.
  /// </summary>
  public enum PinState
  {
    LOW = 0,
    HIGH = 1,
  }
}
