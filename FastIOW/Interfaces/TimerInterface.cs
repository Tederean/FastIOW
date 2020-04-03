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
  /// Represents a Timer interface of an IOWarrior.
  /// </summary>
  public interface TimerInterface
  {

    /// <summary>
    /// Returns true if Timer interface is enabled, otherwise false.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Enable Timer interface on this IOWarrior device.
    /// Set the channels that should be used.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Enable(TimerConfig config);

    /// <summary>
    /// Disable Timer interface on this IOWarrior device.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="IOException"/>
    void Disable();
  }

  /// <summary>
  /// Represents a configuation of Timer channels.
  /// </summary>
  public enum TimerConfig
  {
    Timer_1 = 1,
    Timer_1To2 = 2,
  }
}
