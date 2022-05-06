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

    private int[] m_GpioPins;
    public int[] SupportedPins
    {
      get => m_GpioPins?.ToArray() ?? default;
      private set => m_GpioPins = value;
    }

    public event EventHandler<PinStateChangeEventArgs> PinStateChange;


    internal GPIOImplementation(IOWarriorBase IOWarriorBase)
    {
      this.IOWarriorBase = IOWarriorBase;
      this.SupportedPins = Enumerable.Range(0, 255).Where(e => IOWarriorBase.IsValidDigitalPin(e)).ToArray();

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

    private PinState ToPinState(bool value)
    {
      return value ? PinState.HIGH : PinState.LOW;
    }

    private bool ToBool(PinState value)
    {
      switch (value)
      {
        case (PinState.LOW):
          return false;
        case (PinState.HIGH):
          return true;
        default:
          throw new ArgumentException("Invalid PinState.");
      }
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
              IOPinsReadReport[index].SetBit(bit, newState);

              if (PinStateChange != null && IOWarriorBase.IsValidDigitalPin(pin))
              {
                try
                {
                  PinStateChange.Invoke(this, new PinStateChangeEventArgs(this, pin, ToPinState(newState), ToPinState(oldState)));
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

    public PinState DigitalRead(int pin)
    {
      lock (IOWarriorBase.SyncObject)
      {
        if (!IOWarriorBase.IsValidDigitalPin(pin))
        {
          throw new ArgumentException("Pin not existing on " + IOWarriorBase.Name + ".");
        }

        IOWarriorBase.CheckClosed();

        return ToPinState(IOPinsReadReport[pin / 8].GetBit(pin % 8));
      }
    }

    public void DigitalWrite(int pin, PinState state)
    {
      lock (IOWarriorBase.SyncObject)
      {
        if (!Enum.IsDefined(typeof(PinState), state)) throw new ArgumentException("Invalid PinState.");

        if (!IOWarriorBase.IsValidDigitalPin(pin))
        {
          throw new ArgumentException("Pin not existing on " + IOWarriorBase.Name + ".");
        }

        IOWarriorBase.CheckClosed();

        if (IOPinsWriteReport[pin / 8].GetBit(pin % 8) != ToBool(state))
        {
          IOPinsWriteReport[pin / 8].SetBit(pin % 8, ToBool(state));

          IOWarriorBase.WriteReport(IOPinsWriteReport, Pipe.IO_PINS);
        }
      }
    }
  }
}
