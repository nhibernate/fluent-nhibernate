using System;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class DynamicComponentPart<T> : ComponentPartBase<T>, IDynamicComponent
    {
        public DynamicComponentPart(PropertyInfo property)
            : this(new DynamicComponentMapping(), property.Name)
        {}

        public DynamicComponentPart(DynamicComponentMapping mapping, string propertyName)
            : base(mapping, propertyName)
        {
            this.mapping = mapping;
        }

        protected override ComponentPart<TComponent> Component<TComponent>(PropertyInfo property, Action<ComponentPart<TComponent>> action)
        {
            var part = new ComponentPart<TComponent>(property);
            action(part);
            components.Add(part);

            return part;
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