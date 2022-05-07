using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Tederean.FastIOW.Internal
{

  public class I2CImplementation : I2C
  {

    public bool Enabled { get; private set; }

    private IOWarriorBase IOWarriorBase { get; set; }

    public IOWarrior IOWarrior => IOWarriorBase;

    private Pipe I2CPipe { get; set; }

    private int I2CPacketLength { get; set; }


    internal I2CImplementation(IOWarriorBase IOWarriorBase, Pipe I2CPipe, int I2CPacketLength)
    {
      this.IOWarriorBase = IOWarriorBase;
      this.I2CPipe = I2CPipe;
      this.I2CPacketLength = I2CPacketLength;

      // Set to a secure state.
      Disable();
    }


    public void Enable()
    {
      lock (IOWarriorBase.SyncObject)
      {
        var report = IOWarriorBase.NewReport(I2CPipe);

        report[0] = ReportId.I2C_SETUP;
        report[1] = 0x01; // Enable

        IOWarriorBase.WriteReport(report, I2CPipe);
        Enabled = true;
      }
    }

    public void Disable()
    {
      lock (IOWarriorBase.SyncObject)
      {
        var report = IOWarriorBase.NewReport(I2CPipe);

        report[0] = ReportId.I2C_SETUP;
        report[1] = 0x00; // Disable

        IOWarriorBase.WriteReport(report, I2CPipe);
        Enabled = false;
      }
    }

    public bool IsAvailable(byte address)
    {
      lock (IOWarriorBase.SyncObject)
      {
        var result = WriteBytesInternal(address);

        // Error while writing data.
        if (result[1].GetBit(7)) return false;

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior56 || IOWarriorBase.Type == IOWarriorType.IOWarrior28)
        {
          // Error while writing data, arbitration lost.
          if (result[1].GetBit(6)) return false;
        }

        return true;
      }
    }

    public void WriteBytes(byte address, params byte[] data)
    {
      lock (IOWarriorBase.SyncObject)
      {
        var result = WriteBytesInternal(address);

        if (result[1].GetBit(7))
        {
          throw new IOException("Error while writing data.");
        }

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior56 || IOWarriorBase.Type == IOWarriorType.IOWarrior28)
        {
          if (result[1].GetBit(6))
          {
            throw new IOException("Error while writing data, arbitration lost.");
          }
        }
      }
    }

    private byte[] WriteBytesInternal(byte address, params byte[] data)
    {
      if (!Enabled) throw new InvalidOperationException("I2C interface is not enabled.");
      if (address.GetBit(7)) throw new ArgumentException("Illegal I2C Address: " + string.Format("0x{0:X2}", address));
      if (data.Length > (I2CPacketLength - 1)) throw new ArgumentException("Data length must be between 0 and " + (I2CPacketLength - 1) + ".");

      var report = IOWarriorBase.NewReport(I2CPipe);

      report[0] = ReportId.I2C_WRITE;

      // Write IOW I2C settings
      report[1] = (byte)(1 + data.Length);
      report[1].SetBit(7, true); // Enable start bit
      report[1].SetBit(6, true); // Enable stop bit

      // Write address byte
      report[2] = (byte)(address << 1);
      report[2].SetBit(0, false); // false -> write

      // Write data bytes
      for (int index = 0; index < data.Length; index++)
      {
        report[3 + index] = data[index];
      }

      IOWarriorBase.WriteReport(report, I2CPipe);

      var result = IOWarriorBase.ReadReport(I2CPipe);

      if (result[0] != ReportId.I2C_WRITE)
      {
        if (Debugger.IsAttached) Debugger.Break();

        throw new InvalidOperationException("Recieved wrong packet!");
      }

      return result;
    }

    public byte[] ReadBytes(byte address, int length)
    {
      lock (IOWarriorBase.SyncObject)
      {
        if (!Enabled) throw new InvalidOperationException("I2C is not enabled.");
        if (address.GetBit(7)) throw new ArgumentException("Illegal I2C Address: " + string.Format("0x{0:X2}", address));
        if (length > I2CPacketLength || length < 0) throw new ArgumentException("Data length must be between 0 and " + I2CPacketLength + ".");

        var report = IOWarriorBase.NewReport(I2CPipe);

        report[0] = ReportId.I2C_READ;

        report[1] = (byte)length; // Amount of bytes to read

        // Write address byte
        report[2] = (byte)(address << 1);
        report[2].SetBit(0, true); // true -> read

        IOWarriorBase.WriteReport(report, I2CPipe);

        var result = IOWarriorBase.ReadReport(I2CPipe);

        if (result[0] != ReportId.I2C_READ)
        {
          if (Debugger.IsAttached) Debugger.Break();

          throw new InvalidOperationException("Recieved wrong packet!");
        }

        if (result[1].GetBit(7))
        {
          throw new IOException("Error while reading data.");
        }

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior56 && result[1].GetBit(6))
        {
          throw new IOException("Error while reading data, arbitration lost.");
        }

        return result.Skip(2).Take(length).ToArray();
      }
    }

    public byte ReadByte(byte address)
    {
      return ReadBytes(address, 1).ElementAt(0);
    }

    public ushort Read2Bytes(byte address)
    {
      var result = ReadBytes(address, 2);

      return (ushort)((result[0] << 8) + result[1]);
    }

    public uint Read3Bytes(byte address)
    {
      var result = ReadBytes(address, 3);

      return (uint)((result[0] << 16) + (result[1] << 8) + result[2]);
    }

    public uint Read4Bytes(byte address)
    {
      var result = ReadBytes(address, 4);

      return (uint)((result[0] << 24) + (result[1] << 16) + (result[2] << 8) + result[3]);
    }
  }
}
