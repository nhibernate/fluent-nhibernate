using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public class AutoMapComponent : IAutoMapper
    {
        private readonly AutoMappingExpressions expressions;
        private readonly AutoMapper mapper;

        public AutoMapComponent(AutoMappingExpressions expressions, AutoMapper mapper)
        {
            this.expressions = expressions;
            this.mapper = mapper;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            return expressions.IsComponentType(property.PropertyType);
        }

        public void Map(ClassMappingBase classMap, PropertyInfo property)
        {
            var mapping = new ComponentMapping
            {
                Name = property.Name,
                PropertyInfo = property,
                ContainingEntityType = classMap.Type,
                Type = property.PropertyType
            };

            mapper.FlagAsMapped(property.PropertyType);
            mapper.MergeMap(property.PropertyType, mapping, new List<string>());

            classMap.AddComponent(mapping);
        }
    }
}