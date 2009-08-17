using System;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Automapping
{
    public class AutoMapIdentity : IAutoMapper
    {
        private readonly AutoMappingExpressions expressions;

        public AutoMapIdentity(AutoMappingExpressions conventions)
        {
            this.expressions = conventions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            return expressions.FindIdentity(property);
        }

        public void Map(ClassMappingBase classMap, PropertyInfo property)
        {
            if (!(classMap is ClassMapping)) return;

            var idMapping = new IdMapping { ContainingEntityType = classMap.Type };
            idMapping.AddDefaultColumn(new ColumnMapping() { Name = property.Name });
            idMapping.Name = property.Name;
            idMapping.Type = new TypeReference(property.PropertyType);
            idMapping.PropertyInfo = property;
            idMapping.SetDefaultValue(x => x.Generator, GetDefaultGenerator(property));
            ((ClassMapping)classMap).Id = idMapping;        
        }

        private GeneratorMapping GetDefaultGenerator(PropertyInfo property)
        {
            var generatorMapping = new GeneratorMapping();
            var defaultGenerator = new GeneratorBuilder(generatorMapping, property.PropertyType);

            if (property.PropertyType == typeof(Guid))
                defaultGenerator.GuidComb();
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long))
                defaultGenerator.Identity();
            else
                defaultGenerator.Assigned();

            return generatorMapping;
        }
    }
}