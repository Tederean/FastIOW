using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tederean.FastIOW.Internal
{

  public abstract class IOWarriorBase : IOWarrior
  {

    public abstract string Name { get; }

    public abstract IOWarriorType Type { get; }

    public string SerialNumber { get; private set; }

    public uint Revision { get; private set; }

    public bool Connected { get; private set; }

    public int Id => ((int)Type);

    internal object SyncObject { get; private set; }

    internal abstract Pipe[] SupportedPipes { get; }

    internal abstract int StandardReportSize { get; }

    internal abstract int SpecialReportSize { get; }

    internal IntPtr IOWHandle { get; set; }

    internal List<Peripheral> InterfaceList { get; private set; }


    internal IOWarriorBase(IntPtr handle)
    {
      IOWHandle = handle;
      SyncObject = new object();
      Connected = true;

      IowkitLibrary.IowKitSetTimeout(IOWHandle, 400);

      StringBuilder serialNumberBuilder = new StringBuilder();
      IowkitLibrary.IowKitGetSerialNumber(IOWHandle, serialNumberBuilder);
      SerialNumber = serialNumberBuilder.ToString();

      Revision = IowkitLibrary.IowKitGetRevision(IOWHandle);

      InterfaceList = new List<Peripheral>();
    }


    internal abstract bool IsValidDigitalPin(int pin);

    private void CheckPipe(Pipe pipe)
    {
      if (pipe == null)
      {
        throw new ArgumentNullException("Pipe cannot be null.");
      }

      if (!SupportedPipes.Contains(pipe))
      {
        throw new ArgumentException(Name + " does not support pipe mode " + pipe.Name + ".");
      }
    }

    internal void CancelEvents()
    {
      lock (SyncObject)
      {
        foreach (var entry in InterfaceList.Where(e => e is GPIOImplementation))
        {
          (entry as GPIOImplementation).Shutdown();
        }

        Connected = false;
      }
    }

    internal void Disconnect()
    {
      foreach (var entry in InterfaceList.Where(e => e is GPIOImplementation))
      {
        (entry as GPIOImplementation).WaitForShutdown();
      }
    }

    internal void CheckClosed()
    {
      if (!Connected)
        throw new InvalidOperationException(Name + " (ID: " + string.Format("0x{0:X8}", Id) + " SN: " + SerialNumber + ") is already closed.");
    }

    private int ReportSize(Pipe pipe)
    {
      if (pipe == Pipe.IO_PINS)
        return StandardReportSize;

      return SpecialReportSize;
    }

    public T[] GetPeripherals<T>() where T : Peripheral
    {
      return InterfaceList.Where(entry => entry is T).Cast<T>().ToArray();
    }

    public T GetPeripheral<T>() where T : Peripheral
    {
      return InterfaceList.Where(entry => entry is T).Cast<T>().FirstOrDefault();
    }

    /// <summary>
    /// Returns a new report at given pipe size. All bytes set to be zero.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    internal byte[] NewReport(Pipe pipe)
    {
      CheckPipe(pipe);

      return Enumerable.Repeat((byte)0x0, ReportSize(pipe)).ToArray();
    }

    /// <summary>
    /// Returns byte array report read from IOWarrior device using given pipe.
    /// This method blocks until new data arrived from IOWarrior or timeout
    /// of 400ms expired, which results in an IOException.
    /// Use this method only if you know what you are doing.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    internal byte[] ReadReport(Pipe pipe)
    {
      lock (SyncObject)
      {
        CheckClosed();
        CheckPipe(pipe);

        var report = NewReport(pipe);

        if (report.Length != IowkitLibrary.IowKitRead(IOWHandle, pipe.Id, report, (uint)report.Length))
        {
          throw new IOException("Error while reading data.");
        }

        return report;
      }
    }

    /// <summary>
    /// Try to read byte array report from IOWarrior device using given pipe.
    /// This method blocks until new data arrived from IOWarrior or timeout of 400ms expired.
    /// Returns true if report contains new data, otherwise false.
    /// Use this method only if you know what you are doing.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    internal bool TryReadReport(Pipe pipe, out byte[] report)
    {
      lock (SyncObject)
      {
        CheckClosed();
        CheckPipe(pipe);

        report = NewReport(pipe);

        return report.Length == IowkitLibrary.IowKitRead(IOWHandle, pipe.Id, report, (uint)report.Length);
      }
    }

    /// <summary>
    /// Read byte array report from IOWarrior device using given pipe.
    /// This method returns immediately - true if report contains new data, otherwise false.
    /// Use this method only if you know what you are doing.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    internal bool TryReadReportNonBlocking(Pipe pipe, out byte[] report)
    {
      lock (SyncObject)
      {
        CheckClosed();
        CheckPipe(pipe);

        report = NewReport(pipe);

        return report.Length == IowkitLibrary.IowKitReadNonBlocking(IOWHandle, pipe.Id, report, (uint)report.Length);
      }
    }

    /// <summary>
    /// Write byte array report generated by NewReport()
    /// to IOWarrior device, using given pipe.
    /// Use this method only if you know what you are doing.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    internal void WriteReport(byte[] report, Pipe pipe)
    {
      if (!TryWriteReport(report, pipe))
      {
        throw new IOException("Error while writing data.");
      }
    }

    /// <summary>
    /// Try to write byte array report generated by NewReport()
    /// to IOWarrior device, using given pipe.
    /// Use this method only if you know what you are doing.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    internal bool TryWriteReport(byte[] report, Pipe pipe)
    {
      lock (SyncObject)
      {
        CheckClosed();
        CheckPipe(pipe);

        if (ReportSize(pipe) != report.Length) throw new ArgumentException("Wrong report size!");

        return report.Length == IowkitLibrary.IowKitWrite(IOWHandle, pipe.Id, report, (uint)report.Length);
      }
    }
  }
}
