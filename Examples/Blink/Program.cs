using System;
using System.Collections.Generic;
using System.Threading;
using Tederean.FastIOW;

namespace Blink
{

  class Program
  {

    private static readonly Dictionary<IOWarriorType, int> LedDefinitions = new Dictionary<IOWarriorType, int>()
    {
      { IOWarriorType.IOWarrior40, IOWarrior40.P3_0 },
      { IOWarriorType.IOWarrior24, IOWarrior24.P0_3 },
      { IOWarriorType.IOWarrior56, IOWarrior56.P6_7 },
      { IOWarriorType.IOWarrior28, IOWarrior28.P2_0 },
      { IOWarriorType.IOWarrior28L, IOWarrior28L.P0_0 }
    };


    // This example will blink up LEDs with a fix frequency of 1 Hz. See above for pin definitions.
    static void Main(string[] args)
    {
      FastIOW.OpenConnection();

      if (FastIOW.GetPeripherals<GPIO>().Length == 0)
      {
        FastIOW.CloseConnection();
        Console.WriteLine("No GPIO capable IOWarrior detected!");
        Console.ReadKey();
        return;
      }

      PrintInfos();

      Console.WriteLine("\nPress any key to exit.");

      // Set delay to zero to get maximum frequency
      // (e.g. 250 Hz on Win10 x64 with AMD Ryzen 5 2400G and one IOWarrior 28)
      int delay = 500; // 0;

      while (!Console.KeyAvailable)
      {
        foreach (GPIO gpio in FastIOW.GetPeripherals<GPIO>())
        {
          gpio.DigitalWrite(LedDefinitions[gpio.IOWarrior.Type], PinState.LOW);
        }

        Thread.Sleep(delay);

        foreach (GPIO gpio in FastIOW.GetPeripherals<GPIO>())
        {
          gpio.DigitalWrite(LedDefinitions[gpio.IOWarrior.Type], PinState.HIGH);
        }

        Thread.Sleep(delay);
      }

      FastIOW.CloseConnection();
    }

    private static void PrintInfos()
    {
      Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|", "Name", "Unique Identifier", "Serial Number"));

      foreach (var iow in FastIOW.GetIOWarriors())
      {
        Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|", iow.Name, string.Format("0x{0:X8}", iow.Id), iow.SerialNumber));
      }
    }
  }
}
