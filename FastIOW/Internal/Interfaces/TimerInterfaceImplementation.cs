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

namespace Tederean.FastIOW.Internal
{

  public class TimerInterfaceImplementation : TimerInterface
  {

    public bool Enabled { get; private set; }

    private IOWarriorBase IOWarrior { get; set; }

    private int[] TimerPins { get; set; }

    private TimerConfig SelectedChannels { get; set; }


    internal TimerInterfaceImplementation(IOWarriorBase IOWarrior, int[] TimerPins)
    {
      this.IOWarrior = IOWarrior;
      this.TimerPins = TimerPins;
    }


    public void Enable(TimerConfig config)
    {
      throw new NotImplementedException();
    }

    public void Disable()
    {
      throw new NotImplementedException();
    }

    private int PinToChannelIndex(int pin)
    {
      return Array.IndexOf<int>(TimerPins, pin);
    }

    private bool IsChannelActivated(int pin)
    {
      int index = PinToChannelIndex(pin);

      return index > -1 && index < (int)SelectedChannels;
    }
  }
}
