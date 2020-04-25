using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tederean.FastIOW;

namespace I2C_PCA9555
{
  /// <summary>
  /// This example is the C# variation of the Arduino sketch from:
  /// PCA9555 Portexpander https://www.loetstelle.net/arduino/pca9555_wire/pca9555_wire.html
  /// </summary>
  public class Program
  {
    public readonly byte PCA9555_ADDRESS = 0x20;
    public readonly byte PCA9555_INPUT_0 = 0;
    public readonly byte PCA9555_INPUT_1 = 1;
    public readonly byte PCA9555_OUTPUT_0 = 2;
    public readonly byte PCA9555_OUTPUT_1 = 3;
    public readonly byte PCA9555_PI_0 = 4;
    public readonly byte PCA9555_PI_1 = 5;
    public readonly byte PCA9555_CFG_0 = 6;
    public readonly byte PCA9555_CFG_1 = 7;

    public I2CInterface I2C { get; set; }

    public static void Main(string[] args)
    {
      var program = new Program();

      program.Setup();
      while (true)
      {
        program.Loop();
      }
    }

    public static void PrintDeviceInfos(IOWarrior iow)
    {
      Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|", "Name", "Unique Identifier", "Serial Number"));
      Console.WriteLine(string.Format("|{0,20}|{1,20}|{2,20}|", iow.Name, string.Format("0x{0:X8}", iow.Id), iow.SerialNumber));
    }

    public void Setup()
    {
      FastIOW.OpenConnection();

      // Find the first IO-Warrior device which supports I2C
      var device = FastIOW.GetIOWarriors().Where(entry => entry is I2CDevice).FirstOrDefault();
      if (device == null)
      {
        FastIOW.CloseConnection();
        Console.WriteLine("Error: No I2C capable IOWarriors detected!");
        Console.ReadKey();
        Environment.Exit(1);
      }

      PrintDeviceInfos(device);

      I2C = (device as I2CDevice).I2C;

      I2C.Enable();

      // Config all PCA9555 port pins as output
      I2C.WriteBytes(PCA9555_ADDRESS, PCA9555_CFG_0, 0x00, 0x00);

      Console.WriteLine("\nPress any key to exit.");
    }

    public void Loop()
    {
      if (!Console.KeyAvailable)
      {
        Blink();
      }
      else
      {
        FastIOW.CloseConnection();
        Console.ReadKey();
        Environment.Exit(0);
      }
    }

    public void Blink()
    {
      // Set delay to zero to get maximum frequency
      // (e.g. 166.6 Hz on Win10 x64 with AMD Ryzen 5 2400G and IOWarrior 28)
      int delay = 500; // 0;

      // Set all output pins to high
      I2C.WriteBytes(PCA9555_ADDRESS, PCA9555_OUTPUT_0, 0xFF, 0xFF);
      Thread.Sleep(delay);

      // Set all outputs pins to low
      I2C.WriteBytes(PCA9555_ADDRESS, PCA9555_OUTPUT_0, 0x00, 0x00);
      Thread.Sleep(delay);
    }
  }
}
