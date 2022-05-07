namespace Tederean.FastIOW
{

  /// <summary>
  /// A report id represents the first byte in an IOWarrior report.
  /// </summary>
  public static class ReportId
  {

    public const byte ADC_SETUP = 0x1C;
    public const byte ADC_READ = 0x1D;

    public const byte I2C_SETUP = 0x01;
    public const byte I2C_WRITE = 0x02;
    public const byte I2C_READ = 0x03;

    public const byte PWM_SETUP = 0x20;

    public const byte SPI_SETUP = 0x08;
    public const byte SPI_TRANSFER = 0x09;

    public const byte TIMER_SETUP = 0x28;
    public const byte TIMER_DATA_A = 0x29;
    public const byte TIMER_DATA_B = 0x2A;

    public const byte GPIO_READ_WRITE = 0x00;
    public const byte GPIO_SPECIAL_READ = 0xFF;
  }
}