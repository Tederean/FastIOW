﻿using System;
using Tederean.FastIOW.Internal;

namespace Tederean.FastIOW
{

  public class IOWarrior28L : IOWarriorBase
  {

    public override string Name => "IOWarrior28L";

    public override IOWarriorType Type => IOWarriorType.IOWarrior28L;

    internal override int StandardReportSize => 5;

    internal override int SpecialReportSize => 64;

    internal override Pipe[] SupportedPipes => new[] { Pipe.IO_PINS, Pipe.SPECIAL_MODE };


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

    public const int P2_1 = 3 * 8 + 1;

    public const int I2C_SCL = P0_1;
    public const int I2C_SDA = P0_2;


    internal IOWarrior28L(IntPtr handle) : base(handle)
    {
      InterfaceList.Add(new GPIOImplementation(this));
      InterfaceList.Add(new I2CImplementation(this, Pipe.SPECIAL_MODE, 6));
    }


    internal override bool IsValidDigitalPin(int pin)
    {
      return pin >= P0_0 && (pin <= P1_7 || pin == P2_1);
    }
  }
}
