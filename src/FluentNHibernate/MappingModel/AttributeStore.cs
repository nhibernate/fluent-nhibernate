using System;
using System.Linq;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class AttributeStore
    {
        readonly AttributeLayeredValues layeredValues;

        public AttributeStore()
        {
            layeredValues = new AttributeLayeredValues();
        }

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
            if (other == null) return false;

            return other.layeredValues.ContentEquals(layeredValues);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(AttributeStore)) return false;
            return Equals((AttributeStore)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((layeredValues != null ? layeredValues.GetHashCode() : 0) * 397);
            }
        }

        public void Merge(AttributeStore columnAttributes)
        {
            columnAttributes.layeredValues.CopyTo(layeredValues);
        }
    }

    public static class AttributeStoreExtensions
    {
        public static T GetOrDefault<T>(this AttributeStore store, string attribute)
        {
            return (T)(store.Get(attribute) ?? default(T));
        }
    }
}