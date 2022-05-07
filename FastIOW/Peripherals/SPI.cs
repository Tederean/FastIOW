using System;
using System.IO;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Represents a SPI peripheral of an IOWarrior.
  /// </summary>
  public interface SPI : Peripheral
  {

    /// <summary>
    /// Returns true if SPI interface is enabled, otherwise false.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Enable SPI interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Enable();

    /// <summary>
    /// Disable SPI interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Disable();

    /// <summary>
    /// Writes given bytes to SPI slave - msb first.
    /// Returns the recieved bytes from slave - msb first.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    byte[] TransferBytes(params byte[] data);
  }
}
