using System;
using System.Collections.Generic;
using System.Linq;
using Tederean.FastIOW;

namespace Timer_PulseIn
{

  class Program
  {

    private static readonly Dictionary<IOWarriorType, int> TimerDefinitions = new Dictionary<IOWarriorType, int>()
    {
      { IOWarriorType.IOWarrior24, IOWarrior24.Timer_1 }
    };

    // This programm requires an IOWarrior24. Connect a push button to pin P0.0 and press for short period of time.
    // Pulse duration will show up on console. For more accurate results is neccessary to debounce the push button.
    static void Main(string[] args)
    {
      FastIOW.OpenConnection();

      Timer timer = FastIOW.GetPeripherals<Timer>().FirstOrDefault();

      if (timer == null)
      {
        FastIOW.CloseConnection();
        Console.WriteLine("No timer capable IOWarrior detected! Press any key to exit.");
        Console.ReadKey();
        return;
      }

      Console.WriteLine("Found a timer capable IOWarrior!");
      Console.WriteLine("Press a push button connected to Timer_1 or press any key on keyboard to exit.");

      while (!Console.KeyAvailable)
      {
        // Read duration of button press using Timer_1 on pin P0.0, set timeout to 2 seconds.
        int time_us = timer.PulseIn(TimerDefinitions[timer.IOWarrior.Type], false, TimeSpan.FromSeconds(2));

        if (time_us != -1)
        {
          Console.WriteLine("Pulsewidth = " + time_us + " μs");
        }
      }

      FastIOW.CloseConnection();
    }
  }
}