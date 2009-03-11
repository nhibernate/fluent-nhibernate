using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class MappingPartConventionBuilder : IConventionBuilder<IMappingPartConvention, IMappingPart>
    {
        public IMappingPartConvention Always(Action<IMappingPart> convention)
        {
            return new BuiltMappingPartConvention(x => true, convention);
        }

        public IMappingPartConvention When(Func<IMappingPart, bool> isTrue, Action<IMappingPart> convention)
        {
            return new BuiltMappingPartConvention(isTrue, convention);
        }
    }
}