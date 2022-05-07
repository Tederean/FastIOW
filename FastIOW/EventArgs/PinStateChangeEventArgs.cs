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
