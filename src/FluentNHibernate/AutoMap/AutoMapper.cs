using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel.ClassBased;

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

        public IAutoClasslike MergeMap<T>(IAutoClasslike map, IDictionary<Type, Action<object>> inlineOverrides)
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

                        if (inlineOverrides.ContainsKey(inheritedClass.Type))
                            inlineOverrides[inheritedClass.Type](subclass);

                        MapEverythingInClass(subclass, inheritedClass.Type);
                        inheritedClass.IsMapped = true;
                    }
                    else
                    {
                        var subclass = map.SubClass(inheritedClass.Type, inheritedClass.Type.Name);

                        if (inlineOverrides.ContainsKey(inheritedClass.Type))
                            inlineOverrides[inheritedClass.Type](subclass);

                        MapEverythingInClass(subclass, inheritedClass.Type);
                        inheritedClass.IsMapped = true;
                    }
                }
            }

            if (inlineOverrides.ContainsKey(typeof(T)))
                inlineOverrides[typeof(T)](map);

            MapEverythingInClass(map, typeof(T));
            
            return map;
        }

        public virtual void MapEverythingInClass(IAutoClasslike map, Type entityType)
        {
            foreach (var property in entityType.GetProperties())
            {
                TryToMapProperty(map, property);
            }
        }

        protected void TryToMapProperty(IAutoClasslike map, PropertyInfo property)
        {
            if (property.GetIndexParameters().Length == 0)
            {
                foreach (var rule in mappingRules)
                {
                    if (rule.MapsProperty(property))
                    {
                        if (map.PropertiesMapped.Count(p => p.Name == property.Name) == 0)
                        {
                            var mapping = map.GetMapping();

                            if (mapping is ClassMapping)
                                rule.Map((ClassMapping)mapping, property);
                            else if (mapping is SubclassMapping)
                                rule.Map((SubclassMapping)mapping, property);
                            else if (mapping is JoinedSubclassMapping)
                                rule.Map((JoinedSubclassMapping)mapping, property);
                            break;
                        }
                    }
                }
            }
        }

        public IAutoClasslike Map<T>(List<AutoMapType> types, IDictionary<Type, Action<object>> overrides)
        {
            var classMap = (AutoMap<T>)Activator.CreateInstance(typeof(AutoMap<T>));
            mappingTypes = types;
            return MergeMap<T>(classMap, overrides);
        }
    }
}
