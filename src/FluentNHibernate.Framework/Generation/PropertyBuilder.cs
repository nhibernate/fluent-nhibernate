using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Framework.Generation;


namespace FluentNHibernate.Framework.Generation
{
    public class PropertyBuilder
    {
        private IPropertyTypeBuilder _builder = new PropertyTypeBuilder<NulloProperty>(t => true);

        public PropertyBuilder()
        {
            Return<DomainProperty>(isSingleClassProperty);
            Return<GenericSimpleProperty>(isGenericAndPrimitive);
            Return<SimpleProperty>(isPrimitive);
        }

        public IProperty Build(PropertyInfo property)
        {
            return _builder.Build(property);
        }

        public void Return<T>(Func<PropertyInfo, bool> matches) where T : IProperty, new()
        {
            PropertyTypeBuilder<T> builder = new PropertyTypeBuilder<T>(matches);
            builder.Next = _builder;
            _builder = builder;
        }

        private static bool isGenericAndPrimitive(PropertyInfo property)
        {
            return
                property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static bool isPrimitive(PropertyInfo property)
        {
            return property.PropertyType.IsPrimitive || property.PropertyType.Equals(typeof(string)) || property.PropertyType.Equals(typeof(DateTime)) || property.PropertyType.IsEnum;
        }

        private static bool isSingleClassProperty(PropertyInfo property)
        {
            if (property.PropertyType.IsGenericType)
            {
                Type definition = property.PropertyType.GetGenericTypeDefinition();
                if (typeof(IList<>).IsAssignableFrom(definition))
                {
                    return false;
                }
            }

            return !typeof(ICollection).IsAssignableFrom(property.PropertyType) && !typeof(IList).IsAssignableFrom(property.PropertyType);
        }

        public interface IPropertyTypeBuilder
        {
            IProperty Build(PropertyInfo property);
        }

        public class PropertyTypeBuilder<T> : IPropertyTypeBuilder where T : IProperty, new()
        {
            private Func<PropertyInfo, bool> _matches;


            public PropertyTypeBuilder(Func<PropertyInfo, bool> matches)
            {
                _matches = matches;
            }

            public IProperty Build(PropertyInfo property)
            {
                if (_matches(property))
                {
                    IProperty prop = new T();
                    prop.Property = property;

                    return prop;
                }

                return Next.Build(property);
            }

            public IPropertyTypeBuilder Next { get; set; }
        }
    }
}