using System;
using System.Collections.Generic;
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

        public ClassMap<T> Map<T>()
        {
            var classMap = (ClassMap<T>)Activator.CreateInstance(typeof(ClassMap<T>));

            foreach (var property in typeof(T).GetProperties())
            {
                foreach (var rule in _mappingRules)
                {
                    if (rule.MapsProperty(property))
                    {
                        rule.Map(classMap, property);
                        break;
                    }
                }
            }

            return classMap;
        }
    }
}