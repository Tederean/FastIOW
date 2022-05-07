using System;
using System.IO;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Represents a ADC peripheral of an IOWarrior.
  /// </summary>
  public interface ADC : Peripheral
  {

    /// <summary>
    /// Returns true if internal analog to digital converter is enabled, otherwise false.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Returns all ADC capable pins on this IOWarrior.
    /// </summary>
    int[] SupportedPins { get; }

    /// <summary>
    /// Enable ADC interface on this IOWarrior device.
    /// Set the channels that should be used.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="IOException"/>
    void Enable(ADCConfig config);

    /// <summary>
    /// Disable ADC interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Disable();

    /// <summary>
    /// Returns analog value of given pin. Value ranges from 0 to 65535, equals GND to VCC.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="IOException"/>
    ushort AnalogRead(int pin);
  }

  /// <summary>
  /// Represents a configuation of ADC channels.
  /// </summary>
  public enum ADCConfig
  {
    Channel_0 = 1,
    Channel_0To1 = 2,
    Channel_0To3 = 4,
    Channel_0To7 = 8,
  }
}
