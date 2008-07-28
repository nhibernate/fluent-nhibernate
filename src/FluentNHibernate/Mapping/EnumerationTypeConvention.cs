using System;

namespace FluentNHibernate.Mapping
{
    public class EnumerationTypeConvention : ITypeConvention
    {
        public bool CanHandle(Type type)
        {
            return type.IsEnum;
        }

        public void AlterMap(IProperty property)
        {
            Type mapperType = typeof(GenericEnumMapper<>).MakeGenericType(property.PropertyType);
            property.SetAttributeOnPropertyElement("type", mapperType.AssemblyQualifiedName);

            property.SetAttributeOnColumnElement("sql-type", "string");
            property.SetAttributeOnColumnElement("length", "50");
        }
    }
}