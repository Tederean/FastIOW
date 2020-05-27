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

  public class TimerImplementation : Timer
  {

    private IOWarriorBase IOWarriorBase { get; set; }

    public IOWarrior IOWarrior => IOWarriorBase;

    private int[] m_TimerPins;
    public int[] SupportedPins
    {
      get => m_TimerPins?.ToArray() ?? default;
      private set => m_TimerPins = value;
    }

    private int TimerState { get; set; }


    internal TimerImplementation(IOWarriorBase IOWarriorBase, int[] TimerPins)
    {
      this.IOWarriorBase = IOWarriorBase;
      this.SupportedPins = TimerPins;

      // Set to a secure state.
      SetTimerMode(0x00);
    }


    public int PulseIn(int pin, bool value)
    {
      return PulseIn(pin, value, TimeSpan.FromSeconds(1));
    }

    public int PulseIn(int pin, bool value, TimeSpan timeout)
    {
      return PulseIn(pin, value, timeout, TimeSpan.FromMilliseconds(10));
    }

    public int PulseIn(int pin, bool value, TimeSpan timeout, TimeSpan interval)
    {
      lock (IOWarriorBase.SyncObject)
      {
        if (!Array.Exists<int>(SupportedPins, element => element == pin)) throw new ArgumentException("Not a Timer capable pin.");

        if (IOWarriorBase.Type == IOWarriorType.IOWarrior24)
        {
          if (pin == IOWarrior24.Timer_2 && (IOWarriorBase.GetPeripheral<I2C>()?.Enabled ?? false))
          {
            throw new InvalidOperationException("Timer_2 cannot be used while I2C is enabled.");
          }
        }

        SetTimerMode(PinToChannelIndex(pin) + 1); // Enable Timer

        try
        {
          return PulseInInternal(pin, value, timeout, interval);
        }
        catch (TimeoutException)
        {
          return -1;
        }
        finally
        {
          SetTimerMode(0x00); // Disable Timer
        }
      }
    }

    private int PulseInInternal(int pin, bool value, TimeSpan timeout, TimeSpan interval)
    {
      var start = DateTime.UtcNow;
      var id = PinToReportId(pin);

      while ((DateTime.UtcNow - start) < timeout)
      {
        if (IOWarriorBase.TryReadReportNonBlocking(Pipe.SPECIAL_MODE, out byte[] report))
        {
          int span;
          if (report[0] == id && (span = ReportToTimeSpan(report, value)) > -1)
          {
            return span;
          }

          if (report[0] != ReportId.TIMER_DATA_A && report[0] != ReportId.TIMER_DATA_B)
          {
            if (Debugger.IsAttached) Debugger.Break();

            throw new InvalidOperationException("Recieved wrong packet!");
          }
        }

        Thread.Sleep((int)interval.TotalMilliseconds);
      }

      throw new TimeoutException();
    }

    private int ReportToTimeSpan(byte[] report, bool value)
    {
      var falling = BytesToInt(report[2], report[3], report[4]);
      var rising = BytesToInt(report[5], report[6], report[7]);
      var timePerTick = 4; // 4 us per tick

      if (value && rising < falling)
      {
        return (falling - rising) * timePerTick;
      }

      if (!value && rising > falling)
      {
        return (rising - falling) * timePerTick;
      }

      return -1;
    }

    private int BytesToInt(byte b0, byte b1, byte b2)
    {
      return (b2 << 16) + (b1 << 8) + b0;
    }

    private void SetTimerMode(int state)
    {
      var report = IOWarriorBase.NewReport(Pipe.SPECIAL_MODE);

      report[0] = ReportId.TIMER_SETUP;
      report[1] = (byte)state; // Channels

      IOWarriorBase.WriteReport(report, Pipe.SPECIAL_MODE);
      TimerState = state;
    }

    private int PinToChannelIndex(int pin)
    {
      return Array.IndexOf<int>(SupportedPins, pin);
    }

    private int PinToReportId(int pin)
    {
      return PinToChannelIndex(pin) == 0 ? ReportId.TIMER_DATA_A : ReportId.TIMER_DATA_B;
    }
  }
}
