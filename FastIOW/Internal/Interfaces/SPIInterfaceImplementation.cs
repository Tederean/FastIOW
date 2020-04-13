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
using System.Linq;

namespace Tederean.FastIOW.Internal
{

  public class SPIInterfaceImplementation : SPIInterface
  {

    public bool Enabled { get; private set; }

    private IOWarriorBase IOWarrior { get; set; }

    private int SPIPacketLength { get; set; }


    internal SPIInterfaceImplementation(IOWarriorBase IOWarrior, int SPIPacketLength)
    {
      this.IOWarrior = IOWarrior;
      this.SPIPacketLength = SPIPacketLength;

      // Set to a secure state.
      Enabled = true;
      Disable();
    }


    public void Enable()
    {
      if (IOWarrior is IOWarrior56 && ((IOWarrior as IOWarrior56).PWM as PWMInterfaceImplementation).SelectedChannels == PWMConfig.PWM_1To2)
      {
        throw new InvalidOperationException("SPI cannot be used while PWM_2 is enabled.");
      }

      var report = IOWarrior.NewReport(Pipe.SPECIAL_MODE);

      report[0] = ReportId.SPI_SETUP;
      report[1] = 0x01; // Enable

      if (IOWarrior.Type == IOWarriorType.IOWarrior56)
      {
        // SPI Mode 0, msb first
        report[2] = 0x00;

        // Clock -> 8 MHz
        report[3] = 0x02;
      }

      if (IOWarrior.Type == IOWarriorType.IOWarrior24)
      {
        // SPI Mode 0, msb first, 1MBit/sec
        report[2] = 0x05;
      }

      IOWarrior.WriteReport(report, Pipe.SPECIAL_MODE);
      Enabled = true;
    }

    public void Disable()
    {
      if (!Enabled) return;

      var report = IOWarrior.NewReport(Pipe.SPECIAL_MODE);

      report[0] = ReportId.SPI_SETUP;
      report[1] = 0x00; // Disable

      IOWarrior.WriteReport(report, Pipe.SPECIAL_MODE);
      Enabled = false;
    }

    public byte[] TransferBytes(params byte[] data)
    {
      if (!Enabled) throw new InvalidOperationException("SPI interface is not enabled.");
      if (data.Length < 1 || data.Length > SPIPacketLength) throw new ArgumentException("Data length must be between 1 and " + SPIPacketLength + ".");

      var report = IOWarrior.NewReport(Pipe.SPECIAL_MODE);

      report[0] = ReportId.SPI_TRANSFER;

      if (IOWarrior.Type == IOWarriorType.IOWarrior56)
      {
        // Count
        report[1] = (byte)data.Length;

        // Flags -> no DRDY, SS stays not acive
        report[2] = 0x00;

        // Write data bytes
        for (int index = 0; index < data.Length; index++)
        {
          report[3 + index] = data[index];
        }
      }

      if (IOWarrior.Type == IOWarriorType.IOWarrior24)
      {
        // Count & Flags -> no DRDY, SS stays not acive
        report[1] = (byte)data.Length;

        // Write data bytes
        for (int index = 0; index < data.Length; index++)
        {
          report[2 + index] = data[index];
        }
      }

      IOWarrior.WriteReport(report, Pipe.SPECIAL_MODE);

      var result = IOWarrior.ReadReport(Pipe.SPECIAL_MODE);

      if (result[0] != ReportId.SPI_TRANSFER)
      {
        if (Debugger.IsAttached) Debugger.Break();

        throw new InvalidOperationException("Recieved wrong packet!");
      }

      return result.Skip(2).Take(result[1]).ToArray();
    }
  }
}
