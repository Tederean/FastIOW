namespace Tederean.FastIOW
{

  /// <summary>
  /// Represents a generic peripheral of an IOWarrior.
  /// </summary>
  public interface Peripheral
  {

    /// <summary>
    /// Returns the IOWarrior device that this peripheral is belonging to.
    /// </summary>
    IOWarrior IOWarrior { get; }
  }
}
