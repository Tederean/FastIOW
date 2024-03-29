﻿using System;
using System.IO;
using Tederean.FastIOW.Internal;

namespace Tederean.FastIOW
{

  public class IOWarrior28 : IOWarriorBase
  {

    public override string Name => "IOWarrior28";

    public override IOWarriorType Type => IOWarriorType.IOWarrior28;

    internal override int StandardReportSize => 5;

    internal override int SpecialReportSize => 64;

    internal override Pipe[] SupportedPipes => new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE, Pipe.I2C_MODE, Pipe.ADC_MODE };

    internal int[] AnalogPins => new[] { ADC_0, ADC_1, ADC_2, ADC_3 };


    public const int P0_0 = 1 * 8 + 0;
    public const int P0_1 = 1 * 8 + 1;
    public const int P0_2 = 1 * 8 + 2;
    public const int P0_3 = 1 * 8 + 3;
    public const int P0_4 = 1 * 8 + 4;
    public const int P0_5 = 1 * 8 + 5;
    public const int P0_6 = 1 * 8 + 6;
    public const int P0_7 = 1 * 8 + 7;

    public const int P1_0 = 2 * 8 + 0;
    public const int P1_1 = 2 * 8 + 1;
    public const int P1_2 = 2 * 8 + 2;
    public const int P1_3 = 2 * 8 + 3;
    public const int P1_4 = 2 * 8 + 4;
    public const int P1_5 = 2 * 8 + 5;
    public const int P1_6 = 2 * 8 + 6;
    public const int P1_7 = 2 * 8 + 7;

    public const int P2_0 = 3 * 8 + 0;
    public const int P2_1 = 3 * 8 + 1;

    public const int P3_7 = 4 * 8 + 7;

    public const int ADC_0 = P1_0;
    public const int ADC_1 = P1_1;
    public const int ADC_2 = P1_2;
    public const int ADC_3 = P1_3;

    public const int I2C_SCL = P2_1;
    public const int I2C_SDA = P2_0;

    public const int Pin_4 = P3_7;
    public const int Pin_5 = P0_2;
    public const int Pin_6 = P0_3;
    public const int Pin_7 = P0_4;
    public const int Pin_8 = P0_5;
    public const int Pin_9 = P0_6;
    public const int Pin_10 = P0_7;
    public const int Pin_11 = P2_0;
    public const int Pin_12 = P2_1;

    public const int Pin_17 = P1_0;
    public const int Pin_18 = P1_1;
    public const int Pin_19 = P1_2;
    public const int Pin_20 = P1_3;
    public const int Pin_21 = P1_4;
    public const int Pin_22 = P1_5;
    public const int Pin_23 = P1_6;
    public const int Pin_24 = P1_7;
    public const int Pin_25 = P0_0;
    public const int Pin_26 = P0_1;


    internal IOWarrior28(IntPtr handle) : base(handle)
    {
      if (IsNotDongleVersion())
      {
        InterfaceList.Add(new GPIOImplementation(this));
        InterfaceList.Add(new ADCImplementation(this, Pipe.ADC_MODE, AnalogPins));
      }

      InterfaceList.Add(new I2CImplementation(this, Pipe.I2C_MODE, 62));
    }


    internal override bool IsValidDigitalPin(int pin)
    {
      return pin >= P0_0 && (pin <= P2_1 || pin == P3_7);
    }

    private bool IsNotDongleVersion()
    {
      var report = NewReport(Pipe.ADC_MODE);

      report[0] = ReportId.ADC_SETUP;
      report[1] = 0x00; // ADC Disable

      return TryWriteReport(report, Pipe.ADC_MODE);
    }
  }
}
