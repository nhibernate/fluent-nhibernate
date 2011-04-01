using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class AnyMapping : MappingBase
    {
        private readonly AttributeStore<AnyMapping> attributes;
        private readonly IDefaultableList<ColumnMapping> typeColumns = new DefaultableList<ColumnMapping>();
        private readonly IDefaultableList<ColumnMapping> identifierColumns = new DefaultableList<ColumnMapping>();
        private readonly IList<MetaValueMapping> metaValues = new List<MetaValueMapping>();

        public AnyMapping()
            : this(new AttributeStore())
        {}

        public AnyMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<AnyMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessAny(this);

            foreach (var metaValue in metaValues)
                visitor.Visit(metaValue);

            foreach (var column in typeColumns)
                visitor.Visit(column);

            foreach (var column in identifierColumns)
                visitor.Visit(column);
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public string IdType
        {
            get { return attributes.Get(x => x.IdType); }
            set { attributes.Set(x => x.IdType, value); }
        }

        public TypeReference MetaType
        {
            get { return attributes.Get(x => x.MetaType); }
            set { attributes.Set(x => x.MetaType, value); }
        }

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public bool Insert
        {
            get { return attributes.Get(x => x.Insert); }
            set { attributes.Set(x => x.Insert, value); }
        }

        public bool Update
        {
            get { return attributes.Get(x => x.Update); }
            set { attributes.Set(x => x.Update, value); }
        }

        public string Cascade
        {
            get { return attributes.Get(x => x.Cascade); }
            set { attributes.Set(x => x.Cascade, value); }
        }

        public bool Lazy
        {
            get { return attributes.Get(x => x.Lazy); }
            set { attributes.Set(x => x.Lazy, value); }
        }

        public bool OptimisticLock
        {
            get { return attributes.Get(x => x.OptimisticLock); }
            set { attributes.Set(x => x.OptimisticLock, value); }
        }

        public IDefaultableEnumerable<ColumnMapping> TypeColumns
        {
            get { return typeColumns; }
        }

        public IDefaultableEnumerable<ColumnMapping> IdentifierColumns
        {
            get { return identifierColumns; }
        }

        public IEnumerable<MetaValueMapping> MetaValues
        {
            get { return metaValues; }
        }

        public Type ContainingEntityType { get; set; }

        public void AddTypeDefaultColumn(ColumnMapping column)
        {
            typeColumns.AddDefault(column);
        }

        public void AddTypeColumn(ColumnMapping column)
        {
            typeColumns.Add(column);
        }

        public void AddIdentifierDefaultColumn(ColumnMapping column)
        {
            identifierColumns.AddDefault(column);
        }

        public void AddIdentifierColumn(ColumnMapping column)
        {
            identifierColumns.Add(column);
        }

        public void AddMetaValue(MetaValueMapping metaValue)
        {
            metaValues.Add(metaValue);
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<AnyMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<AnyMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        public bool Equals(AnyMapping other)
        {
            return Equals(other.attributes, attributes) &&
                other.typeColumns.ContentEquals(typeColumns) &&
                other.identifierColumns.ContentEquals(identifierColumns) &&
                other.metaValues.ContentEquals(metaValues) &&
                Equals(other.ContainingEntityType, ContainingEntityType);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(AnyMapping)) return false;
            return Equals((AnyMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (attributes != null ? attributes.GetHashCode() : 0);
                result = (result * 397) ^ (typeColumns != null ? typeColumns.GetHashCode() : 0);
                result = (result * 397) ^ (identifierColumns != null ? identifierColumns.GetHashCode() : 0);
                result = (result * 397) ^ (metaValues != null ? metaValues.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                return result;
            }
        }
    }
}