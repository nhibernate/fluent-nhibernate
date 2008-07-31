using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;
using ShadeTree.Core.Validation;
using ShadeTree.Validation;

namespace FluentNHibernate
{
    public interface IPropertyConvention
    {
        void Process(IProperty property);
    }

    public class AttributeConvention<T> : IPropertyConvention where T : Attribute
    {
        private Action<T, IProperty> _action;

        public AttributeConvention(Action<T, IProperty> action)
        {
            _action = action;
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

    public class Conventions
    {
        private readonly List<ITypeConvention> _typeConventions = new List<ITypeConvention>();
        private List<IPropertyConvention> _propertyConventions = new List<IPropertyConvention>();

        public int DefaultStringLength { get; set; }

        public Conventions()
        {
            DefaultStringLength = 100;

            AddTypeConvention(new IgnoreNullableTypeConvention());
            AddTypeConvention(new EnumerationTypeConvention());

            ForAttribute<RequiredAttribute>((att, prop) =>
            {
                if (prop.ParentIsRequired)
                {
                    prop.SetAttributeOnPropertyElement("not-null", "true");
                }
            });

            ForAttribute<MaximumStringLengthAttribute>((att, prop) => prop.SetAttributeOnPropertyElement("length", att.Length.ToString()));
            ForAttribute<UniqueAttribute>((att, prop) => prop.SetAttributeOnColumnElement("unique", "true"));
        }

        public Func<PropertyInfo, string> GetForeignKeyName = prop => prop.Name + "_id";
        public Func<Type, string> GetForeignKeyNameOfParent = type => type.Name + "_id";

        public Func<Type, Type, string> GetManyToManyTableName =
            (child, parent) => child.Name + "To" + parent.Name;

        public void AddTypeConvention(ITypeConvention convention)
        {
            _typeConventions.Add(convention);
        }

        public ITypeConvention FindConvention(Type propertyType)
        {
            var find = _typeConventions.Find(c => c.CanHandle(propertyType));
            return find ?? new DefaultConvention();
        }

        public void AlterMap(IProperty property)
        {
            if (property.PropertyType == typeof(string))
            {
                property.SetAttributeOnPropertyElement("length", DefaultStringLength.ToString());
            }

            ITypeConvention convention = FindConvention(property.PropertyType);
            convention.AlterMap(property);

            _propertyConventions.ForEach(c => c.Process(property));
        }

        public void ForAttribute<T>(Action<T, IProperty> action) where T : Attribute
        {
            AttributeConvention<T> convention = new AttributeConvention<T>(action);
            _propertyConventions.Add(convention);
        }
    }
}