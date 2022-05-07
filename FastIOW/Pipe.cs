using Tederean.FastIOW.Internal;

namespace Tederean.FastIOW
{

  /// <summary>
  /// A pipe that can be used in IOWarrior ecosystem.
  /// </summary>
  public class Pipe : Enumeration
  {

    /// <summary>
    /// Pipe used for addressing special mode functions.
    /// </summary>
    public static readonly Pipe IO_PINS = new Pipe(0, "IO_PINS");

    /// <summary>
    /// Pipe used for addressing input output pins.
    /// </summary>
    public static readonly Pipe SPECIAL_MODE = new Pipe(1, "SPECIAL_MODE");

    /// <summary>
    /// Pipe used for addressing hardware I2C interface. Pipe only supported by IOWarrior28.
    /// </summary>
    public static readonly Pipe I2C_MODE = new Pipe(2, "I2C_MODE");

    /// <summary>
    /// Pipe used for addressing buildin analog to digital converter. Pipe only supported by IOWarrior28.
    /// </summary>
    public static readonly Pipe ADC_MODE = new Pipe(3, "ADC_MODE");


    internal Pipe(uint Id, string Name) : base(Id, Name) { }
  }
}