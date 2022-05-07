using System;
using System.IO;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Represents a PWM peripheral of an IOWarrior.
  /// </summary>
  public interface PWM : Peripheral
  {

    /// <summary>
    /// Returns true if PWM interface is enabled, otherwise false.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Returns all PWM capable pins on this IOWarrior.
    /// </summary>
    int[] SupportedPins { get; }

    /// <summary>
    /// Enable PWM interface on this IOWarrior device.
    /// Set the channels that should be used.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Enable(PWMConfig config);

    /// <summary>
    /// Disable PWM interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Disable();

    /// <summary>
    /// Write analog 16bit PWM value to given pin.
    /// Range is from 0 to 65535.
    /// Frequency is approximately 732 Hz.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="IOException"/>
    void AnalogWrite(int pin, ushort value);
  }

  /// <summary>
  /// Represents a configuation of PWM channels.
  /// </summary>
  public enum PWMConfig
  {
    PWM_1 = 1,
    PWM_1To2 = 2,
  }
}
