using System;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapComponent : IAutoMapper
    {
        private readonly AutoMappingExpressions expressions;

        public AutoMapComponent(AutoMappingExpressions expressions)
        {
            this.expressions = expressions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            return expressions.IsComponentType(property.PropertyType);
        }

        public void Map(ClassMapping classMap, PropertyInfo property)
        {
            MapComponent(classMap, property);
        }

        public void Map(JoinedSubclassMapping classMap, PropertyInfo property)
        {
            MapComponent(classMap, property);
        }

        public void Map(SubclassMapping classMap, PropertyInfo property)
        {
            MapComponent(classMap, property);
        }

        private void MapComponent(ClassMappingBase classMapping, PropertyInfo property)
        {
            var mapping = new ComponentMapping
            {
                Name = property.Name
            };

            var columnNamePrefix = expressions.GetComponentColumnPrefix(property);

            foreach (var componentProperty in property.PropertyType.GetProperties())
            {
                if (componentProperty.PropertyType.IsEnum || componentProperty.GetIndexParameters().Length != 0) continue;

                mapping.AddProperty(GetPropertyMapping(property.PropertyType, componentProperty, columnNamePrefix));
            }

            classMapping.AddComponent(mapping);
        }

        private PropertyMapping GetPropertyMapping(Type type, PropertyInfo property, string columnNamePrefix)
        {
            var mapping = new PropertyMapping
            {
                ContainingEntityType = type,
                PropertyInfo = property
            };

            mapping.AddColumn(new ColumnMapping { Name = columnNamePrefix + mapping.PropertyInfo.Name });

            if (!mapping.Attributes.IsSpecified(x => x.Name))
                mapping.Name = mapping.PropertyInfo.Name;

            if (!mapping.Attributes.IsSpecified(x => x.Type))
                mapping.Attributes.SetDefault(x => x.Type, new TypeReference(mapping.PropertyInfo.PropertyType));

            return mapping;
        }
    }
}