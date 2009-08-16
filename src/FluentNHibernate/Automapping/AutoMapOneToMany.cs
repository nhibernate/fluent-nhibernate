using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Automapping
{
    public class AutoMapOneToMany : IAutoMapper
    {
        private readonly AutoMappingExpressions expressions;
        private readonly Func<PropertyInfo, bool> findPropertyconvention = 
            p => (p.PropertyType.Namespace == "System.Collections.Generic" ||
                  p.PropertyType.Namespace == "Iesi.Collections.Generic");

        public AutoMapOneToMany(AutoMappingExpressions expressions)
        {
            this.expressions = expressions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            if (property.CanWrite)
                return findPropertyconvention(property);

            return false;
        }

        public void Map(ClassMappingBase classMap, PropertyInfo property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            var mapping = new BagMapping();

            mapping.MemberInfo = property;
            mapping.SetDefaultValue(x => x.Name, property.Name);

            SetRelationship(property, classMap, mapping);
            SetKey(mapping, classMap, property);

            classMap.AddCollection(mapping);        
        }

        private void SetRelationship(PropertyInfo property, ClassMappingBase classMap, BagMapping mapping)
        {
            var relationship = new OneToManyMapping
            {
                Class = new TypeReference(property.PropertyType.GetGenericArguments()[0]),
                ContainingEntityType = classMap.Type
            };

            mapping.SetDefaultValue(x => x.Relationship, relationship);
        }

        private void SetKey(BagMapping mapping, ClassMappingBase classMap, PropertyInfo property)
        {
            var columnName = property.DeclaringType.Name + "_Id";

            if (classMap is ComponentMapping)
                columnName = expressions.GetComponentColumnPrefix(((ComponentMapping)classMap).PropertyInfo) + columnName;

            var key = new KeyMapping();

            key.AddDefaultColumn(new ColumnMapping { Name = columnName});

            mapping.SetDefaultValue(x => x.Key, key);
        }
    }
}
