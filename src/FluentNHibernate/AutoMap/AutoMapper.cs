using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using System.Reflection;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapper
    {
        protected readonly List<IAutoMapper> _mappingRules;
        protected List<AutoMapType> mappingTypes;
        protected AutoMappingExpressions expressions;

        public AutoMapper(AutoMappingExpressions expressions, IConventionFinder conventionFinder)
        {
            this.expressions = expressions;

            _mappingRules = new List<IAutoMapper>
            {
                new AutoMapIdentity(expressions), 
                new AutoMapVersion(), 
                new AutoMapComponent(expressions),
                new AutoMapColumn(conventionFinder),
                new ManyToManyAutoMapper(expressions),
                new AutoMapManyToOne(),
                new AutoMapOneToMany(),
            };
        }

        public AutoMap<T> MergeMap<T>(AutoMap<T> map)
        {
            if (mappingTypes != null)
            {
                foreach (var inheritedClass in mappingTypes.Where(q =>
                    q.Type.BaseType == typeof(T) &&
                    !expressions.IsConcreteBaseType.Invoke(q.Type.BaseType)))
                {
                    object joinedClass = map.JoinedSubClass(inheritedClass.Type, typeof (T).Name + "Id");
                    var method = GetType().GetMethod("mapEverythingInClass");
                    var genericMethod = method.MakeGenericMethod(inheritedClass.Type);
                    genericMethod.Invoke(this, new[] {joinedClass});
                    inheritedClass.IsMapped = true;
                }
            }

            mapEverythingInClass(map);
            
            return map;
        }

        public virtual void mapEverythingInClass<T>(AutoMap<T> map)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                TryToMapProperty<T>(map, property);
            }
        }

        protected void TryToMapProperty<T>(AutoMap<T> map, PropertyInfo property)
        {
            if (property.GetIndexParameters().Length == 0)
            {
                foreach (var rule in _mappingRules)
                {
                    if (rule.MapsProperty(property))
                    {
                        if (map.PropertiesMapped.Count(p => p.Name == property.Name) == 0)
                        {
                            rule.Map<T>(map, property);
                            break;
                        }
                    }
                }
            }
        }

        public AutoMap<T> Map<T>(List<AutoMapType> types)
        {
            var classMap = (AutoMap<T>)Activator.CreateInstance(typeof(AutoMap<T>));
            mappingTypes = types;
            return MergeMap(classMap);
        }
    }
}
