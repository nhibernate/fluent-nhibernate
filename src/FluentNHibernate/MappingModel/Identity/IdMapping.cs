using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Identity
{
    [Serializable]
    public class IdMapping : ColumnBasedMappingBase, IIdentityMapping
    {
        public IdMapping()
            : this(new AttributeStore())
        {}

        public IdMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {}

        public Member Member { get; set; }

        public GeneratorMapping Generator
        {
            get { return attributes.GetOrDefault<GeneratorMapping>("Generator"); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessId(this);

            foreach (var column in Columns)
                visitor.Visit(column);

            if (Generator != null)
                visitor.Visit(Generator);
        }

        public string Name
        {
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public string Access
        {
            get { return attributes.GetOrDefault<string>("Access"); }
        }

        public TypeReference Type
        {
            get { return attributes.GetOrDefault<TypeReference>("Type"); }
        }

        public string UnsavedValue
        {
            get { return attributes.GetOrDefault<string>("UnsavedValue"); }
        }

        public Type ContainingEntityType { get; set; }

        public void Set(Expression<Func<IdMapping, object>> expression, int layer, object value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        protected override void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }

        public bool Equals(IdMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.Member, Member) && Equals(other.ContainingEntityType, ContainingEntityType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as IdMapping);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result * 397) ^ (Member != null ? Member.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                return result;
            }
        }

        public override bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }
    }
}