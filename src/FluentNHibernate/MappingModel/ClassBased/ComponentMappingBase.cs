using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public abstract class ComponentMappingBase : ClassMappingBase
    {
        private readonly AttributeStore<ComponentMappingBase> attributes;

        protected ComponentMappingBase()
            : this(new AttributeStore())
        {}

        protected ComponentMappingBase(AttributeStore store)
        {
            attributes = new AttributeStore<ComponentMappingBase>(store);
            attributes.SetDefault(x => x.Unique, false);
            attributes.SetDefault(x => x.Update, true);
            attributes.SetDefault(x => x.Insert, true);
            attributes.SetDefault(x => x.OptimisticLock, true);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            if (Parent != null)
                visitor.Visit(Parent);

            base.AcceptVisitor(visitor);
        }

        public Type ContainingEntityType { get; set; }
        public Member Member { get; set; }

        public ParentMapping Parent
        {
            get { return attributes.Get(x => x.Parent); }
            set { attributes.Set(x => x.Parent, value); }
        }

        public bool Unique
        {
            get { return attributes.Get(x => x.Unique); }
            set { attributes.Set(x => x.Unique, value); }
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

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public bool OptimisticLock
        {
            get { return attributes.Get(x => x.OptimisticLock); }
            set { attributes.Set(x => x.OptimisticLock, value); }
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public abstract bool HasValue(string property);

        public bool HasValue<TResult>(Expression<Func<ComponentMappingBase, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ComponentMappingBase, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        public bool Equals(ComponentMappingBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) &&
                Equals(other.attributes, attributes) &&
                Equals(other.ContainingEntityType, ContainingEntityType) &&
                Equals(other.Member, Member);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as ComponentMappingBase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                result = (result * 397) ^ (Member != null ? Member.GetHashCode() : 0);
                return result;
            }
        }
    }
}