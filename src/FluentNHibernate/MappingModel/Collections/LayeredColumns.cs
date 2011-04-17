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
                var values = ((HashSet<ColumnMapping>)layeredValues[maxLayer]);

                foreach (var value in values)
                {
                    yield return value;
                }
            }
        }

        public void AddColumn(int layer, ColumnMapping mapping)
        {
            if (!layeredValues.ContainsKey(layer))
                layeredValues[layer] = new HashSet<ColumnMapping>(new ColumnMappingComparer());

            ((HashSet<ColumnMapping>)layeredValues[layer]).Add(mapping);
        }

        public void MakeColumnsEmpty(int layer)
        {
            layeredValues[layer] = new HashSet<ColumnMapping>();
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