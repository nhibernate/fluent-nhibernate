using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapOneToMany : IAutoMapper
    {
        private readonly Func<PropertyInfo, bool> findPropertyconvention = 
            p => (p.PropertyType.Namespace == "System.Collections.Generic" ||
                  p.PropertyType.Namespace == "Iesi.Collections.Generic");


        public bool MapsProperty(PropertyInfo property)
        {
            if (property.CanWrite)
                return findPropertyconvention(property);

            return false;
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
//            var classMapType = typeof(ClassMap<T>);
//            var hasManyMethod = classMapType.GetMethod("HasMany", new[] { typeof(Expression<Func<T, object>>) });
//            var listType = property.PropertyType.GetGenericArguments()[0];
//            var genericHasManyMethod = hasManyMethod.MakeGenericMethod(listType);
//            genericHasManyMethod.Invoke(classMap, new object[] { ExpressionBuilder.Create<T>(property) });
        }

        public void Map(ClassMapping classMap, PropertyInfo property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            var setMapping = new BagMapping() {Name = property.Name};
            setMapping.Relationship = new OneToManyMapping() { Class = new TypeReference(property.PropertyType.GetGenericArguments()[0]) };
            setMapping.Key = new KeyMapping();
            setMapping.Key.AddColumn(new ColumnMapping() { Name = property.DeclaringType.Name + "_Id"});
            classMap.AddCollection(setMapping);        
}
    }
}
