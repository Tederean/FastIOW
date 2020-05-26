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
    /// Represents a pin state pulled to low potential by open drain circuit.
    /// </summary>
    bool LOW { get; }

    /// <summary>
    /// Represents a pin state pulled to high potential by pullup resistor.
    /// </summary>
    bool HIGH { get; }


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
    void DigitalWrite(int pin, bool state);

    /// <summary>
    /// Returns state of input output pin.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    bool DigitalRead(int pin);
  }
}
