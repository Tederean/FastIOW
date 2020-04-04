using System;
using System.Linq;
using Tederean.FastIOW;

namespace Timer_PulseIn
{

  class Program
  {

    // This programm requires an IOWarrior24. Connect a push button to pin P0.0 and press for short period of time.
    // Pulse duration will show up on console. For more accurate results is neccessary to debounce the push button.
    static void Main(string[] args)
    {
      FastIOW.OpenConnection();

      IOWarrior24 iow = (IOWarrior24)FastIOW.GetIOWarriors().Where(entry => entry is IOWarrior24).FirstOrDefault();

      if (iow == null)
      {
        FastIOW.CloseConnection();
        Console.WriteLine("No IOWarrior24 detected! Press any key to exit.");
        Console.ReadKey();
        return;
      }

      Console.WriteLine("Found an IOWarrior24!");
      Console.WriteLine("Press a push button connected to Timer_1 or press any key on keyboard to exit.");

      while (!Console.KeyAvailable)
      {
        // Read duration of button press using Timer_1 on pin P0.0, set timeout to 2 seconds.
        int time_us = iow.Timer.PulseIn(IOWarrior24.Timer_1, false, TimeSpan.FromSeconds(2));

        if (time_us != -1)
        {
          Console.WriteLine("Pulsewidth = " + time_us + " μs");
        }
      }

      FastIOW.CloseConnection();
    }
  }
}