using System;
using System.Collections.Generic;
using System.Threading;
using Tederean.FastIOW;

namespace I2C_Scan
{

  class Program
  {

    private static readonly Dictionary<IOWarriorType, int> LedDefinitions = new Dictionary<IOWarriorType, int>()
    {
      { IOWarriorType.IOWarrior40, IOWarrior40.BUILDIN_LED0 },
      { IOWarriorType.IOWarrior24, IOWarrior24.BUILDIN_LED },
      { IOWarriorType.IOWarrior56, IOWarrior56.BUILDIN_LED },
      { IOWarriorType.IOWarrior28, IOWarrior28.BUILDIN_LED },
      { IOWarriorType.IOWarrior28L, IOWarrior28L.P0_0 }
    };


    // This example will blink up LEDs with a fix frequency of 1 Hz. See above for pin definitions.
    static void Main(string[] args)
    {
      FastIOW.OpenConnection();

      if (!FastIOW.Connected)
      {
        FastIOW.CloseConnection();
        Console.WriteLine("No IOWarrior detected!");
        Console.ReadKey();
        return;
      }

      PrintInfos();

      IOWarrior[] iows = FastIOW.GetIOWarriors();

      while (!Console.KeyAvailable)
      {
        foreach (var iow in iows)
        {
          iow.DigitalWrite(LedDefinitions[iow.Type], iow.LOW);
        }

        Thread.Sleep(500);

        foreach (var iow in iows)
        {
          iow.DigitalWrite(LedDefinitions[iow.Type], iow.HIGH);
        }

        Thread.Sleep(500);
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

      Console.WriteLine("\nPress any key to exit.");
    }
  }
}
