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

namespace Tederean.FastIOW
{

  /// <summary>
  /// Event args for PinStateChange event.
  /// </summary>
  public class PinStateChangeEventArgs : EventArgs
  {

    /// <summary>
    /// Get the interface whose pin state has changed.
    /// </summary>
    public GPIO GPIO { get; private set; }

    /// <summary>
    /// Get the device whose pin state has changed.
    /// </summary>
    public IOWarrior IOWarrior => GPIO.IOWarrior;

    /// <summary>
    /// Get the pin who has changed.
    /// </summary>
    public int Pin { get; private set; }

    /// <summary>
    /// State that is pin switching to.
    /// </summary>
    public PinState NewPinState { get; private set; }

    /// <summary>
    /// State that is pin switching from.
    /// </summary>
    public PinState OldPinState { get; private set; }


    internal PinStateChangeEventArgs(GPIO GPIO, int Pin, PinState NewPinState, PinState OldPinState)
    {
      this.GPIO = GPIO;
      this.Pin = Pin;
      this.NewPinState = NewPinState;
      this.OldPinState = OldPinState;
    }
  }
}
