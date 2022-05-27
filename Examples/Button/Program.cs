using System;
using System.Collections.Generic;
using Tederean.FastIOW;

namespace Button
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

    private static readonly Dictionary<IOWarriorType, int> ButtonDefinitions = new Dictionary<IOWarriorType, int>()
    {
      { IOWarriorType.IOWarrior40, IOWarrior40.P0_0 },
      { IOWarriorType.IOWarrior24, IOWarrior24.P0_0 },
      { IOWarriorType.IOWarrior56, IOWarrior56.P6_0 },
      { IOWarriorType.IOWarrior28, IOWarrior28.P2_1 },
      { IOWarriorType.IOWarrior28L, IOWarrior28L.P0_0 }
    };

    // This example lights up a LED as long a pushbutton is pressed. See above for pin definitions. 
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

      foreach (GPIO gpio in FastIOW.GetPeripherals<GPIO>())
      {
        gpio.PinStateChange += OnPinStateChange;
      }

      Console.WriteLine("\nPress any key to exit.");
      Console.ReadKey();

      FastIOW.CloseConnection();
    }

    private static void OnPinStateChange(object sender, PinStateChangeEventArgs args)
    {
      if (args.Pin == ButtonDefinitions[args.IOWarrior.Type])
      {
        args.GPIO.DigitalWrite(LedDefinitions[args.IOWarrior.Type], args.NewPinState);
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
