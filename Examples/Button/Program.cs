using System;
using System.Collections.Generic;
using System.Threading;
using Tederean.FastIOW;

namespace Button
{

  class Program
  {

    private static readonly Dictionary<IOWarriorType, int> LedDefinitions = new Dictionary<IOWarriorType, int>()
    {
      { IOWarriorType.IOWarrior40, Pinout.IOWarrior40.BUILDIN_LED0 },
      { IOWarriorType.IOWarrior24, Pinout.IOWarrior24.BUILDIN_LED },
      { IOWarriorType.IOWarrior56, Pinout.IOWarrior56.BUILDIN_LED },
      { IOWarriorType.IOWarrior28, Pinout.IOWarrior28.BUILDIN_LED },
      { IOWarriorType.IOWarrior28L, Pinout.IOWarrior28L.P0_1 },
      { IOWarriorType.Unknown, -1 },
    };

    private static readonly Dictionary<IOWarriorType, int> ButtonDefinitions = new Dictionary<IOWarriorType, int>()
    {
      { IOWarriorType.IOWarrior40, Pinout.IOWarrior40.BUILDIN_BUTTON },
      { IOWarriorType.IOWarrior24, Pinout.IOWarrior24.P0_0 },
      { IOWarriorType.IOWarrior56, Pinout.IOWarrior56.BUILDIN_BUTTON },
      { IOWarriorType.IOWarrior28, Pinout.IOWarrior28.BUILDIN_BUTTON },
      { IOWarriorType.IOWarrior28L, Pinout.IOWarrior28L.P0_0 },
      { IOWarriorType.Unknown, -1 },
    };


    static void Main(string[] args)
    {
      FastIOW.OpenConnection();

      if (!FastIOW.Connected)
      {
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
          bool button = iow.DigitalRead(ButtonDefinitions[iow.Type]);
          iow.DigitalWrite(LedDefinitions[iow.Type], button);
        }

        Thread.Sleep(10);
      }

      FastIOW.CloseConnection();
    }

    private static void PrintInfos()
    {
      Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|", "Name", "Unique Identifier", "Serial Number"));

      foreach (var iow in FastIOW.GetIOWarriors())
      {
        Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|", iow.Type.Name, string.Format("0x{0:X8}", iow.Type.Id), iow.SerialNumber));
      }

      Console.WriteLine("\nPress any key to exit.");
    }
  }
}
