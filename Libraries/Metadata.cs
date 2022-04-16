using System;
using System.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
namespace Libraries;
public struct MetaData : IEnumerable, IComparable<MetaData>
{
  public MetaDataEntry[] Entries { get; init; }
  public int Count { get => Entries.Length; }
  public MetaData(params MetaDataEntry[] es) {
    Entries = es;
  }
  public object? this[string name]
  {
    get
    {
      try
      {
        return Array.Find(Entries,entry => entry.AttributeName.Equals(name));
      }
      catch
      {
        return null;
      }
    }
  }
  public int CompareTo(MetaData other) => Count.CompareTo(other.Count);
  public override bool Equals(object? obj) => obj!= null && base.Equals(obj);
  public override int GetHashCode() => HashCode.Combine<MetaDataEntry[]>(Entries);
  IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
  public IEnumerator<MetaDataEntry> GetEnumerator()
  {
    Array.Sort(Entries);
    foreach(MetaDataEntry entry in Entries)
    {
      yield return entry;
    }
  }
  public static implicit operator MetaData(MetaDataEntry[] Entries) => new MetaData(Entries);
}
public record struct MetaDataEntry(string AttributeName, object AttributeValue) : IComparable<MetaDataEntry>
{
  public string AttributeName { get; init; } = AttributeName;
  public object AttributeValue{get; init;} = AttributeValue;
  public Type AttributeType { get; init;} = AttributeValue.GetType();
  public override string ToString()
  {
    return $"{AttributeName}: {AttributeType}({AttributeValue.ToString()})";
  }
  public int CompareTo(MetaDataEntry other)
  {
    return AttributeName.CompareTo(other.AttributeName);
  }
  public static implicit operator MetaDataEntry((string an, object av) setup) => new(AttributeName: setup.an, AttributeValue: setup.av);
}
public record struct NamedObject<ObjectType>(string Name, ObjectType Value) where ObjectType : notnull
{
  public string Name { get; init; } = Name;
  public Type type { get; init; } = Value.GetType();
  public ObjectType Value { get; init; } = Value;
}
public struct MetaObject<ObjectType> : IEquatable<MetaObject<ObjectType>> where ObjectType : notnull
{
  public ObjectType Value { get; set; }
  public MetaData metaData { get; init; }
  public MetaObject(ObjectType o, params MetaDataEntry[] metas)
  {
    metaData = new(metas);
    Value = o;
  }
  public object? this[string metaTag]
  {
    get => metaData[metaTag];
  }
  public bool Equals(MetaObject<ObjectType> other)
  {
    return Value.Equals(other);
  }
  public override string ToString() => Value.ToString() ?? "";
  public override int GetHashCode() => HashCode.Combine<object,MetaData>(Value,metaData);
  public override bool Equals(object? other)
  {
    if(other == null) return false;
    else if(!other.GetType().Equals(Value.GetType())) return false;
    else return Value.Equals(other);
  }
}