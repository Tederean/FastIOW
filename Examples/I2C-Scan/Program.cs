using System;
using System.Linq;
using Tederean.FastIOW;

namespace I2C_Scan
{

  class Program
  {

    // This example scans I2C interface of all connected IOWarriors
    // for I2C slave devices, printing their address to console.
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


      Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|{3,20}|", "Name", "Unique Identifier", "Serial Number", "I2C Address"));

      foreach (I2C i2c in FastIOW.GetPeripherals<I2C>())
      {
        i2c.Enable();

        foreach (byte address in Enumerable.Range(0, 127))
        {
          if (i2c.IsAvailable(address))
          {
            Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|{3,20}|", i2c.IOWarrior.Name, string.Format("0x{0:X8}", i2c.IOWarrior.Id), i2c.IOWarrior.SerialNumber, string.Format("0x{0:X2}", address)));
          }
        }

        i2c.Disable();
      }

      FastIOW.CloseConnection();

      Console.WriteLine("\nPress any key to exit.");
      Console.ReadKey();
    }
  }
}
