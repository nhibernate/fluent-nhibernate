using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class AttributeLayeredValues
    {
        readonly Dictionary<string, LayeredValues> inner = new Dictionary<string, LayeredValues>();

        public AttributeLayeredValues()
        {}

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
    }
}