using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltMappingPartConvention : BuiltConventionBase<IMappingPart>, IMappingPartConvention
    {
        public BuiltMappingPartConvention(Func<IMappingPart, bool> accept, Action<IMappingPart> convention)
            : base(accept, convention)
        { }
    }
}