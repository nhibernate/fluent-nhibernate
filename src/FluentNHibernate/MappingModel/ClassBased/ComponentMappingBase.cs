using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public abstract class ComponentMappingBase : ClassMappingBase, IComponentMapping
    {
        private readonly AttributeStore<ComponentMappingBase> attributes;

        protected ComponentMappingBase()
            : this(new AttributeStore())
        {}

        protected ComponentMappingBase(AttributeStore store)
        {
            attributes = new AttributeStore<ComponentMappingBase>(store);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            if (Parent != null)
                visitor.Visit(Parent);

            base.AcceptVisitor(visitor);
        }

        public Type ContainingEntityType { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

        public ParentMapping Parent
        {
            get { return attributes.Get(x => x.Parent); }
            set { attributes.Set(x => x.Parent, value); }
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

        public bool IsSpecified<TResult>(Expression<Func<ComponentMappingBase, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ComponentMappingBase, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ComponentMappingBase, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}