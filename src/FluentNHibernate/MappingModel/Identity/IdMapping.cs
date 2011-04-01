using System;
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
            get { return attributes.Get<GeneratorMapping>("Generator"); }
            set { attributes.Set("Generator", value); }
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
            get { return attributes.Get("Name"); }
            set { attributes.Set("Name", value); }
        }

        public string Access
        {
            get { return attributes.Get("Access"); }
            set { attributes.Set("Access", value); }
        }

        public TypeReference Type
        {
            get { return attributes.Get<TypeReference>("Type"); }
            set { attributes.Set("Type", value); }
        }

        public string UnsavedValue
        {
            get { return attributes.Get("UnsavedValue"); }
            set { attributes.Set("UnsavedValue", value); }
        }

        public Type ContainingEntityType { get; set; }

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
    }
}