using System;
using System.IO;
using System.Linq;
using Tederean.FastIOW;

namespace Button
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
        Console.WriteLine("No IOWarrior detected!");
        Console.ReadKey();
        return;
      }


      Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|{3,20}|", "Name", "Unique Identifier", "Serial Number", "I2C Address"));

      foreach (IOWarrior iow in FastIOW.GetIOWarriors())
      {
        iow.EnableI2C();

        foreach (byte address in Enumerable.Range(0, 127))
        {
          try
          {
            // Throws IOException when I2C device is not responding.
            iow.I2CWriteBytes(address);

            // This is only called when I2C device send a response.
            Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|{3,20}|", iow.Type.Name, string.Format("0x{0:X8}", iow.Type.Id), iow.SerialNumber, string.Format("0x{0:X2}", address)));
          }
          catch (IOException) { }
        }

        iow.DisableI2C();
      }

      FastIOW.CloseConnection();

      Console.WriteLine("\nPress any key to exit.");
      Console.ReadKey();
    }
  }
}
