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
    public int Id { get; private set; }

    protected Enumeration(int id, string name)
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
      var otherValue = obj as Enumeration;

      if (otherValue == null)
        return false;

      var typeMatches = GetType().Equals(obj.GetType());
      var valueMatches = Id.Equals(otherValue.Id);

      return typeMatches && valueMatches;
    }

    public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

    public override int GetHashCode() => Id.GetHashCode();
  }
}
