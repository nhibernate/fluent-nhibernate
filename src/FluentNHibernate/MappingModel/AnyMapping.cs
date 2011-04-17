using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class AnyMapping : MappingBase
    {
        readonly AttributeStore attributes;
        readonly IDefaultableList<ColumnMapping> typeColumns = new DefaultableList<ColumnMapping>();
        readonly IDefaultableList<ColumnMapping> identifierColumns = new DefaultableList<ColumnMapping>();
        readonly IList<MetaValueMapping> metaValues = new List<MetaValueMapping>();

        public AnyMapping()
            : this(new AttributeStore())
        {}

        public AnyMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
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
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public string IdType
        {
            get { return attributes.GetOrDefault<string>("IdType"); }
        }

        public TypeReference MetaType
        {
            get { return attributes.GetOrDefault<TypeReference>("MetaType"); }
        }

        public string Access
        {
            get { return attributes.GetOrDefault<string>("Access"); }
        }

        public bool Insert
        {
            get { return attributes.GetOrDefault<bool>("Insert"); }
        }

        public bool Update
        {
            get { return attributes.GetOrDefault<bool>("Update"); }
        }

        public string Cascade
        {
            get { return attributes.GetOrDefault<string>("Cascade"); }
        }

        public bool Lazy
        {
            get { return attributes.GetOrDefault<bool>("Lazy"); }
        }

        public bool OptimisticLock
        {
            get { return attributes.GetOrDefault<bool>("OptimisticLock"); }
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

        public void Set<T>(Expression<Func<AnyMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        protected override void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }

        public override bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }
    }
}