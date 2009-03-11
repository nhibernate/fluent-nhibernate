using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FluentNHibernate.AutoMap
{
    public class PrivateAutoMapper : AutoMapper
    {
        private readonly ConventionOverrides _conventions;

        public PrivateAutoMapper(ConventionOverrides conventions)
            : base(conventions)
        {
            _conventions = conventions;
        }

        public override void mapEverythingInClass<T>(AutoMap<T> map)
        {
            // This will ONLY map private properties. Do not call base.

            var rule = _conventions.FindMappablePrivateProperties;
            if (rule == null)
                throw new InvalidOperationException("The FindMappablePrivateProperties convention must be supplied to use the PrivateAutoMapper. ");

            foreach (var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (rule(property))
                    TryToMapProperty(map, property);
            }
        }
    }
}
