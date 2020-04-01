/*
 *   
 *   Copyright 2020 Florian Porsch <tederean@gmail.com>
 *   
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation; either version 3 of the License, or
 *   (at your option) any later version.
 *   
 *   This program is distributed in the hope that it will be useful,
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
using System.Collections.Generic;
using Tederean.FastIOW.Internal;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Main class of FastIOW library - used to access all connected IOWarriors.
  /// </summary>
  public static class FastIOW
  {

    private static int m_DevHandle;

    private static readonly List<IOWarrior> m_IOWarriors = new List<IOWarrior>();

    /// <summary>
    /// Returns true if connected to at least one IOWarrior, otherwise false.
    /// </summary>
    public static bool Connected => m_DevHandle != 0x0;

    /// <summary>
    /// Connect to all available IOWarriors. Returns true if
    /// connected to at least one IOWarrior, otherwise false.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public static bool OpenConnection()
    {
      if (m_DevHandle != 0x0)
        throw new InvalidOperationException("Connection already opened.");

      m_DevHandle = NativeLib.IowKitOpenDevice();

      if (m_DevHandle == 0x0) return false;

      int deviceCount = NativeLib.IowKitGetNumDevs();

      for (int index = 0; index < deviceCount; index++)
      {
        m_IOWarriors.Add(new IOWarrior(index));
      }

      return true;
    }

    /// <summary>
    /// Close connection for all connected IOWarriors.
    /// </summary>
    public static void CloseConnection()
    {
      m_IOWarriors.ForEach(entry => entry.Disconnect());
      m_IOWarriors.Clear();

      if (m_DevHandle != 0x0)
      {
        NativeLib.IowKitCloseDevice(m_DevHandle);
        m_DevHandle = 0x0;
      }
    }

    /// <summary>
    /// Returns an array containing all connected IOWarriors.
    /// </summary>
    public static IOWarrior[] GetIOWarriors()
    {
      return m_IOWarriors.ToArray();
    }
  }
}
