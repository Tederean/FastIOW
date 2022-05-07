using System;
using System.IO;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Represents a I2C peripheral of an IOWarrior.
  /// </summary>
  public interface I2C : Peripheral
  {

    /// <summary>
    /// Returns true if I2C interface is enabled, otherwise false.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Enable I2C interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Enable();

    /// <summary>
    /// Disable I2C interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Disable();

    /// <summary>
    /// Check if slave for given 7bit I2C address is responding.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    bool IsAvailable(byte address);

    /// <summary>
    /// Write bytes to given 7bit I2C address - msb first.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void WriteBytes(byte address, params byte[] data);

    /// <summary>
    /// Read in bytes from given 7bit I2C address. Size of returned
    /// read in array (msb first) equals parameter length.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    byte[] ReadBytes(byte address, int length);

    /// <summary>
    /// Read in single byte from given 7bit I2C address.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    byte ReadByte(byte address);

    /// <summary>
    /// Returns read in two bytes from given 7bit I2C address.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    ushort Read2Bytes(byte address);

    /// <summary>
    /// Returns read in three bytes from given 7bit I2C address.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    uint Read3Bytes(byte address);

    /// <summary>
    /// Returns read in four bytes from given 7bit I2C address.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    uint Read4Bytes(byte address);
  }
}
