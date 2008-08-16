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

        public AutoMapper()
        {
            _mappingRules = new List<IAutoMapper>
                                {
                                    new AutoMapIdentity(), 
                                    new AutoMapVersion(), 
                                    new AutoMapColumn(),
                                    new AutoMapManyToOne(),
                                    new AutoMapOneToMany(),
                                };
        }

        public AutoMap<T> MergeMap<T>(AutoMap<T> map)
        {
            foreach (var property in typeof(T).GetProperties())
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
            return map;
        }

        public AutoMap<T> Map<T>()
        {
            var classMap = (AutoMap<T>)Activator.CreateInstance(typeof(AutoMap<T>));
            return MergeMap(classMap);
        }

        public void SetConvention<T>(Func<PropertyInfo, bool> propertyConvention, Func<PropertyInfo, string> columnConvention)
        {
            _mappingRules
                .Find(map => map.GetType() == typeof (T))
                .SetConvention(propertyConvention, columnConvention);
        }
    }
}