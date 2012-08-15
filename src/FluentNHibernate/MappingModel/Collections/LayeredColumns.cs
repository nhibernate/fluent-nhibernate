using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class LayeredColumns
    {
        readonly LayeredValues layeredValues = new LayeredValues();

        public IEnumerable<ColumnMapping> Columns
        {
            get
            {
                if (!layeredValues.Any())
                    yield break;

                var maxLayer = layeredValues.Keys.Max();
                var values = ((Dictionary<int,ColumnMapping>)layeredValues[maxLayer]);
                foreach (var value in values.Values)
                {
                    yield return value;
                }
            }
        }

        public void AddColumn(int layer, ColumnMapping mapping)
        {           
            if (!layeredValues.ContainsKey(layer))
                layeredValues[layer] = new Dictionary<int, ColumnMapping>();
            ((Dictionary<int,ColumnMapping>)layeredValues[layer]).Add(mapping.GetHashCode(), mapping);
        }

        public void MakeColumnsEmpty(int layer)
        {
            layeredValues[layer] = new Dictionary<int, ColumnMapping>();
        }

        public bool ContentEquals(LayeredColumns columns)
        {
            return layeredValues.ContentEquals(columns.layeredValues);
        }

        [Serializable]
        class ColumnMappingComparer : IEqualityComparer<ColumnMapping>
        {
            public bool Equals(ColumnMapping x, ColumnMapping y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(ColumnMapping obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}