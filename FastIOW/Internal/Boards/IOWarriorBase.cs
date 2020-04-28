using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tederean.FastIOW.Internal
{

  public abstract class IOWarriorBase : IOWarrior
  {

    internal object SyncObject { get; private set; }

    public abstract string Name { get; }

    public abstract IOWarriorType Type { get; }

    public int Id => ((int)Type);

    protected abstract Pipe[] SupportedPipes { get; }

    protected abstract int StandardReportSize { get; }

    protected abstract int SpecialReportSize { get; }

    public string SerialNumber { get; private set; }

    public uint Revision { get; private set; }

    public bool Connected { get; private set; }

    private IntPtr IOWHandle { get; set; }

    private byte[] IOPinsWriteReport { get; set; }

    private byte[] IOPinsReadReport { get; set; }

    private Thread IOThread { get; set; }

    public bool LOW => false;

    public bool HIGH => true;


    public event EventHandler<PinStateChangeEventArgs> PinStateChange;


    internal IOWarriorBase(IntPtr handle)
    {
      IOWHandle = handle;
      SyncObject = new object();
      Connected = true;

      NativeLib.IowKitSetTimeout(IOWHandle, 400);

      StringBuilder serialNumberBuilder = new StringBuilder();
      NativeLib.IowKitGetSerialNumber(IOWHandle, serialNumberBuilder);
      SerialNumber = serialNumberBuilder.ToString();

      Revision = NativeLib.IowKitGetRevision(IOWHandle);

      var report = NewReport(Pipe.SPECIAL_MODE);
      report[0] = ReportId.GPIO_SPECIAL_READ;

      // Get state using special mode
      WriteReport(report, Pipe.SPECIAL_MODE);
      var result = ReadReport(Pipe.SPECIAL_MODE).Take(StandardReportSize).ToArray();
      result[0] = ReportId.GPIO_READ_WRITE;

      IOPinsWriteReport = result.ToArray();
      IOPinsReadReport = result.ToArray();

      IOThread = new Thread(ProcessIO) { IsBackground = true };
      IOThread.Start();
    }


    protected abstract bool IsValidDigitalPin(int pin);

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
        PinStateChange?.GetInvocationList().ToList().ForEach(d => PinStateChange -= (EventHandler<PinStateChangeEventArgs>)d);

        Connected = false;
      }
    }

    internal void Disconnect()
    {
      IOThread.Join();
      IOThread = null;
    }

    private void ProcessIO()
    {
      while (Connected)
      {
        var result = NewReport(Pipe.IO_PINS);

        if (result.Length != NativeLib.IowKitRead(IOWHandle, Pipe.IO_PINS.Id, result, (uint)result.Length))
          continue;

        if (!Connected)
          return;

        for (int index = 1; index < result.Length; index++)
        {
          foreach (var bit in Enumerable.Range(0, 7))
          {
            bool newState = result[index].GetBit(bit);
            bool oldState = IOPinsReadReport[index].GetBit(bit);
            int pin = index * 8 + bit;

            if (newState != oldState)
            {
              lock (SyncObject)
              {
                IOPinsReadReport[index].SetBit(bit, newState);

                if (PinStateChange != null && IsValidDigitalPin(pin))
                {
                  try
                  {
                    PinStateChange.Invoke(this, new PinStateChangeEventArgs(this, pin, newState, oldState));
                  }
                  catch (Exception)
                  {
                    if (Debugger.IsAttached) Debugger.Break();
                  }
                }
              }
            }
          }
        }
      }
    }

    private void CheckClosed()
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

    public bool DigitalRead(int pin)
    {
      lock (SyncObject)
      {
        if (!IsValidDigitalPin(pin))
        {
          throw new ArgumentException("Pin not existing on " + Name + ".");
        }

        CheckClosed();

        return IOPinsReadReport[pin / 8].GetBit(pin % 8);
      }
    }

    public void DigitalWrite(int pin, bool state)
    {
      lock (SyncObject)
      {
        if (!IsValidDigitalPin(pin))
        {
          throw new ArgumentException("Pin not existing on " + Name + ".");
        }

        CheckClosed();

        if (IOPinsWriteReport[pin / 8].GetBit(pin % 8) != state)
        {
          IOPinsWriteReport[pin / 8].SetBit(pin % 8, state);

          WriteReport(IOPinsWriteReport, Pipe.IO_PINS);
        }
      }
    }

    public byte[] NewReport(Pipe pipe)
    {
      CheckPipe(pipe);

      return Enumerable.Repeat((byte)0x0, ReportSize(pipe)).ToArray();
    }

    public byte[] ReadReport(Pipe pipe)
    {
      lock (SyncObject)
      {
        CheckClosed();
        CheckPipe(pipe);

        var report = NewReport(pipe);

        if (report.Length != NativeLib.IowKitRead(IOWHandle, pipe.Id, report, (uint)report.Length))
        {
          throw new IOException("Error while reading data.");
        }

        return report;
      }
    }

    public bool TryReadReport(Pipe pipe, out byte[] report)
    {
      lock (SyncObject)
      {
        CheckClosed();
        CheckPipe(pipe);

        report = NewReport(pipe);

        return report.Length == NativeLib.IowKitRead(IOWHandle, pipe.Id, report, (uint)report.Length);
      }
    }

    public bool TryReadReportNonBlocking(Pipe pipe, out byte[] report)
    {
      lock (SyncObject)
      {
        CheckClosed();
        CheckPipe(pipe);

        report = NewReport(pipe);

        return report.Length == NativeLib.IowKitReadNonBlocking(IOWHandle, pipe.Id, report, (uint)report.Length);
      }
    }

    public void WriteReport(byte[] report, Pipe pipe)
    {
      lock (SyncObject)
      {
        CheckClosed();
        CheckPipe(pipe);

        if (ReportSize(pipe) != report.Length) throw new ArgumentException("Wrong report size!");

        if (report.Length != NativeLib.IowKitWrite(IOWHandle, pipe.Id, report, (uint)report.Length))
        {
          throw new IOException("Error while writing data.");
        }
      }
    }
  }
}
