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
using System.Diagnostics;
using System.Linq;

namespace Tederean.FastIOW.Internal
{

  public class ADCInterfaceImplementation : ADCInterface
  {

    public bool Enabled { get; private set; }

    private IOWarriorBase IOWarrior { get; set; }

    private Pipe ADCPipe { get; set; }

    private int[]  m_AnalogPins;
    public int[] AnalogPins
    { 
      get => m_AnalogPins?.ToArray() ?? default;
      private set => m_AnalogPins = value;
    }

    private ADCConfig SelectedChannels { get; set; }


    internal ADCInterfaceImplementation(IOWarriorBase IOWarrior, Pipe ADCPipe, int[] AnalogPins)
    {
      this.IOWarrior = IOWarrior;
      this.ADCPipe = ADCPipe;
      this.AnalogPins = AnalogPins;

      // Set to a secure state.
      Enabled = true;
      Disable();
    }


    public void Enable(ADCConfig config)
    {
      if (!Enum.IsDefined(typeof(ADCConfig), config)) throw new ArgumentException("Invalid channel.");

      if (IOWarrior.Type == IOWarriorType.IOWarrior28)
      {
        SelectedChannels = (ADCConfig)Math.Min((byte)config, (byte)ADCConfig.Channel_0To3);
      }

      if (IOWarrior.Type == IOWarriorType.IOWarrior56)
      {
        SelectedChannels = (ADCConfig)Math.Min((byte)config, (byte)ADCConfig.Channel_0To7);
      }

      Disable(); // ADC has be disabled before changing adc setup.

      var report = IOWarrior.NewReport(ADCPipe);

      report[0] = ReportId.ADC_SETUP;
      report[1] = 0x01; // Enable
      report[2] = (byte)SelectedChannels; // Channel size

      if (IOWarrior.Type == IOWarriorType.IOWarrior28)
      {
        report[5] = 0x01; // Continuous measuring
        report[6] = 0x04; // Sample rate of 6 kHz
      }

      if (IOWarrior.Type == IOWarriorType.IOWarrior56)
      {
        report[3] = 0x02; // Measurement range from GND to VCC.
      }

      IOWarrior.WriteReport(report, ADCPipe);
      Enabled = true;
    }

    public void Disable()
    {
      if (!Enabled) return;

      var report = IOWarrior.NewReport(ADCPipe);

      report[0] = ReportId.ADC_SETUP;
      report[1] = 0x00; // ADC Disable

      IOWarrior.WriteReport(report, ADCPipe);
      Enabled = false;
    }

    public ushort AnalogRead(int pin)
    {
      if (!Enabled) throw new InvalidOperationException("ADC interface is not enabled.");
      if (!Array.Exists<int>(AnalogPins, element => element == pin)) throw new ArgumentException("Not an ADC capable pin.");
      if (!IsChannelActivated(pin)) throw new ArgumentException("ADC channel not enabled.");

      var result = IOWarrior.ReadReport(ADCPipe);

      if (result[0] != ReportId.ADC_READ)
      {
        if (Debugger.IsAttached) Debugger.Break();

        throw new InvalidOperationException("Recieved wrong packet!");
      }

      int index = PinToChannelIndex(pin);

      if (IOWarrior.Type == IOWarriorType.IOWarrior56)
      {
        var lsb = result[1 + 2 * index];
        var msb = result[2 + 2 * index];

        // Shift by 2 to increase resolution from 14bit to 16bit.
        return (ushort)(((msb << 8) + lsb) << 2);
      }

      if (IOWarrior.Type == IOWarriorType.IOWarrior28)
      {
        var lsb = result[2 + 2 * index];
        var msb = result[3 + 2 * index];

        // Shift by 4 to increase resolution from 12bit to 16bit.
        return (ushort)(((msb << 8) + lsb) << 4);
      }

      throw new NotImplementedException();
    }

    private int PinToChannelIndex(int pin)
    {
      return Array.IndexOf<int>(AnalogPins, pin);
    }

    private bool IsChannelActivated(int pin)
    {
      int index = PinToChannelIndex(pin);

      return index > -1 && index < (int)SelectedChannels;
    }
  }
}
