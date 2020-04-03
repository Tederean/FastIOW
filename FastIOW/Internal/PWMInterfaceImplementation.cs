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

  public class PWMInterfaceImplementation : PWMInterface
  {

    public bool Enabled { get; private set; }

    private IOWarriorBase IOWarrior { get; set; }

    private int[] PWMPins { get; set; }

    private byte[] PWMWriteReport { get; set; }

    private PWMConfig SelectedChannels
    {
      get => (PWMConfig) PWMWriteReport[1];
      set => PWMWriteReport[1] = (byte)value;
    }

    private ushort PWM1
    {
      get => (ushort)((PWMWriteReport[5] << 8) + PWMWriteReport[4]);
      set
      {
        PWMWriteReport[4] = (byte)(0xFF & value);
        PWMWriteReport[5] = (byte)(value >> 8);
      }
    }

    private ushort PWM2
    {
      get => (ushort)((PWMWriteReport[10] << 8) + PWMWriteReport[9]);
      set
      {
        PWMWriteReport[9] = (byte)(0xFF & value);
        PWMWriteReport[10] = (byte)(value >> 8);
      }
    }


    internal PWMInterfaceImplementation(IOWarriorBase IOWarrior, int[] PWMPins)
    {
      this.IOWarrior = IOWarrior;
      this.PWMPins = PWMPins;

      // PWM setup: Output frequency ~ 732 Hz at 16bit resolution.
      PWMWriteReport = IOWarrior.NewReport(Pipe.SPECIAL_MODE);
      PWMWriteReport[0] = 0x20; // PWM

      // Set Per1 to 65535
      PWMWriteReport[2] = 0xFF;
      PWMWriteReport[3] = 0xFF;

      // PWM1 Master Clock 48 MHz
      PWMWriteReport[6] = 0x03;

      // Set Per2 to 65535
      PWMWriteReport[7] = 0xFF;
      PWMWriteReport[8] = 0xFF;

      // PWM2 Master Clock 48 MHz
      PWMWriteReport[11] = 0x03;
    }


    public void Enable(PWMConfig config)
    {
      if (!Enum.IsDefined(typeof(PWMConfig), config)) throw new ArgumentException("Invalid channel.");

      SelectedChannels = config;
      PWM1 = 0;
      PWM2 = 0;

      IOWarrior.WriteReport(PWMWriteReport, Pipe.SPECIAL_MODE);
      Enabled = true;
    }

    public void Disable()
    {
      if (!Enabled) return;

      SelectedChannels = 0x00; // Disable

      IOWarrior.WriteReport(PWMWriteReport, Pipe.SPECIAL_MODE);
      Enabled = false;
    }

    public void AnalogWrite(int pin, ushort value)
    {
      if (!Enabled) throw new InvalidOperationException("PWM is not enabled.");
      if (!IsChannelActivated(pin)) throw new ArgumentException("Not an PWM pin or just not enabled.");

      int index = PinToChannelIndex(pin);

      if (index == 0) PWM1 = value;
      if (index == 1) PWM2 = value;

      IOWarrior.WriteReport(PWMWriteReport, Pipe.SPECIAL_MODE);
    }

    private int PinToChannelIndex(int pin)
    {
      return Array.IndexOf<int>(PWMPins, pin);
    }

    private bool IsChannelActivated(int pin)
    {
      int index = PinToChannelIndex(pin);

      return index > -1 && index < (int)SelectedChannels;
    }
  }
}
