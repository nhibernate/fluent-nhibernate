using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class ColumnMapping : MappingBase
    {
        readonly AttributeStore attributes;

        public ColumnMapping()
            : this(new AttributeStore())
        {}

        public ColumnMapping(string defaultColumnName)
            : this()
        {
            Set(x => x.Name, Layer.Defaults, defaultColumnName);
        }

        public ColumnMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessColumn(this);
        }

        public Member Member { get; set; }

        public string Name
        {
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public int Length
        {
            get { return attributes.GetOrDefault<int>("Length"); }
        }

        public bool NotNull
        {
            get { return attributes.GetOrDefault<bool>("NotNull"); }
        }

        public bool Unique
        {
            get { return attributes.GetOrDefault<bool>("Unique"); }
        }

        public string UniqueKey
        {
            get { return attributes.GetOrDefault<string>("UniqueKey"); }
        }

        public string SqlType
        {
            get { return attributes.GetOrDefault<string>("SqlType"); }
        }

        public string Index
        {
            get { return attributes.GetOrDefault<string>("Index"); }
        }

        public string Check
        {
            get { return attributes.GetOrDefault<string>("Check"); }
        }

        public int Precision
        {
            get { return attributes.GetOrDefault<int>("Precision"); }
        }

        public int Scale
        {
            get { return attributes.GetOrDefault<int>("Scale"); }
        }

        public string Default
        {
            get { return attributes.GetOrDefault<string>("Default"); }
        }

        public ColumnMapping Clone()
        {
            return new ColumnMapping(attributes.Clone());
        }

        public bool Equals(ColumnMapping other)
        {
            return Equals(other.attributes, attributes) && Equals(other.Member, Member);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(ColumnMapping)) return false;
            return Equals((ColumnMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((attributes != null ? attributes.GetHashCode() : 0) * 397) ^ (Member != null ? Member.GetHashCode() : 0);
            }
        }

        public void Set<T>(Expression<Func<ColumnMapping, T>> expression, int layer, T value)
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

        public void MergeAttributes(AttributeStore columnAttributes)
        {
            attributes.Merge(columnAttributes);
        }
    }
}