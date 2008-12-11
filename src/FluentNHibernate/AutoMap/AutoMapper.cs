using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapper
    {
        private readonly List<IAutoMapper> _mappingRules;
        private List<AutoMapType> mappingTypes;

        public AutoMapper(Conventions conventions)
        {
            _mappingRules = new List<IAutoMapper>
                                {
                                    new AutoMapIdentity(conventions), 
                                    new AutoMapVersion(conventions), 
                                    new AutoMapColumn(conventions),
                                    new ManyToManyAutoMapper(),
                                    new AutoMapManyToOne(),
                                    new AutoMapOneToMany(),
                                };
        }

        public AutoMap<T> MergeMap<T>(AutoMap<T> map)
        {
            if (mappingTypes != null)
            {
                foreach (var inheritedClass in mappingTypes.Where(q => q.Type.BaseType == typeof (T)))
                {                    
                    object joinedClass = map.JoinedSubClass(inheritedClass.Type, typeof(T).Name + "Id");
                    var method = this.GetType().GetMethod("mapEverythingInClass");
                    var genericMethod = method.MakeGenericMethod(inheritedClass.Type);
                    genericMethod.Invoke(this, new[] {joinedClass});
                    inheritedClass.IsMapped = true;
                }
            }

            mapEverythingInClass(map);
            return map;
        }

        public void mapEverythingInClass<T>(AutoMap<T> map)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                if ((property.DeclaringType == typeof(T)) && !property.PropertyType.IsEnum && property.GetIndexParameters().Length == 0)
                {
                    foreach (var rule in _mappingRules)
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
        }

        public AutoMap<T> Map<T>(List<AutoMapType> types)
        {
            var classMap = (AutoMap<T>)Activator.CreateInstance(typeof(AutoMap<T>));
            mappingTypes = types;
            return MergeMap(classMap);
        }
    }
}
