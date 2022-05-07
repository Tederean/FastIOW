using System;
using System.Diagnostics;
using System.Linq;

namespace Tederean.FastIOW.Internal
{

  public class ADCImplementation : ADC
  {

    public bool Enabled { get; private set; }

    private IOWarriorBase IOWarriorBase { get; set; }

    public IOWarrior IOWarrior => IOWarriorBase;

    private Pipe ADCPipe { get; set; }

    private int[]  m_AnalogPins;
    public int[] SupportedPins
    { 
      get => m_AnalogPins?.ToArray() ?? default;
      private set => m_AnalogPins = value;
    }

    private ADCConfig SelectedChannels { get; set; }


    internal ADCImplementation(IOWarriorBase IOWarriorBase, Pipe ADCPipe, int[] AnalogPins)
    {
      this.IOWarriorBase = IOWarriorBase;
      this.ADCPipe = ADCPipe;
      this.SupportedPins = AnalogPins;

      // Set to a secure state.
      Disable();
    }


    public void Enable(ADCConfig config)
    {
      lock (IOWarriorBase.SyncObject)
      {
        if (!Enum.IsDefined(typeof(ADCConfig), config)) throw new ArgumentException("Invalid channel.");

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior28)
        {
          SelectedChannels = (ADCConfig)Math.Min((byte)config, (byte)ADCConfig.Channel_0To3);
        }

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior56)
        {
          SelectedChannels = (ADCConfig)Math.Min((byte)config, (byte)ADCConfig.Channel_0To7);
        }

        Disable(); // ADC has be disabled before changing adc setup.

        var report = IOWarriorBase.NewReport(ADCPipe);

        report[0] = ReportId.ADC_SETUP;
        report[1] = 0x01; // Enable
        report[2] = (byte)SelectedChannels; // Channel size

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior28)
        {
          report[5] = 0x01; // Continuous measuring
          report[6] = 0x04; // Sample rate of 6 kHz
        }

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior56)
        {
          report[3] = 0x02; // Measurement range from GND to VCC.
        }

        IOWarriorBase.WriteReport(report, ADCPipe);
        Enabled = true;
      }
    }

    public void Disable()
    {
      lock (IOWarriorBase.SyncObject)
      {
        var report = IOWarriorBase.NewReport(ADCPipe);

        report[0] = ReportId.ADC_SETUP;
        report[1] = 0x00; // ADC Disable

        IOWarriorBase.WriteReport(report, ADCPipe);
        Enabled = false;
      }
    }

    public ushort AnalogRead(int pin)
    {
      lock (IOWarriorBase.SyncObject)
      {
        if (!Enabled) throw new InvalidOperationException("ADC interface is not enabled.");
        if (!Array.Exists<int>(SupportedPins, element => element == pin)) throw new ArgumentException("Not an ADC capable pin.");
        if (!IsChannelActivated(pin)) throw new ArgumentException("ADC channel not enabled.");

        var result = IOWarriorBase.ReadReport(ADCPipe);

        if (result[0] != ReportId.ADC_READ)
        {
          if (Debugger.IsAttached) Debugger.Break();

          throw new InvalidOperationException("Recieved wrong packet!");
        }

        int index = PinToChannelIndex(pin);

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior56)
        {
          var lsb = result[1 + 2 * index];
          var msb = result[2 + 2 * index];

          // Shift by 2 to increase resolution from 14bit to 16bit.
          return (ushort)(((msb << 8) + lsb) << 2);
        }

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior28)
        {
          var lsb = result[2 + 2 * index];
          var msb = result[3 + 2 * index];

          // Shift by 4 to increase resolution from 12bit to 16bit.
          return (ushort)(((msb << 8) + lsb) << 4);
        }

        throw new NotImplementedException();
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
