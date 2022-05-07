using System;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Method extension class to modify a single bit in a byte.
  /// Just import it by using directive, enjoy additional methods for byte objects.
  /// </summary>
  public static class ByteExtensions
  {

    private static void CheckIndex(int index)
    {
      if (index < 0 || index > 7)
      {
        throw new IndexOutOfRangeException("Byte index out of range. Allowed 0-7, given: " + index);
      }
    }

    /// <summary>
    /// Modify a single bit in a byte. Set a bit specified by index to true or false.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"/>
    public static void SetBit(this ref byte value, int index, bool state)
    {
      CheckIndex(index);

      int targetInt = value;

      if (state)
      {
        targetInt |= (1 << index); // Bitwise OR
      }
      else
      {
        targetInt &= ~(1 << index); // Bitwise AND
      }

      value = (byte)targetInt;
    }

    /// <summary>
    /// Modify a single bit in a byte. Toggle a bit specified by index.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"/>
    public static void ToggleBit(this ref byte value, int index)
    {
      CheckIndex(index);

      int targetInt = value;

      targetInt ^= (1 << index); // Bitwise Invert

      value = (byte)targetInt;
    }

    /// <summary>
    /// Returns a single bit in a byte, specified by index.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"/>
    public static bool GetBit(this byte value, int index)
    {
      CheckIndex(index);

      return Convert.ToBoolean(value & (1 << index));
    }

    /// <summary>
    /// Apply OR bit masking to a byte. The bit mask can be inverted before masking.
    /// Returns the masking result.
    /// </summary>
    public static byte MaskOr(this byte value, byte bitmask, bool invertBitmask = false)
    {
      var intValue = (int)value;
      var intBitmask = (int)bitmask;

      if (invertBitmask)
      {
        intBitmask = ~intBitmask;
      }

      var intResult = intValue | intBitmask;

      return Convert.ToByte(intResult & 0xFF);
    }

    /// <summary>
    /// Apply AND bit masking to a byte. The bit mask can be inverted before masking.
    /// Returns the masking result.
    /// </summary>
    public static byte MaskAnd(this byte value, byte bitmask, bool invertBitmask = false)
    {
      var intValue = (int)value;
      var intBitmask = (int)bitmask;

      if (invertBitmask)
      {
        intBitmask = ~intBitmask;
      }

      var intResult = intValue & intBitmask;

      return Convert.ToByte(intResult & 0xFF);
    }
  }
}
