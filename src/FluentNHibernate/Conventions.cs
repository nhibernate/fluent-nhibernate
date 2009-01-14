using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate
{
    public class Conventions
    {
        private readonly List<ITypeConvention> _typeConventions = new List<ITypeConvention>();
        private readonly List<IPropertyConvention> _propertyConventions = new List<IPropertyConvention>();

        public int DefaultStringLength { get; set; }

        public Conventions()
        {
            DefaultStringLength = 100;
            DefaultLazyLoad = true;

            AddTypeConvention(new IgnoreNullableTypeConvention());
            AddTypeConvention(new EnumerationTypeConvention());
        }

        public Func<Type, Type, Type> GetParentSideForManyToMany = (one, two) =>
            one.FullName.CompareTo(two.FullName) < 0 ? one : two;

        public Func<Type, string> GetTableName = type => String.Format("`{0}`", type.Name);
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

        public void AddPropertyConvention(IPropertyConvention convention)
        {
            _propertyConventions.Add(convention);
        }

        public ITypeConvention FindConvention(Type propertyType)
        {
            var find = _typeConventions.Find(c => c.CanHandle(propertyType));
            return find ?? new DefaultConvention();
        }

        public void AlterId(IIdentityPart part)
        {
            IdConvention(part);
        }

        public void AlterMap(IProperty property)
        {
            if (property.PropertyType == typeof(string))
            {
                property.SetAttribute("length", DefaultStringLength.ToString());
            }

            ITypeConvention convention = FindConvention(property.PropertyType);
            convention.AlterMap(property);

            _propertyConventions.ForEach(c =>
            {
                if (c.CanHandle(property))
                    c.Process(property);
            });
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

        public Action<IIdentityPart> IdConvention = id => {};

        public Action<IOneToManyPart> OneToManyConvention = m => {};

        public Action<IMappingPart> ManyToOneConvention = m => {};

        public Action<IMappingPart> JoinConvention = m => {};

        public Action<IMappingPart> OneToOneConvention = m => { };

        public Func<CachePart, CachePart> DefaultCache = cache => null;

        public Func<PropertyInfo, string> GetVersionColumnName;

        public bool DefaultLazyLoad = false;

        public Func<Type, string> GetPrimaryKeyNameFromType;

        public Func<Type, bool> IsBaseType = b => b == typeof(object);

        public string CalculatePrimaryKey(Type classType, PropertyInfo _property)
        {
            if (GetPrimaryKeyNameFromType != null)
                return GetPrimaryKeyNameFromType(classType);

            return GetPrimaryKeyName(_property);
        }
    }
}
