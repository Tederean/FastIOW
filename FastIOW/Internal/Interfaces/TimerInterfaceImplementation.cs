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
using System.Threading;

namespace Tederean.FastIOW.Internal
{

  public class TimerInterfaceImplementation : TimerInterface
  {

    private IOWarriorBase IOWarrior { get; set; }

    private int[] TimerPins { get; set; }


    internal TimerInterfaceImplementation(IOWarriorBase IOWarrior, int[] TimerPins)
    {
      this.IOWarrior = IOWarrior;
      this.TimerPins = TimerPins;
    }


    public int PulseIn(int pin, bool value)
    {
      return PulseIn(pin, value, TimeSpan.FromSeconds(1));
    }

    public int PulseIn(int pin, bool value, TimeSpan timeout)
    {
      return PulseIn(pin, value, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(10));
    }

    public int PulseIn(int pin, bool value, TimeSpan timeout, TimeSpan interval)
    {
      if (!Array.Exists<int>(TimerPins, element => element == pin)) throw new ArgumentException("Not a Timer capable pin.");

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

    private int PulseInInternal(int pin, bool value, TimeSpan timeout, TimeSpan interval)
    {
      var start = DateTime.UtcNow;
      var id = PinToReportId(pin);

      while ((DateTime.UtcNow - start) < timeout)
      {
        byte[] report;
        if (IOWarrior.ReadReportNonBlocking(Pipe.SPECIAL_MODE, out report))
        {
          int span;
          if (report[0] == id && (span = ReportToTimeSpan(report, value)) > -1)
          {
            return span;
          }

          if (report[0] != 0x29 && report[0] != 0x2A)
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

      if (value && rising < falling)
      {
        return falling - rising;
      }

      if (!value && rising > falling)
      {
        return rising - falling;
      }

      return -1;
    }

    private int BytesToInt(byte b0, byte b1, byte b2)
    {
      return (b2 << 16) + (b1 << 8) + b0;
    }

    private void SetTimerMode(int state)
    {
      var report = IOWarrior.NewReport(Pipe.SPECIAL_MODE);

      report[0] = 0x28; // Timer
      report[1] = (byte)state; // Channels

      IOWarrior.WriteReport(report, Pipe.SPECIAL_MODE);
    }

    private int PinToChannelIndex(int pin)
    {
      return Array.IndexOf<int>(TimerPins, pin);
    }

    private int PinToReportId(int pin)
    {
      return PinToChannelIndex(pin) == 0 ? 0x29 : 0x2A;
    }
  }
}
