using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FluentNHibernate.AutoMap
{
    public class PrivateAutoMapper : AutoMapper
    {
        private readonly Conventions _conventions;

        public PrivateAutoMapper(Conventions conventions)
            : base(conventions)
        {
            _conventions = conventions;
        }

        public override void mapEverythingInClass<T>(AutoMap<T> map)
        {
            base.mapEverythingInClass<T>(map);

            var rule = _conventions.FindMappablePrivateProperties;
            if (rule == null)
                return;

            foreach (var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (rule(property))
                    TryToMapProperty(map, property);
            }
        }
    }
}
