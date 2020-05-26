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
  /// Represents a Timer peripheral of an IOWarrior.
  /// </summary>
  public interface Timer : Peripheral
  {

    /// <summary>
    /// Returns all Timer capable pins on this IOWarrior.
    /// </summary>
    int[] TimerPins { get; }

    /// <summary>
    /// Reads a pulse on a pin in micro seconds. True for a positive pulse,
    /// false for a negative pulse. Gives up after 1 second, returning -1.
    /// Polling rate set to 10 milliseconds.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    int PulseIn(int pin, bool value);

    /// <summary>
    /// Reads a pulse on a pin in micro seconds. True for a positive pulse,
    /// false for a negative pulse. Gives up after a defined timeout, returning -1.
    /// Polling rate set to 10 milliseconds.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    int PulseIn(int pin, bool value, TimeSpan timeout);

    /// <summary>
    /// Reads a pulse on a pin in microseconds. True for a positive pulse,
    /// false for a negative pulse. Gives up after a defined timeout, returning -1.
    /// Polling rate is defined with interval.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    int PulseIn(int pin, bool value, TimeSpan timeout, TimeSpan interval);
  }
}
