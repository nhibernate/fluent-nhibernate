using System;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public class AutoMapManyToOne : IAutoMapper
    {
        private readonly Func<PropertyInfo, bool> findPropertyconvention = p => (
            p.PropertyType.Namespace != "System" && // ignore clr types (won't be entities)
            p.PropertyType.Namespace != "System.Collections.Generic" &&
            p.PropertyType.Namespace != "Iesi.Collections.Generic" &&
	    !p.PropertyType.IsEnum);

        public bool MapsProperty(PropertyInfo property)
        {
            if (property.CanWrite)
                return findPropertyconvention(property);

            return false;
        }

        public void Map(ClassMappingBase classMap, PropertyInfo property)
        {
            var manyToOne = CreateMapping(property);
            classMap.AddReference(manyToOne);
        }

        private ManyToOneMapping CreateMapping(PropertyInfo property)
        {
            var mapping = new ManyToOneMapping { PropertyInfo = property };

            mapping.SetDefaultValue(x => x.Name, property.Name);
            mapping.AddDefaultColumn(new ColumnMapping { Name = property.Name + "_id" });

            return mapping;
        }
    }
}
