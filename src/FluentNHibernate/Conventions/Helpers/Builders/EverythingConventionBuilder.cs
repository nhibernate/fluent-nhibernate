using System;
using System.Collections.Generic;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class EverythingConventionBuilder : IConventionBuilder<IEntireMappingsConvention, IEnumerable<IClassMap>>
    {
        public IEntireMappingsConvention Always(Action<IEnumerable<IClassMap>> convention)
        {
            return new BuiltEntireMappingsConvention(x => true, convention);
        }

        public IEntireMappingsConvention When(Func<IEnumerable<IClassMap>, bool> isTrue, Action<IEnumerable<IClassMap>> convention)
        {
            return new BuiltEntireMappingsConvention(isTrue, convention);
        }
    }
}