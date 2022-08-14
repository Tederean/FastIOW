using System;
using System.Linq;
using System.Threading;
using Tederean.FastIOW;

namespace I2C_BME280
{

  public class BME280
  {

    private readonly I2C m_I2C;

    private readonly BME280Address m_Address;

    private readonly Coefficients m_Coefficients;

    private readonly ControlMeasurement m_ControlMeasurement;

    private readonly ControlHumidity m_ControlHumidity;

    private readonly Config m_Config;


    private int t_fine; //!< temperature with high resolution, stored as an attribute
                        //!< as this is used for temperature compensation reading
                        //!< humidity and pressure

    private int t_fine_adjust = 0; //!< add to compensate temp readings and in turn
                                   //!< to pressure and humidity readings


    public BME280(I2C i2c, BME280Address address)
    {
      m_I2C = i2c;
      m_Address = address;
      m_ControlMeasurement = new ControlMeasurement();
      m_ControlHumidity = new ControlHumidity();
      m_Config = new Config();

      if (ReadByte(BME280Register.CHIPID) != 0x60) // check if sensor, i.e. the chip ID is correct
      {
        throw new InvalidOperationException("Device is not a BME280.");
      }

      // reset the device using soft-reset
      // this makes sure the IIR is off, etc.
      m_I2C.WriteBytes((byte)m_Address, (byte)BME280Register.SOFTRESET, 0xB6);

      // wait for chip to wake up.
      Thread.Sleep(10);

      while (IsBusy()) // if chip is still reading calibration, delay
      {
        Thread.Sleep(10);
      }

      m_Coefficients = ReadCoefficients(); // read trimming parameters, see DS 4.2.2

      SetSampling();

      Thread.Sleep(100);
    }


    public float ReadTemperature()
    {
      int var1, var2;

      int adc_T = Read3BytesSigned(BME280Register.TEMPDATA);

      if (adc_T == 0x800000) // value in case temp measurement was disabled
        return float.NaN;
      adc_T >>= 4;

      var1 = (int)((adc_T / 8) - ((int)m_Coefficients.T1 * 2));
      var1 = (var1 * ((int)m_Coefficients.T2)) / 2048;
      var2 = (int)((adc_T / 16) - ((int)m_Coefficients.T1));
      var2 = (((var2 * var2) / 4096) * ((int)m_Coefficients.T3)) / 16384;

      t_fine = var1 + var2 + t_fine_adjust;

      int T = (t_fine * 5 + 128) / 256;

      return (float)T / 100.0f;
    }

    public float ReadPressure()
    {
      long var1, var2, var3, var4;

      ReadTemperature(); // must be done first to get t_fine

      int adc_P = Read3BytesSigned(BME280Register.PRESSUREDATA);
      if (adc_P == 0x800000) // value in case pressure measurement was disabled
        return float.NaN;
      adc_P >>= 4;

      var1 = ((long)t_fine) - 128000;
      var2 = var1 * var1 * (long)m_Coefficients.P6;
      var2 = var2 + ((var1 * (long)m_Coefficients.P5) * 131072);
      var2 = var2 + (((long)m_Coefficients.P4) * 34359738368);
      var1 = ((var1 * var1 * (long)m_Coefficients.P3) / 256) +
             ((var1 * ((long)m_Coefficients.P2) * 4096));
      var3 = ((long)1) * 140737488355328;
      var1 = (var3 + var1) * ((long)m_Coefficients.P1) / 8589934592;

      if (var1 == 0)
      {
        return 0; // avoid exception caused by division by zero
      }

      var4 = 1048576 - adc_P;
      var4 = (((var4 * 2147483648) - var2) * 3125) / var1;
      var1 = (((long)m_Coefficients.P9) * (var4 / 8192) * (var4 / 8192)) /
             33554432;
      var2 = (((long)m_Coefficients.P8) * var4) / 524288;
      var4 = ((var4 + var1 + var2) / 256) + (((long)m_Coefficients.P7) * 16);

      float P = var4 / 256.0f / 100.0f;

      return P;
    }

    public float ReadHumidity()
    {
      int var1, var2, var3, var4, var5;

      ReadTemperature(); // must be done first to get t_fine

      // adc_H: 24972
      // 47.53 %

      int adc_H = Read2Bytes(BME280Register.HUMIDDATA);
      if (adc_H == 0x8000) // value in case humidity measurement was disabled
        return float.NaN;

      var1 = t_fine - ((int)76800);
      var2 = (int)(adc_H * 16384);
      var3 = (int)(((int)m_Coefficients.H4) * 1048576);
      var4 = ((int)m_Coefficients.H5) * var1;
      var5 = (((var2 - var3) - var4) + (int)16384) / 32768;
      var2 = (var1 * ((int)m_Coefficients.H6)) / 1024;
      var3 = (var1 * ((int)m_Coefficients.H3)) / 2048;
      var4 = ((var2 * (var3 + (int)32768)) / 1024) + (int)2097152;
      var2 = ((var4 * ((int)m_Coefficients.H2)) + 8192) / 16384;
      var3 = var5 * var2;
      var4 = ((var3 / 32768) * (var3 / 32768)) / 128;
      var5 = var3 - ((var4 * ((int)m_Coefficients.H1)) / 16);
      var5 = (var5 < 0 ? 0 : var5);
      var5 = (var5 > 419430400 ? 419430400 : var5);
      uint H = (uint)(var5 / 4096);

      return (float)H / 1024.0f;
    }




    private void SetSampling(SensorMode mode = SensorMode.MODE_NORMAL, SensorSampling tempSampling = SensorSampling.SAMPLING_X16, SensorSampling pressSampling = SensorSampling.SAMPLING_X16, SensorSampling humSampling = SensorSampling.SAMPLING_X16, SensorFilter filter = SensorFilter.FILTER_OFF, StandbyDuration duration = StandbyDuration.STANDBY_MS_0_5)
    {
      m_ControlMeasurement.DeviceMode = (byte)mode;
      m_ControlMeasurement.TemperatureOversampling = (byte)tempSampling;
      m_ControlMeasurement.PressureOversampling = (byte)pressSampling;

      m_ControlHumidity.PressureOversampling = (byte)humSampling;
      m_Config.Filter = (byte)filter;
      m_Config.StandbyTime = (byte)duration;
      m_Config.Spi3w_en = 0;

      // making sure sensor is in sleep mode before setting configuration
      // as it otherwise may be ignored
      m_I2C.WriteBytes((byte)m_Address, (byte)BME280Register.CONTROL, (byte)SensorMode.MODE_SLEEP);

      // you must make sure to also set REGISTER_CONTROL after setting the
      // CONTROLHUMID register, otherwise the values won't be applied (see
      // DS 5.4.3)

      m_I2C.WriteBytes((byte)m_Address, (byte)BME280Register.CONTROLHUMID, m_ControlHumidity.Get());
      m_I2C.WriteBytes((byte)m_Address, (byte)BME280Register.CONFIG, m_Config.Get());
      m_I2C.WriteBytes((byte)m_Address, (byte)BME280Register.CONTROL, m_ControlMeasurement.Get());
    }

    private Coefficients ReadCoefficients()
    {
      return new Coefficients()
      {
        T1 = Read2Bytes(BME280Register.T1),
        T2 = Read2BytesSigned(BME280Register.T2),
        T3 = Read2BytesSigned(BME280Register.T3),

        P1 = Read2Bytes(BME280Register.P1),
        P2 = Read2BytesSigned(BME280Register.P2),
        P3 = Read2BytesSigned(BME280Register.P3),
        P4 = Read2BytesSigned(BME280Register.P4),
        P5 = Read2BytesSigned(BME280Register.P5),
        P6 = Read2BytesSigned(BME280Register.P6),
        P7 = Read2BytesSigned(BME280Register.P7),
        P8 = Read2BytesSigned(BME280Register.P8),
        P9 = Read2BytesSigned(BME280Register.P9),

        H1 = ReadByte(BME280Register.H1),
        H2 = Read2BytesSigned(BME280Register.H2),
        H3 = ReadByte(BME280Register.H3),
        H4 = (short)(((sbyte)ReadByte(BME280Register.H4) << 4) | (ReadByte(BME280Register.H4 + 1) & 0xF)),
        H5 = (short)(((sbyte)ReadByte(BME280Register.H5 + 1) << 4) | (ReadByte(BME280Register.H5) >> 4)),
        H6 = (sbyte)ReadByte(BME280Register.H6),
      };
    }

    private bool IsBusy()
    {
      return ReadByte(BME280Register.STATUS).GetBit(0);
    }

    private ushort Read2Bytes(BME280Register register)
    {
      m_I2C.WriteBytes((byte)m_Address, (byte)register);

      var result = m_I2C.ReadBytes((byte)m_Address, 2);

      return (ushort)((result[1] << 8) + result[0]);
    }

    private short Read2BytesSigned(BME280Register register)
    {
      m_I2C.WriteBytes((byte)m_Address, (byte)register);

      var result = m_I2C.ReadBytes((byte)m_Address, 2);

      return (short)((result[1] << 8) + result[0]);
    }

    private int Read3BytesSigned(BME280Register register)
    {
      m_I2C.WriteBytes((byte)m_Address, (byte)register);

      var result = m_I2C.ReadBytes((byte)m_Address, 3);

      return BitConverter.ToInt32(new byte[] { result[2], result[1], result[0], 0x00 }, 0);
    }

    private byte ReadByte(BME280Register register)
    {
      m_I2C.WriteBytes((byte)m_Address, (byte)register);

      return m_I2C.ReadByte((byte)m_Address);
    }


    private class Config
    {
      // inactive duration (standby time) in normal mode
      // 000 = 0.5 ms
      // 001 = 62.5 ms
      // 010 = 125 ms
      // 011 = 250 ms
      // 100 = 500 ms
      // 101 = 1000 ms
      // 110 = 10 ms
      // 111 = 20 ms
      public byte StandbyTime = 3; ///< inactive duration (standby time) in normal mode

      // filter settings
      // 000 = filter off
      // 001 = 2x filter
      // 010 = 4x filter
      // 011 = 8x filter
      // 100 and above = 16x filter
      public byte Filter = 3; ///< filter settings

      // unused - don't set
      public byte None = 1;     ///< unused - don't set
      public byte Spi3w_en = 1; ///< unused - don't set


      /// @return combined config register
      public byte Get()
      {
        return (byte)((StandbyTime << 5) | (Filter << 2) | Spi3w_en);
      }
    };

    private class ControlHumidity
    {

      /// unused - don't set
      public byte None = 5;

      // pressure oversampling
      // 000 = skipped
      // 001 = x1
      // 010 = x2
      // 011 = x4
      // 100 = x8
      // 101 and above = x16
      public byte PressureOversampling = 3; ///< pressure oversampling


      /// @return combined ctrl hum register
      public byte Get()
      {
        return PressureOversampling;
      }
    };

    private class ControlMeasurement
    {

      // temperature oversampling
      // 000 = skipped
      // 001 = x1
      // 010 = x2
      // 011 = x4
      // 100 = x8
      // 101 and above = x16
      public byte TemperatureOversampling = 3; ///< temperature oversampling

      // pressure oversampling
      // 000 = skipped
      // 001 = x1
      // 010 = x2
      // 011 = x4
      // 100 = x8
      // 101 and above = x16
      public byte PressureOversampling = 3; ///< pressure oversampling

      // device mode
      // 00       = sleep
      // 01 or 10 = forced
      // 11       = normal
      public byte DeviceMode = 2; ///< device mode


      /// @return combined ctrl register
      public byte Get()
      {
        return (byte)((TemperatureOversampling << 5) | (PressureOversampling << 2) | DeviceMode);
      }
    }

    private class Coefficients
    {
      public ushort T1 { get; set; }
      public short T2 { get; set; }
      public short T3 { get; set; }

      public ushort P1 { get; set; }
      public short P2 { get; set; }
      public short P3 { get; set; }
      public short P4 { get; set; }
      public short P5 { get; set; }
      public short P6 { get; set; }
      public short P7 { get; set; }
      public short P8 { get; set; }
      public short P9 { get; set; }

      public byte H1 { get; set; }
      public short H2 { get; set; }
      public byte H3 { get; set; }
      public short H4 { get; set; }
      public short H5 { get; set; }
      public sbyte H6 { get; set; }
    }

  }


  public enum BME280Address : byte
  {
    Primary = 0x77,
    Secondary = 0x76,
  }

  public enum BME280Register : byte
  {
    T1 = 0x88,
    T2 = 0x8A,
    T3 = 0x8C,

    P1 = 0x8E,
    P2 = 0x90,
    P3 = 0x92,
    P4 = 0x94,
    P5 = 0x96,
    P6 = 0x98,
    P7 = 0x9A,
    P8 = 0x9C,
    P9 = 0x9E,

    H1 = 0xA1,
    H2 = 0xE1,
    H3 = 0xE3,
    H4 = 0xE4,
    H5 = 0xE5,
    H6 = 0xE7,

    CHIPID = 0xD0,
    VERSION = 0xD1,
    SOFTRESET = 0xE0,

    CAL26 = 0xE1, // R calibration stored in 0xE1-0xF0

    CONTROLHUMID = 0xF2,
    STATUS = 0XF3,
    CONTROL = 0xF4,
    CONFIG = 0xF5,
    PRESSUREDATA = 0xF7,
    TEMPDATA = 0xFA,
    HUMIDDATA = 0xFD
  }

  public enum SensorSampling : byte
  {
    SAMPLING_NONE = 0b000,
    SAMPLING_X1 = 0b001,
    SAMPLING_X2 = 0b010,
    SAMPLING_X4 = 0b011,
    SAMPLING_X8 = 0b100,
    SAMPLING_X16 = 0b101
  };

  public enum SensorMode : byte
  {
    MODE_SLEEP = 0b00,
    MODE_FORCED = 0b01,
    MODE_NORMAL = 0b11
  };

  public enum SensorFilter : byte
  {
    FILTER_OFF = 0b000,
    FILTER_X2 = 0b001,
    FILTER_X4 = 0b010,
    FILTER_X8 = 0b011,
    FILTER_X16 = 0b100
  };

  public enum StandbyDuration : byte
  {
    STANDBY_MS_0_5 = 0b000,
    STANDBY_MS_10 = 0b110,
    STANDBY_MS_20 = 0b111,
    STANDBY_MS_62_5 = 0b001,
    STANDBY_MS_125 = 0b010,
    STANDBY_MS_250 = 0b011,
    STANDBY_MS_500 = 0b100,
    STANDBY_MS_1000 = 0b101
  };
}
