using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapper
    {
        protected readonly List<IAutoMapper> mappingRules;
        protected List<AutoMapType> mappingTypes;
        protected AutoMappingExpressions expressions;

        public AutoMapper(AutoMappingExpressions expressions, IConventionFinder conventionFinder)
        {
            this.expressions = expressions;

            mappingRules = new List<IAutoMapper>
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

        public AutoMap<T> MergeMap<T>(AutoMap<T> map, IDictionary<Type, Action<object>> inlineOverrides)
        {
            if (mappingTypes != null)
            {
                var discriminatorSet = false;

                foreach (var inheritedClass in mappingTypes.Where(q =>
                    q.Type.BaseType == typeof(T) &&
                    !expressions.IsConcreteBaseType(q.Type.BaseType)))
                {
                    if (!discriminatorSet)
                    {
                        var discriminatorColumn = expressions.DiscriminatorColumn(typeof(T));
                        map.DiscriminateSubClassesOnColumn(discriminatorColumn);
                        discriminatorSet = true;
                    }

                    var subclassStrategy = expressions.SubclassStrategy(typeof(T));

                    if (subclassStrategy == SubclassStrategy.JoinedSubclass)
                    {
                        var subclass = map.JoinedSubClass(inheritedClass.Type, typeof(T).Name);

                        if (inlineOverrides.ContainsKey(subclass.EntityType))
                            inlineOverrides[subclass.EntityType](subclass);

                        var method = GetType().GetMethod("MapEverythingInClass");
                        var genericMethod = method.MakeGenericMethod(inheritedClass.Type);
                        genericMethod.Invoke(this, new[] { subclass });
                        inheritedClass.IsMapped = true;
                    }
                    else
                    {
                        var subclass = map.SubClass(inheritedClass.Type, inheritedClass.Type.Name);

                        if (inlineOverrides.ContainsKey(subclass.EntityType))
                            inlineOverrides[subclass.EntityType](subclass);

                        var method = GetType().GetMethod("MapEverythingInClass");
                        var genericMethod = method.MakeGenericMethod(inheritedClass.Type);
                        genericMethod.Invoke(this, new[] {subclass});
                        inheritedClass.IsMapped = true;
                    }
                }
            }

            if (inlineOverrides.ContainsKey(typeof(T)))
                inlineOverrides[typeof(T)](map);

            MapEverythingInClass(map);
            
            return map;
        }

        public virtual void MapEverythingInClass<T>(AutoMap<T> map)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                TryToMapProperty(map, property);
            }
        }

        protected void TryToMapProperty<T>(AutoMap<T> map, PropertyInfo property)
        {
            if (property.GetIndexParameters().Length == 0)
            {
                foreach (var rule in mappingRules)
                {
                    if (rule.MapsProperty(property))
                    {
                        if (map.PropertiesMapped.Count(p => p.Name == property.Name) == 0)
                        {
                            rule.Map(map, property);
                            break;
                        }
                    }
                }
            }
        }

        public AutoMap<T> Map<T>(List<AutoMapType> types, IDictionary<Type, Action<object>> overrides)
        {
            var classMap = (AutoMap<T>)Activator.CreateInstance(typeof(AutoMap<T>));
            mappingTypes = types;
            return MergeMap(classMap, overrides);
        }
    }
}
