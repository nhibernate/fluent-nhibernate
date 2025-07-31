using System;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class AttributeStore: IEquatable<AttributeStore>
{
    readonly AttributeLayeredValues layeredValues = new();

    public object Get(string property)
    {
        var values = layeredValues[property];

        if (!values.Any())
            return null;

        var topLayer = values.Max(x => x.Key);

        return values[topLayer];
    }

    public void Set(string attribute, int layer, object value)
    {
        layeredValues[attribute][layer] = value;
    }

    public bool IsSpecified(string attribute)
    {
        return layeredValues[attribute].Any();
    }

    public void CopyTo(AttributeStore theirStore)
    {
        layeredValues.CopyTo(theirStore.layeredValues);
    }

    public AttributeStore Clone()
    {
        var clonedStore = new AttributeStore();

        CopyTo(clonedStore);

        return clonedStore;
    }

    public bool Equals(AttributeStore other)
    {
        if (other is null) return false;

        return other.layeredValues.ContentEquals(layeredValues);
    }

    public override bool Equals(object obj)
    {
        var typed = obj as AttributeStore;
        if (typed is null) return false;
        return Equals(typed);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((layeredValues is not null ? layeredValues.GetHashCode() : 0) * 397);
        }
    }

    public void Merge(AttributeStore columnAttributes)
    {
        columnAttributes.layeredValues.CopyTo(layeredValues);
    }
}

public static class AttributeStoreExtensions
{
    public static T GetOrDefault<T>(this AttributeStore store, [CallerMemberName] string attribute = default)
    {
        return (T)(store.Get(attribute) ?? default(T));
    }
}
