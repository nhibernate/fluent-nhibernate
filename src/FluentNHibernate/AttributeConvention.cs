using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate
{
    public class AttributeConvention<T> : IPropertyConvention where T : Attribute
    {
        private readonly Action<T, IProperty> _action;

        public AttributeConvention(Action<T, IProperty> action)
        {
            _action = action;
        }

        public bool CanHandle(IProperty property)
        {
            var att = Attribute.GetCustomAttribute(property.Property, typeof(T), true) as T;

            return att != null;
        }

        public void Process(IProperty property)
        {
            T att = Attribute.GetCustomAttribute(property.Property, typeof (T), true) as T;

            if (att != null)
            {
                _action(att, property);
            }
        }
    }
}