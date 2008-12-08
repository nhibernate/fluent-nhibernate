using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;

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
        }

        public Func<Type, string> GetTableName = prop => String.Format("[{0}]", prop.Name);
        public Func<PropertyInfo, string> GetPrimaryKeyName = prop => prop.Name;
        public Func<PropertyInfo, string> GetForeignKeyName = prop => prop.Name + "_id";
        public Func<Type, string> GetForeignKeyNameOfParent = type => type.Name + "_id";
        public Func<MethodInfo, string> GetReadOnlyCollectionBackingFieldName = method => method.Name.Replace("Get", "");

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
                property.SetAttribute("length", DefaultStringLength.ToString());
            }

            ITypeConvention convention = FindConvention(property.PropertyType);
            convention.AlterMap(property);

            _propertyConventions.ForEach(c => c.Process(property));
        }

        public void AlterManyToOneMap(IManyToOnePart part)
        {
            ManyToOneConvention.Invoke(part);
        }

        public void AlterOneToOneMap(IOneToOnePart part)
        {
            OneToOneConvention.Invoke(part);
        }

        public void AlterOneToManyMap(IOneToManyPart oneToManyPart)
        {
            OneToManyConvention(oneToManyPart);
        }

        public void AlterJoin(IMappingPart part)
        {
            JoinConvention.Invoke(part);
        }

        public void ForAttribute<T>(Action<T, IProperty> action) where T : Attribute
        {
            AttributeConvention<T> convention = new AttributeConvention<T>(action);
            _propertyConventions.Add(convention);
        }


        public Func<PropertyInfo,bool> FindIdentity = p => p.Name == "Id";

        public Action<IOneToManyPart> OneToManyConvention = m => {};
        public Action<IMappingPart> ManyToOneConvention = m => {};
        public Action<IMappingPart> JoinConvention = m => {};
        public Action<IMappingPart> OneToOneConvention = m => { };

        public Func<PropertyInfo, string> GetVersionColumnName;

        public bool DefaultLazyLoad;
    }
}
