using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.AutoMap
{
    public class PrivateAutoMapper : AutoMapper
    {
        private readonly AutoMappingExpressions expressions;

        internal PrivateAutoMapper(AutoMappingExpressions expressions, IConventionFinder conventionFinder)
            : base(expressions, conventionFinder)
        {
            this.expressions = expressions;
        }

        public override void mapEverythingInClass<T>(AutoMap<T> map)
        {
            // This will ONLY map private properties. Do not call base.

            var rule = expressions.FindMappablePrivateProperties;
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
