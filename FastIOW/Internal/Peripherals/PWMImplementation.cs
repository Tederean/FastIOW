﻿using System;
using System.Linq;

namespace Tederean.FastIOW.Internal
{

  public class PWMImplementation : PWM
  {

    public bool Enabled { get; private set; }

    private IOWarriorBase IOWarriorBase { get; set; }

    public IOWarrior IOWarrior => IOWarriorBase;

    private int[] m_PWMPins;
    public int[] SupportedPins
    {
      get => m_PWMPins?.ToArray() ?? default;
      private set => m_PWMPins = value;
    }

    private byte[] PWMWriteReport { get; set; }

    internal PWMConfig SelectedChannels
    {
      get => (PWMConfig) PWMWriteReport[1];
      private set => PWMWriteReport[1] = (byte)value;
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


    internal PWMImplementation(IOWarriorBase IOWarriorBase, int[] PWMPins)
    {
      this.IOWarriorBase = IOWarriorBase;
      this.SupportedPins = PWMPins;

      // PWM setup: Output frequency ~ 732 Hz at 16bit resolution.
      PWMWriteReport = IOWarriorBase.NewReport(Pipe.SPECIAL_MODE);
      PWMWriteReport[0] = ReportId.PWM_SETUP;

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

      // Set to a secure state.
      Disable();
    }


    public void Enable(PWMConfig config)
    {
      lock (IOWarriorBase.SyncObject)
      {
        if (!Enum.IsDefined(typeof(PWMConfig), config)) throw new ArgumentException("Invalid channel.");

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior56)
        {
          if (config == PWMConfig.PWM_1To2)
          {
            if (IOWarriorBase.Revision < 0x2002) throw new InvalidOperationException("PWM_2 is only supported by IOWarrior firmware 2.0.0.2 or higher.");

            if (IOWarriorBase.GetPeripheral<SPI>()?.Enabled ?? false) throw new InvalidOperationException("PWM_2 cannot be used while SPI is enabled.");
          }
        }

        SelectedChannels = config;
        PWM1 = 0;
        PWM2 = 0;

        IOWarriorBase.WriteReport(PWMWriteReport, Pipe.SPECIAL_MODE);
        Enabled = true;
      }
    }

    public void Disable()
    {
      lock (IOWarriorBase.SyncObject)
      {
        SelectedChannels = 0x00; // Disable

        IOWarriorBase.WriteReport(PWMWriteReport, Pipe.SPECIAL_MODE);
        Enabled = false;
      }
    }

    public void AnalogWrite(int pin, ushort value)
    {
      lock (IOWarriorBase.SyncObject)
      {
        if (!Enabled) throw new InvalidOperationException("PWM interface is not enabled.");
        if (!Array.Exists<int>(SupportedPins, element => element == pin)) throw new ArgumentException("Not a PWM capable pin.");
        if (!IsChannelActivated(pin)) throw new ArgumentException("PWM channel not enabled.");

        int index = PinToChannelIndex(pin);

        if (index == 0) PWM1 = value;
        if (index == 1) PWM2 = value;

        IOWarriorBase.WriteReport(PWMWriteReport, Pipe.SPECIAL_MODE);
      }
    }

    private int PinToChannelIndex(int pin)
    {
      return Array.IndexOf<int>(SupportedPins, pin);
    }

    private bool IsChannelActivated(int pin)
    {
      int index = PinToChannelIndex(pin);

      return index > -1 && index < (int)SelectedChannels;
    }
  }
}
