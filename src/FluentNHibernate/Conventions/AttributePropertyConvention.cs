using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    public abstract class AttributePropertyConvention<T> : IPropertyConvention
        where T : Attribute
    {
        public bool Accept(IProperty target)
        {
            var attribute = Attribute.GetCustomAttribute(target.Property, typeof(T)) as T;

            return attribute != null;
        }

        public void Apply(IProperty target, ConventionOverrides overrides)
        {
            var attribute = Attribute.GetCustomAttribute(target.Property, typeof(T)) as T;
            
            Apply(attribute, target, overrides);
        }

        protected abstract void Apply(T attribute, IProperty target, ConventionOverrides overrides);
    }
}