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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tederean.FastIOW.Internal
{

  public abstract class Enumeration : IComparable
  {

    /// <summary>
    /// Returns name of enum.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Returns id of enum.
    /// </summary>
    public UInt32 Id { get; private set; }

    protected Enumeration(UInt32 id, string name)
    {
      Id = id;
      Name = name;
    }

    public override string ToString() => Name;

    /// <summary>
    /// Returns a collection of defined enum constants.
    /// </summary>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
      var fields = typeof(T).GetFields(BindingFlags.Public |
                                       BindingFlags.Static |
                                       BindingFlags.DeclaredOnly);

      return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Enumeration otherValue))
        return false;

      var typeMatches = GetType().Equals(obj.GetType());
      var valueMatches = Id.Equals(otherValue.Id);

      return typeMatches && valueMatches;
    }

    public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

    public override int GetHashCode() => Id.GetHashCode();
  }
}
