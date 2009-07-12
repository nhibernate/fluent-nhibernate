using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.AutoMap
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

        public void Map(ClassMapping classMap, PropertyInfo property)
        {
            var idMapping = new IdMapping { ContainingEntityType = classMap.Type };
            idMapping.AddDefaultColumn(new ColumnMapping() { Name = property.Name });
            idMapping.Name = property.Name;
            idMapping.Type = new TypeReference(property.PropertyType);
            idMapping.PropertyInfo = property;
            idMapping.Generator= new GeneratorMapping { Class = "identity" };
            classMap.Id = idMapping;        
        }

        public void Map(JoinedSubclassMapping classMap, PropertyInfo property)
        {
        }

        public void Map(SubclassMapping classMap, PropertyInfo property)
        {
        }
    }
}