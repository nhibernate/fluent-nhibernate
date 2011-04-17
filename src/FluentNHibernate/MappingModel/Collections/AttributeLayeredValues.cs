using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class AttributeLayeredValues
    {
        readonly Dictionary<string, LayeredValues> inner = new Dictionary<string, LayeredValues>();

        public LayeredValues this[string attribute]
        {
            get
            {
                EnsureValueExists(attribute);

                return inner[attribute];
            }
        }

        void EnsureValueExists(string attribute)
        {
            if (!inner.ContainsKey(attribute))
                inner[attribute] = new LayeredValues();
        }

        public bool ContainsKey(string attribute)
        {
            return inner.ContainsKey(attribute);
        }

        public void CopyTo(AttributeLayeredValues other)
        {
            foreach (var layeredValues in inner)
            {
                foreach (var layeredValue in layeredValues.Value)
                    other[layeredValues.Key][layeredValue.Key] = layeredValue.Value;
            }
        }

        public bool ContentEquals(AttributeLayeredValues other)
        {
            if (other.inner.Keys.Count != inner.Keys.Count)
                return false; // different number of keys
            if (!other.inner.Keys.All(key => inner.Keys.Contains(key)))
                return false; // different keys

            foreach (var theirs in other.inner)
            {
                var ours = inner[theirs.Key];

                if (!theirs.Value.ContentEquals(ours))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is AttributeLayeredValues)
                return Equals((AttributeLayeredValues)obj);
            return base.Equals(obj);
        }

        public bool Equals(AttributeLayeredValues other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.inner.ContentEquals(inner);
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            unchecked
            {
                foreach (var pair in inner)
                {
                    var pairCode = 0;
                
                    pairCode += pair.Key.GetHashCode();
                    pairCode += pair.Value.GetHashCode();

                    hashCode += pairCode ^ 367;
                }
            }

            return hashCode;
        }
    }
}