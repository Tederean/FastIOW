using System;
using System.IO;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Represents a GPIO peripheral of an IOWarrior.
  /// </summary>
  public interface GPIO : Peripheral
  {

    /// <summary>
    /// Returns all GPIO capable pins on this IOWarrior.
    /// </summary>
    int[] SupportedPins { get; }

    /// <summary>
    /// Event that gets triggered when a pin changes its state.
    /// </summary>
    event EventHandler<PinStateChangeEventArgs> PinStateChange;

    /// <summary>
    /// Set input output pin to given state.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void DigitalWrite(int pin, PinState state);

    /// <summary>
    /// Returns state of input output pin.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    PinState DigitalRead(int pin);
  }

  /// <summary>
  /// Represents a digital state of a GPIO.
  /// </summary>
  public enum PinState
  {
    LOW = 0,
    HIGH = 1,
  }
}
