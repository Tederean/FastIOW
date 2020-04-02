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
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Tederean.FastIOW.Internal
{

  public class I2CInterfaceImplementation : I2CInterface
  {

    public bool Enabled { get; private set; }

    private IOWarriorBase IOWarrior { get; set; }

    private Pipe I2CPipe { get; set; }

    private int I2CPacketLength { get; set; }


    internal I2CInterfaceImplementation(IOWarriorBase IOWarrior, Pipe I2CPipe, int I2CPacketLength)
    {
      this.IOWarrior = IOWarrior;
      this.I2CPipe = I2CPipe;
      this.I2CPacketLength = I2CPacketLength;
    }


    public void Enable()
    {
      var report = IOWarrior.NewReport(I2CPipe);

      // I2C Enable
      report[0] = 0x01;
      report[1] = 0x01;

      IOWarrior.WriteReport(report, I2CPipe);
      Enabled = true;
    }

    public void Disable()
    {
      if (!Enabled) return;

      var report = IOWarrior.NewReport(I2CPipe);

      // I2C Disable
      report[0] = 0x01;

      IOWarrior.WriteReport(report, I2CPipe);
      Enabled = false;
    }

    public void WriteBytes(byte address, params byte[] data)
    {
      if (!Enabled) throw new InvalidOperationException("I2C is not enabled.");
      if (address.GetBit(7)) throw new ArgumentException("Illegal I2C Address: " + string.Format("0x{0:X2}", address));
      if (data.Length > (I2CPacketLength - 1)) throw new ArgumentException("Data length must be between 0 and " + (I2CPacketLength - 1) + ".");

      var report = IOWarrior.NewReport(I2CPipe);

      report[0] = 0x02; // I2C Write

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

      IOWarrior.WriteReport(report, I2CPipe);

      var result = IOWarrior.ReadReport(I2CPipe);

      if (result[0] != 0x02)
      {
        if (Debugger.IsAttached) Debugger.Break();

        throw new InvalidOperationException("Recieved wrong packet!");
      }

      if (result[1].GetBit(7))
      {
        throw new IOException("Error while writing data.");
      }

      if (IOWarrior.Type == IOWarriorType.IOWarrior56 || IOWarrior.Type == IOWarriorType.IOWarrior28)
      {
        if (result[1].GetBit(6))
        {
          throw new IOException("Error while writing data, arbitration lost.");
        }
      }
    }

    public byte[] ReadBytes(byte address, int length)
    {
      if (!Enabled) throw new InvalidOperationException("I2C is not enabled.");
      if (address.GetBit(7)) throw new ArgumentException("Illegal I2C Address: " + string.Format("0x{0:X2}", address));
      if (length > I2CPacketLength || length < 0) throw new ArgumentException("Data length must be between 0 and " + I2CPacketLength + ".");

      var report = IOWarrior.NewReport(I2CPipe);

      report[0] = 0x03; // I2C Read

      report[1] = (byte)length; // Amount of bytes to read

      // Write address byte
      report[2] = (byte)(address << 1);
      report[2].SetBit(0, true); // true -> read

      if (IOWarrior.Type == IOWarriorType.IOWarrior28)
      {
        report[3] = (byte)length; // Amount of bytes to read
      }

      IOWarrior.WriteReport(report, I2CPipe);

      var result = IOWarrior.ReadReport(I2CPipe);

      if (result[0] != 0x03)
      {
        if (Debugger.IsAttached) Debugger.Break();

        throw new InvalidOperationException("Recieved wrong packet!");
      }

      if (result[1].GetBit(7))
      {
        throw new IOException("Error while reading data.");
      }

      if (IOWarrior.Type == IOWarriorType.IOWarrior56 && result[1].GetBit(6))
      {
        throw new IOException("Error while reading data, arbitration lost.");
      }

      return result.Skip(2).Take(length).ToArray();
    }

    public ushort Read2Bytes(byte address)
    {
      var result = ReadBytes(address, 2);

      return (ushort)((result[0] << 8) + result[1]);
    }
  }
}
