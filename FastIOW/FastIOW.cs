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
using System.Diagnostics;
using Tederean.FastIOW.Internal;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Main class of FastIOW library - used to access all connected IOWarriors.
  /// </summary>
  public static class FastIOW
  {

    private static int m_DevHandle;

    private static readonly List<IOWarriorBase> m_IOWarriors = new List<IOWarriorBase>();

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

      try
      {
        m_DevHandle = NativeLib.IowKitOpenDevice();
      }
      catch (DllNotFoundException exception)
      {
        throw new InvalidOperationException("Cannot find iowkit.dll file! Ensure that it is located next to your application or in SysWOW64 respectively System32 folder.", exception);
      }

      if (m_DevHandle == 0x0) return false;

      int deviceCount = NativeLib.IowKitGetNumDevs();

      for (int index = 0; index < deviceCount; index++)
      {
        try
        {
          int handle = NativeLib.IowKitGetDeviceHandle(index + 1);
          IOWarriorType id = (IOWarriorType)NativeLib.IowKitGetProductId(handle);

          if (id == IOWarriorType.IOWarrior40)
          {
            m_IOWarriors.Add(new IOWarrior40(handle));
          }

          else if (id == IOWarriorType.IOWarrior24)
          {
            m_IOWarriors.Add(new IOWarrior24(handle));
          }

          else if (id == IOWarriorType.IOWarrior56)
          {
            m_IOWarriors.Add(new IOWarrior56(handle));
          }

          else if (id == IOWarriorType.IOWarrior28)
          {
            m_IOWarriors.Add(new IOWarrior28(handle));
          }

          else if (id == IOWarriorType.IOWarrior28L)
          {
            m_IOWarriors.Add(new IOWarrior28L(handle));
          }
        }
        catch (Exception ex)
        {
          var stack = ex.StackTrace;

          if (Debugger.IsAttached) Debugger.Break();
        }
      }

      return true;
    }

    /// <summary>
    /// Close connection for all connected IOWarriors.
    /// </summary>
    public static void CloseConnection()
    {
      m_IOWarriors.ForEach(entry => entry.CancelEvents());
      m_IOWarriors.ForEach(entry => entry.Disconnect());
      m_IOWarriors.Clear();

      if (m_DevHandle != 0x0)
      {
        NativeLib.IowKitCloseDevice(m_DevHandle);
        m_DevHandle = 0x0;
      }
    }

    /// <summary>
    /// Returns an array that contains all connected and supported IOWarriors at the same time.
    /// </summary>
    public static IOWarrior[] GetIOWarriors()
    {
      return m_IOWarriors.ToArray();
    }
  }
}
