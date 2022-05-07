namespace Tederean.FastIOW
{

  /// <summary>
  /// Represents a pysical IOWarrior device.
  /// </summary>
  public interface IOWarrior
  {

    /// <summary>
    /// Returns the name of this device.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns IOWarrior model type of this device.
    /// </summary>
    IOWarriorType Type { get; }

    /// <summary>
    /// Returns the id of this device.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Returns unique serial number of this device.
    /// </summary>
    string SerialNumber { get; }

    /// <summary>
    /// Returns true if this device is connected, otherwise false.
    /// </summary>
    bool Connected { get; }

    /// <summary>
    /// Returns all supported peripherals of this IOWarrior device.
    /// </summary>
    T[] GetPeripherals<T>() where T : Peripheral;

    /// <summary>
    /// Returns first peripheral of this IOWarrior device, matching the given Type T or returns null if not supported.
    /// </summary>
    T GetPeripheral<T>() where T : Peripheral;
  }
}
