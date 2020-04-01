using System;
using System.Linq;
using Tederean.FastIOW;

namespace Button
{

  class Program
  {

    static void Main(string[] args)
    {
      FastIOW.OpenConnection();

      if (!FastIOW.Connected)
      {
        Console.WriteLine("No IOWarrior detected!");
        Console.ReadKey();
        return;
      }


      Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|{3,20}|", "Name", "Unique Identifier", "Serial Number", "I2C Address"));

      foreach (IOWarrior iow in FastIOW.GetIOWarriors())
      {
        iow.EnableI2C();

        foreach (byte index in Enumerable.Range(0, 127))
        {
          try
          {
            iow.I2CWriteBytes(index);

            Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|{3,20}|", iow.Type.Name, string.Format("0x{0:X8}", iow.Type.Id), iow.SerialNumber, string.Format("0x{0:X2}", index)));
          }
          catch (Exception) { }
        }

        iow.DisableI2C();
      }

      FastIOW.CloseConnection();

      Console.WriteLine("\nPress any key to exit.");
      Console.ReadKey();
    }
  }
}
