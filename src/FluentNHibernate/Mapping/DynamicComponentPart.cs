using System;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class DynamicComponentPart<T> : ComponentPartBase<T>, IDynamicComponent
    {
        public DynamicComponentPart(PropertyInfo property)
            : this(new DynamicComponentMapping(), property)
        {}

        public DynamicComponentPart(DynamicComponentMapping mapping, PropertyInfo property) : base(mapping, property)
        {}

        protected override ComponentPart<TComponent> Component<TComponent>(PropertyInfo property, Action<ComponentPart<TComponent>> action)
        {
            var part = new ComponentPart<TComponent>(property);
            action(part);
            components.Add(part);

            return part;
        }
    }
}