/*
 *   
 *   Copyright 2020 Florian Porsch <tederean@gmail.com>
 *   
 *   This library is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation; either version 3 of the License, or
 *   (at your option) any later version.
 *   
 *   This library is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Lesser General Public License for more details.
 *   
 *   You should have received a copy of the GNU Lesser General Public License
 *   along with this program; if not, write to the Free Software
 *   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 *   MA 02110-1301 USA.
 *
 */
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
    public static void SetBit(this ref byte report, int index, bool state)
    {
      CheckIndex(index);

      int targetInt = report;

      if (state)
      {
        targetInt |= (1 << index); // Bitwise OR
      }
      else
      {
        targetInt &= ~(1 << index); // Bitwise AND
      }

      report = (byte)targetInt;
    }

    /// <summary>
    /// Modify a single bit in a byte. Toggle a bit specified by index.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"/>
    public static void ToggleBit(this ref byte report, int index)
    {
      CheckIndex(index);

      int targetInt = report;

      targetInt ^= (1 << index); // Bitwise Invert

      report = (byte)targetInt;
    }

    /// <summary>
    /// Returns a single bit in a byte, specified by index.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"/>
    public static bool GetBit(this ref byte report, int index)
    {
      CheckIndex(index);

      return Convert.ToBoolean(report & (1 << index));
    }
  }
}
