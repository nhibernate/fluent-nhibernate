using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{
    public class AutoSimpleTypeCollection : IAutoMapper
    {
        readonly AutoMappingExpressions expressions;
        readonly AutoKeyMapper keys;
        readonly AutoCollectionCreator collections;

        public AutoSimpleTypeCollection(AutoMappingExpressions expressions)
        {
            this.expressions = expressions;
            keys = new AutoKeyMapper(expressions);
            collections = new AutoCollectionCreator();
        }

        public bool MapsProperty(Member property)
        {
            if (!property.PropertyType.IsGenericType)
                return false;

            var childType = property.PropertyType.GetGenericArguments()[0];

            return property.CanWrite &&
                property.PropertyType.ClosesInterface(typeof(IEnumerable<>)) &&
                    (childType.IsPrimitive || childType.In(typeof(string), typeof(DateTime)));
        }

        public void Map(ClassMappingBase classMap, Member property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            var mapping = collections.CreateCollectionMapping(property.PropertyType);

            mapping.ContainingEntityType = classMap.Type;
            mapping.Member = property;
            mapping.SetDefaultValue(x => x.Name, property.Name);

            keys.SetKey(property, classMap, mapping);
            SetElement(property, classMap, mapping);
        
            classMap.AddCollection(mapping);
        }

        private void SetElement(Member property, ClassMappingBase classMap, ICollectionMapping mapping)
        {
            var element = new ElementMapping
            {
                ContainingEntityType = classMap.Type,
                Type = new TypeReference(property.PropertyType.GetGenericArguments()[0])
            };

            element.AddDefaultColumn(new ColumnMapping { Name = expressions.SimpleTypeCollectionValueColumn(property) });
            mapping.SetDefaultValue(x => x.Element, element);
        }
    }
}