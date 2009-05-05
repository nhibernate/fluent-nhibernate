using System;
using System.Reflection;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.AutoMap
{
    public class PrivateAutoMapper : AutoMapper
    {
        private readonly AutoMappingExpressions localExpressions;

        internal PrivateAutoMapper(AutoMappingExpressions expressions, IConventionFinder conventionFinder)
            : base(expressions, conventionFinder)
        {
            localExpressions = expressions;
        }

        public override void MapEverythingInClass<T>(AutoMap<T> map)
        {
            // This will ONLY map private properties. Do not call base.

            var rule = localExpressions.FindMappablePrivateProperties;
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
