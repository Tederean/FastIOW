using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Tederean.FastIOW.Internal;

namespace Tederean.FastIOW
{

  /// <summary>
  /// Main class of FastIOW library - used to access all connected IOWarriors.
  /// </summary>
  public static class FastIOW
  {

    private static readonly object SyncObject = new object();

    private static IntPtr m_DevHandle;

    private static readonly List<IOWarriorBase> m_IOWarriors = new List<IOWarriorBase>();

    /// <summary>
    /// Returns true if connected to at least one IOWarrior, otherwise false.
    /// </summary>
    public static bool Connected => m_DevHandle != IntPtr.Zero;

    /// <summary>
    /// Connect to all available IOWarriors. Returns true if
    /// connected to at least one IOWarrior, otherwise false.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public static bool OpenConnection()
    {
      lock (SyncObject)
      {
        if (m_DevHandle != IntPtr.Zero)
          throw new InvalidOperationException("Connection already opened.");

        try
        {
          IowkitLibraryHandler.ExtractNativeLibrary();

          m_DevHandle = IowkitLibrary.IowKitOpenDevice();

          if (m_DevHandle == IntPtr.Zero) return false;

          uint deviceCount = IowkitLibrary.IowKitGetNumDevs();

          for (uint index = 0; index < deviceCount; index++)
          {
            TryInitDevice(index);
          }
        }
        catch (Exception ex)
        {
          ThrowInternalException(ex);
        }

        return m_IOWarriors.Count > 0;
      }
    }

    private static void TryInitDevice(uint index)
    {
      for (int counter = 0; counter < 3; counter++)
      {
        if (InitDevice(index)) return;

        Thread.Sleep(10);
      }

      // Error initializing an IOWarrior!
      if (Debugger.IsAttached) Debugger.Break();
    }

    private static bool InitDevice(uint index)
    {
      try
      {
        IntPtr handle = IowkitLibrary.IowKitGetDeviceHandle(index + 1);
        IOWarriorType id = (IOWarriorType)IowkitLibrary.IowKitGetProductId(handle);

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

        return true;
      }
      catch (IOException)
      {
        return false;
      }
    }

    /// <summary>
    /// Close connection for all connected IOWarriors.
    /// </summary>
    public static void CloseConnection()
    {
      lock (SyncObject)
      {
        m_IOWarriors.ForEach(entry => entry.CancelEvents());
        m_IOWarriors.ForEach(entry => entry.Disconnect());
        m_IOWarriors.Clear();

        if (m_DevHandle != IntPtr.Zero)
        {
          try
          {
            IowkitLibrary.IowKitCloseDevice(m_DevHandle);
          }
          catch (Exception ex)
          {
            ThrowInternalException(ex);
          }
          finally
          {
            m_DevHandle = IntPtr.Zero;
          }
        }
      }
    }

    private static void ThrowInternalException(Exception ex)
    {
      if (ex is DllNotFoundException)
      {
        throw new InvalidOperationException("Cannot find iowkit.dll file! Ensure that it is located next to your application or in System32 folder.", ex);
      }

      else if (ex is SEHException || ex is AccessViolationException)
      {
        throw new InvalidOperationException("iowkit.dll caused segmentation fault! Try to reinsert your IOWarrior USB devices or reboot your system.", ex);
      }

      if (ex is BadImageFormatException)
      {
        var arch = Environment.Is64BitProcess ? 64 : 32;

        throw new InvalidOperationException("iowkit.dll cannot be loaded! Take care that you installed the " + arch + "-bit iowkit version. Check that your project is based on NET-Framework, NET-Core isn't supported.", ex);
      }

      else throw ex;
    }

    /// <summary>
    /// Returns an array that contains all connected and supported IOWarriors at the same time.
    /// </summary>
    public static IOWarrior[] GetIOWarriors()
    {
      lock (SyncObject)
      {
        return m_IOWarriors.ToArray();
      }
    }

    /// <summary>
    /// Returns an arry of all supported peripherals using all connected devices.
    /// </summary>
    public static T[] GetPeripherals<T>() where T : Peripheral
    {
      lock (SyncObject)
      {
        var peripherals = new List<T>();

        foreach (var iow in m_IOWarriors)
        {
          peripherals.AddRange(iow.InterfaceList.Where(entry => entry is T).Cast<T>());
        }

        return peripherals.ToArray();
      }
    }
  }
}
