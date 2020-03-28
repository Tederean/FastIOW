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


      public const int BUILDIN_LED = P0_3;
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

      public const int AD0 = P1_0;
      public const int AD1 = P1_1;
      public const int AD2 = P1_2;
      public const int AD3 = P1_3;

      public const int BUTTON_S3 = P1_0;
      public const int BUTTON_S2 = P1_1;
      public const int BUTTON_S1 = P1_2;

      public const int LED_GREEN = P1_5;
      public const int LED_YELLOW = P1_6;
      public const int LED_RED = P1_7;
    }
  }
}
