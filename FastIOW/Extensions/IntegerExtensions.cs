using System;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Method extension class to modify a single bit in an int.
  /// Just import it by using directive, enjoy additional methods for int objects.
  /// </summary>
  public static class IntegerExtensions
  {

    private static void CheckIndex(int index)
    {
      if (index < 0 || index > 31)
      {
        throw new IndexOutOfRangeException("Integer index out of range. Allowed 0-31, given: " + index);
      }
    }

    /// <summary>
    /// Modify a single bit in an int. Set a bit specified by index to true or false.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"/>
    public static void SetBit(this ref int value, int index, bool state)
    {
      CheckIndex(index);

      if (state)
      {
        value |= (1 << index); // Bitwise OR
      }
      else
      {
        value &= ~(1 << index); // Bitwise AND
      }
    }

    /// <summary>
    /// Modify a single bit in an int. Toggle a bit specified by index.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"/>
    public static void ToggleBit(this ref int value, int index)
    {
      CheckIndex(index);

      value ^= (1 << index); // Bitwise Invert
    }

    /// <summary>
    /// Returns a single bit in an int, specified by index.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"/>
    public static bool GetBit(this int value, int index)
    {
      CheckIndex(index);

      return Convert.ToBoolean(value & (1 << index));
    }
  }
}
