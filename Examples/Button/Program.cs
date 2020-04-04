using System;
using System.Collections.Generic;
using Tederean.FastIOW;

namespace Button
{

  class Program
  {

    private static readonly Dictionary<IOWarriorType, int> LedDefinitions = new Dictionary<IOWarriorType, int>()
    {
      { IOWarriorType.IOWarrior40, IOWarrior40.BUILDIN_LED0 },
      { IOWarriorType.IOWarrior24, IOWarrior24.BUILDIN_LED },
      { IOWarriorType.IOWarrior56, IOWarrior56.BUILDIN_LED },
      { IOWarriorType.IOWarrior28, IOWarrior28.BUILDIN_LED },
      { IOWarriorType.IOWarrior28L, IOWarrior28L.P0_1 }
    };

    private static readonly Dictionary<IOWarriorType, int> ButtonDefinitions = new Dictionary<IOWarriorType, int>()
    {
      { IOWarriorType.IOWarrior40, IOWarrior40.BUILDIN_BUTTON },
      { IOWarriorType.IOWarrior24, IOWarrior24.P0_0 },
      { IOWarriorType.IOWarrior56, IOWarrior56.BUILDIN_BUTTON },
      { IOWarriorType.IOWarrior28, IOWarrior28.BUILDIN_BUTTON },
      { IOWarriorType.IOWarrior28L, IOWarrior28L.P0_0 }
    };

    // This example lights up buildin LED as long a pushbutton is pressed. See above for pin definitions.
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

      foreach (var iow in FastIOW.GetIOWarriors())
      {
        iow.PinStateChange += OnPinStateChange;
      }

      Console.WriteLine("\nPress any key to exit.");
      Console.ReadKey();

      FastIOW.CloseConnection();
    }

    private static void OnPinStateChange(object sender, PinStateChangeEventArgs args)
    {
      if (args.Pin == ButtonDefinitions[args.IOWarrior.Type])
      {
        args.IOWarrior.DigitalWrite(LedDefinitions[args.IOWarrior.Type], args.NewPinState);
      }
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
