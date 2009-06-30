using System;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.AutoMap
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

        public void Map(ClassMapping classMap, PropertyInfo property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            var manyToOne = new ManyToOneMapping {Name = property.Name, PropertyInfo = property};
            classMap.AddReference(manyToOne);
        }

        public void Map(JoinedSubclassMapping classMap, PropertyInfo property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            var manyToOne = new ManyToOneMapping { Name = property.Name, PropertyInfo = property };
            classMap.AddReference(manyToOne);
        }

        public void Map(SubclassMapping classMap, PropertyInfo property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            var manyToOne = new ManyToOneMapping { Name = property.Name, PropertyInfo = property };
            classMap.AddReference(manyToOne);
        }
    }
}
