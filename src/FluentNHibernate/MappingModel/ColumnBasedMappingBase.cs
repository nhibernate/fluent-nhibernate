using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public abstract class ColumnBasedMappingBase : MappingBase, IHasColumnMappings
    {
        readonly LayeredColumns columns = new LayeredColumns();
        protected readonly AttributeStore attributes;

        protected ColumnBasedMappingBase(AttributeStore underlyingStore)
        {
            attributes = underlyingStore.Clone();
        }

        public IEnumerable<ColumnMapping> Columns
        {
            get { return columns.Columns; }
        }

        public void AddColumn(int layer, ColumnMapping mapping)
        {
            columns.AddColumn(layer, mapping);
        }

        public void MakeColumnsEmpty(int layer)
        {
            columns.MakeColumnsEmpty(layer);
        }

        public bool Equals(ColumnBasedMappingBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.columns.ContentEquals(columns) &&
                Equals(other.attributes, attributes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ColumnBasedMappingBase)) return false;
            return Equals((ColumnBasedMappingBase)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((columns != null ? columns.GetHashCode() : 0) * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
            }
        }
    }
}