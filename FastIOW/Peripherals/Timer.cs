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
    int[] SupportedPins { get; }

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
