/*
 *   
 *   Copyright 2020 Florian Porsch <tederean@gmail.com>
 *   
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation; either version 3 of the License, or
 *   (at your option) any later version.
 *   
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Lesser General Public License for more details.
 *   
 *   You should have received a copy of the GNU Lesser General Public License
 *   along with this program; if not, write to the Free Software
 *   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 *   MA 02110-1301 USA.
 *
 */
namespace Tederean.FastIOW
{

  /// <summary>
  /// Class for holding pinout definitions for all IOWarrior boards.
  /// </summary>
  public static class Pinout
  {

    /// <summary>
    /// Class for holding pinout definitions for IOWarrior40.
    /// </summary>
    public static class IOWarrior40
    {
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
      public const int P2_2 = 3 * 8 + 2;
      public const int P2_3 = 3 * 8 + 3;
      public const int P2_4 = 3 * 8 + 4;
      public const int P2_5 = 3 * 8 + 5;
      public const int P2_6 = 3 * 8 + 6;
      public const int P2_7 = 3 * 8 + 7;

      public const int P3_0 = 4 * 8 + 0;
      public const int P3_1 = 4 * 8 + 1;
      public const int P3_2 = 4 * 8 + 2;
      public const int P3_3 = 4 * 8 + 3;
      public const int P3_4 = 4 * 8 + 4;
      public const int P3_5 = 4 * 8 + 5;
      public const int P3_6 = 4 * 8 + 6;
      public const int P3_7 = 4 * 8 + 7;

      public const int I2C_SCL = P0_6;
      public const int I2C_SDA = P0_7;


      public const int BUILDIN_BUTTON = P0_0;

      public const int BUILDIN_LED0 = P3_0;
      public const int BUILDIN_LED1 = P3_1;
      public const int BUILDIN_LED2 = P3_2;
      public const int BUILDIN_LED3 = P3_3;
      public const int BUILDIN_LED4 = P3_4;
      public const int BUILDIN_LED5 = P3_5;
      public const int BUILDIN_LED6 = P3_6;
      public const int BUILDIN_LED7 = P3_7;
    }


    /// <summary>
    /// Class for holding pinout definitions for IOWarrior24.
    /// </summary>
    public static class IOWarrior24
    {
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

      public const int IR_DATA = P0_0;

      public const int I2C_SCL = P0_1;
      public const int I2C_SDA = P0_2;

      public const int SPI_DRDY = P0_3;
      public const int SPI_SS = P0_4;
      public const int SPI_MOSI = P0_5;
      public const int SPI_MISO = P0_6;
      public const int SPI_SCK = P0_7;


      public const int BUILDIN_LED = P0_3;
    }


    /// <summary>
    /// Class for holding pinout definitions for IOWarrior56.
    /// </summary>
    public static class IOWarrior56
    {
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
      public const int P2_2 = 3 * 8 + 2;
      public const int P2_3 = 3 * 8 + 3;
      public const int P2_4 = 3 * 8 + 4;
      public const int P2_5 = 3 * 8 + 5;
      public const int P2_6 = 3 * 8 + 6;
      public const int P2_7 = 3 * 8 + 7;

      public const int P3_0 = 4 * 8 + 0;
      public const int P3_1 = 4 * 8 + 1;
      public const int P3_2 = 4 * 8 + 2;
      public const int P3_3 = 4 * 8 + 3;
      public const int P3_4 = 4 * 8 + 4;
      public const int P3_5 = 4 * 8 + 5;
      public const int P3_6 = 4 * 8 + 6;
      public const int P3_7 = 4 * 8 + 7;

      public const int P4_0 = 5 * 8 + 0;
      public const int P4_1 = 5 * 8 + 1;
      public const int P4_2 = 5 * 8 + 2;
      public const int P4_3 = 5 * 8 + 3;
      public const int P4_4 = 5 * 8 + 4;
      public const int P4_5 = 5 * 8 + 5;
      public const int P4_6 = 5 * 8 + 6;
      public const int P4_7 = 5 * 8 + 7;

      public const int P5_0 = 6 * 8 + 0;
      public const int P5_1 = 6 * 8 + 1;
      public const int P5_2 = 6 * 8 + 2;
      public const int P5_3 = 6 * 8 + 3;
      public const int P5_4 = 6 * 8 + 4;
      public const int P5_5 = 6 * 8 + 5;
      public const int P5_6 = 6 * 8 + 6;
      public const int P5_7 = 6 * 8 + 7;

      /// <summary>
      /// On some boards P6.0 is labled as P7.0.
      /// </summary>
      public const int P6_0 = 7 * 8 + 0;
      /// <summary>
      /// On some boards P6.7 is labled as P7.7.
      /// </summary>
      public const int P6_7 = 7 * 8 + 7;

      /// <summary>
      /// On some boards P6.0 is labled as P7.0.
      /// </summary>
      public const int P7_0 = P6_0;
      /// <summary>
      /// On some boards P6.7 is labled as P7.7.
      /// </summary>
      public const int P7_7 = P6_7;

      public const int I2C_SCL = P1_7;
      public const int I2C_SDA = P1_5;

      public const int SPI_DRDY = P5_3;
      public const int SPI_SS = P5_1;
      public const int SPI_MOSI = P5_2;
      public const int SPI_MISO = P5_4;
      public const int SPI_SCK = P5_0;

      public const int ADC_AGND = P2_4;
      public const int ADC_AREF = P2_6;

      public const int ADC_0 = P0_0;
      public const int ADC_1 = P0_1;
      public const int ADC_2 = P0_2;
      public const int ADC_3 = P0_3;
      public const int ADC_4 = P0_4;
      public const int ADC_5 = P0_5;
      public const int ADC_6 = P0_6;
      public const int ADC_7 = P0_7;

      public const int PWM_1 = P6_7;
      public const int PWM_2 = P6_0;


      public const int BUILDIN_LED = P6_7;

      public const int BUILDIN_BUTTON = P6_0;
    }


    /// <summary>
    /// Class for holding pinout definitions for IOWarrior28.
    /// </summary>
    public static class IOWarrior28
    {
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


      public const int BUILDIN_LED = P2_0;

      public const int BUILDIN_BUTTON = P2_1;
    }


    /// <summary>
    /// Class for holding pinout definitions for IOWarrior28L.
    /// </summary>
    public static class IOWarrior28L
    {
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
    }
  }
}
