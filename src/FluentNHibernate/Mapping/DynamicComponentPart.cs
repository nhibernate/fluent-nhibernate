using System;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class DynamicComponentPart<T> : ComponentPartBase<T>, IDynamicComponent
    {
        public DynamicComponentPart(Type entity, PropertyInfo property)
            : this(new DynamicComponentMapping { ContainingEntityType = entity }, property.Name)
        {}

        private DynamicComponentPart(DynamicComponentMapping mapping, string propertyName)
            : base(mapping, propertyName)
        {
            this.mapping = mapping;
        }

        public DynamicComponentPart<T> Not
        {
            get
            {
                var forceCall = ((IComponentBase)this).Not;
                return this;
            }
        }

        public DynamicComponentPart<T> ReadOnly()
        {
            ((IComponentBase)this).ReadOnly();
            return this;
        }

        public DynamicComponentPart<T> Insert()
        {
            ((IComponentBase)this).Insert();
            return this;
        }

        public DynamicComponentPart<T> Update()
        {
            ((IComponentBase)this).Update();
            return this;
        }
    }
}