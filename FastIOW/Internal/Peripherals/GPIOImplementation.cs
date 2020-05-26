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
using System.Threading;

namespace Tederean.FastIOW.Internal
{

  public class GPIOImplementation : GPIO
  {

    private IOWarriorBase IOWarriorBase { get; set; }

    public IOWarrior IOWarrior => IOWarriorBase;

    private byte[] IOPinsWriteReport { get; set; }

    private byte[] IOPinsReadReport { get; set; }

    internal Thread IOThread { get; private set; }

    public bool LOW => false;

    public bool HIGH => true;

    public event EventHandler<PinStateChangeEventArgs> PinStateChange;


    internal GPIOImplementation(IOWarriorBase IOWarriorBase)
    {
      this.IOWarriorBase = IOWarriorBase;

      var report = IOWarriorBase.NewReport(Pipe.SPECIAL_MODE);
      report[0] = ReportId.GPIO_SPECIAL_READ;

      // Get state using special mode
      IOWarriorBase.WriteReport(report, Pipe.SPECIAL_MODE);
      var result = IOWarriorBase.ReadReport(Pipe.SPECIAL_MODE).Take(IOWarriorBase.StandardReportSize).ToArray();
      result[0] = ReportId.GPIO_READ_WRITE;

      IOPinsWriteReport = result.ToArray();
      IOPinsReadReport = result.ToArray();

      IOThread = new Thread(ProcessIO) { IsBackground = true };
      IOThread.Start();
    }

    internal void Shutdown()
    {
      PinStateChange?.GetInvocationList().ToList().ForEach(d => PinStateChange -= (EventHandler<PinStateChangeEventArgs>)d);
    }

    internal void WaitForShutdown()
    {
      IOThread.Join();
      IOThread = null;
    }

    private void ProcessIO()
    {
      while (IOWarriorBase.Connected)
      {
        var result = IOWarriorBase.NewReport(Pipe.IO_PINS);

        if (result.Length != NativeLib.IowKitRead(IOWarriorBase.IOWHandle, Pipe.IO_PINS.Id, result, (uint)result.Length))
          continue;

        if (!IOWarriorBase.Connected)
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
              lock (IOWarriorBase.SyncObject)
              {
                IOPinsReadReport[index].SetBit(bit, newState);

                if (PinStateChange != null && IOWarriorBase.IsValidDigitalPin(pin))
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

    public bool DigitalRead(int pin)
    {
      lock (IOWarriorBase.SyncObject)
      {
        if (!IOWarriorBase.IsValidDigitalPin(pin))
        {
          throw new ArgumentException("Pin not existing on " + IOWarriorBase.Name + ".");
        }

        IOWarriorBase.CheckClosed();

        return IOPinsReadReport[pin / 8].GetBit(pin % 8);
      }
    }

    public void DigitalWrite(int pin, bool state)
    {
      lock (IOWarriorBase.SyncObject)
      {
        if (!IOWarriorBase.IsValidDigitalPin(pin))
        {
          throw new ArgumentException("Pin not existing on " + IOWarriorBase.Name + ".");
        }

        IOWarriorBase.CheckClosed();

        if (IOPinsWriteReport[pin / 8].GetBit(pin % 8) != state)
        {
          IOPinsWriteReport[pin / 8].SetBit(pin % 8, state);

          IOWarriorBase.WriteReport(IOPinsWriteReport, Pipe.IO_PINS);
        }
      }
    }
  }
}
