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

      foreach (IOWarrior iow in FastIOW.GetIOWarriors().Where(entry => entry is I2CDevice))
      {
        I2CInterface i2c = (iow as I2CDevice).I2CInterface;

        i2c.Enable();

        foreach (byte address in Enumerable.Range(0, 127))
        {
          try
          {
            // Throws IOException when I2C device is not responding.
            i2c.WriteBytes(address);

            // This is only called when I2C device send a response.
            Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|{3,20}|", iow.Name, string.Format("0x{0:X8}", iow.Id), iow.SerialNumber, string.Format("0x{0:X2}", address)));
          }
          catch (IOException) { }
        }

        i2c.Disable();
      }

      FastIOW.CloseConnection();

      Console.WriteLine("\nPress any key to exit.");
      Console.ReadKey();
    }
  }
}
