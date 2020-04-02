using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tederean.FastIOW.Internal
{

  public abstract class IOWarriorBase : IOWarrior
  {

    public abstract string Name { get; }

    public abstract IOWarriorType Type { get; }

    public int Id => ((int)Type);

    protected abstract Pipe[] SupportedPipes { get; }

    protected abstract int StandardReportSize { get; }

    protected abstract int SpecialReportSize { get; }

    public string SerialNumber { get; private set; }

    public bool Connected { get; private set; }

    private int IOWHandle { get; set; }

    private byte[] IOPinsWriteReport { get; set; }

    private byte[] IOPinsReadReport { get; set; }

    private Thread IOThread { get; set; }

    public bool LOW => false;

    public bool HIGH => true;


    public event EventHandler<PinStateChangeEventArgs> PinStateChange;


    internal IOWarriorBase(int handle)
    {
      IOWHandle = handle;
      Connected = true;

      StringBuilder serialNumberBuilder = new StringBuilder();
      NativeLib.IowKitGetSerialNumber(IOWHandle, serialNumberBuilder);
      SerialNumber = serialNumberBuilder.ToString();

      var report = NewReport(Pipe.SPECIAL_MODE);
      report[0] = 0xFF;

      // Get state using special mode
      WriteReport(report, Pipe.SPECIAL_MODE);
      var result = ReadReport(Pipe.SPECIAL_MODE).Take(StandardReportSize).ToArray();
      result[0] = 0x00;

      IOPinsWriteReport = result.ToArray();
      IOPinsReadReport = result.ToArray();

      IOThread = new Thread(ProcessIO);
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

    internal void Disconnect()
    {
      PinStateChange?.GetInvocationList().ToList().ForEach(d => PinStateChange -= (EventHandler<PinStateChangeEventArgs>)d);

      Connected = false;

      IOThread.Abort();
      IOThread = null;
    }

    private void ProcessIO()
    {
      while (Connected)
      {
        try
        {
          var result = ReadReport(Pipe.IO_PINS);

          for (int index = 1; index < result.Length; index++)
          {
            foreach (var bit in Enumerable.Range(0, 7))
            {
              bool newState = result[index].GetBit(bit);
              bool oldState = IOPinsReadReport[index].GetBit(bit);
              int pin = index * 8 + bit;

              if (newState != oldState)
              {
                IOPinsReadReport[index].SetBit(bit, newState);

                if (PinStateChange != null && IsValidDigitalPin(pin))
                {
                  PinStateChange.Invoke(this, new PinStateChangeEventArgs(this, pin, newState, oldState));
                }
              }
            }
          }
        }
        catch (Exception) { }
      }
    }

    private void CheckClosed()
    {
      if (!Connected)
        throw new InvalidOperationException(Name + " (ID: " + string.Format("0x{0:X8}", Id) + " SN: " + SerialNumber + ") is already closed.");
    }

    public bool DigitalRead(int pin)
    {
      if (!IsValidDigitalPin(pin))
      {
        throw new ArgumentNullException("Pin not existing on " + Name + ".");
      }

      CheckClosed();

      return IOPinsReadReport[pin / 8].GetBit(pin % 8);
    }

    public void DigitalWrite(int pin, bool state)
    {
      if (!IsValidDigitalPin(pin))
      {
        throw new ArgumentNullException("Pin not existing on " + Name + ".");
      }

      CheckClosed();

      if (IOPinsWriteReport[pin / 8].GetBit(pin % 8) != state)
      {
        IOPinsWriteReport[pin / 8].SetBit(pin % 8, state);
        IOPinsWriteReport[0] = 0x00;

        WriteReport(IOPinsWriteReport, Pipe.IO_PINS);
      }
    }

    public byte[] NewReport(Pipe pipe)
    {
      CheckPipe(pipe);

      if (pipe == Pipe.IO_PINS)
      {
        return Enumerable.Repeat((byte)0x0, StandardReportSize).ToArray();
      }

      return Enumerable.Repeat((byte)0x0, SpecialReportSize).ToArray();
    }

    public byte[] ReadReport(Pipe pipe)
    {
      CheckClosed();
      CheckPipe(pipe);

      var report = NewReport(pipe);

      if (report.Length != NativeLib.IowKitRead(IOWHandle, pipe.Id, report, report.Length))
      {
        throw new IOException("Error while reading data.");
      }

      return report;
    }

    public bool ReadReportNonBlocking(Pipe pipe, out byte[] report)
    {
      CheckClosed();
      CheckPipe(pipe);

      report = NewReport(pipe);

      return report.Length == NativeLib.IowKitReadNonBlocking(IOWHandle, pipe.Id, report, report.Length);
    }

    public void WriteReport(byte[] report, Pipe pipe)
    {
      CheckClosed();
      CheckPipe(pipe);

      if (report.Length != NativeLib.IowKitWrite(IOWHandle, pipe.Id, report, report.Length))
      {
        throw new IOException("Error while writing data.");
      }
    }
  }
}
