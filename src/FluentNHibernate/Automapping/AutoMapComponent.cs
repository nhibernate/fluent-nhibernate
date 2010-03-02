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

        public bool MapsProperty(Member property)
        {
            return expressions.IsComponentType(property.PropertyType);
        }

        public void Map(ClassMappingBase classMap, Member property)
        {
            var mapping = new ComponentMapping(ComponentType.Component)
            {
                Name = property.Name,
                Member = property,
                ContainingEntityType = classMap.Type,
                Type = property.PropertyType
            };

            mapper.FlagAsMapped(property.PropertyType);
            mapper.MergeMap(property.PropertyType, mapping, new List<string>());

            classMap.AddComponent(mapping);
        }
    }
}