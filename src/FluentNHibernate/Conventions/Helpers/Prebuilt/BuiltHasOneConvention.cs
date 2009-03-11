using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltHasOneConvention : BuiltConventionBase<IOneToOnePart>, IHasOneConvention
    {
        public BuiltHasOneConvention(Func<IOneToOnePart, bool> accept, Action<IOneToOnePart> convention)
            : base(accept, convention)
        { }
    }
}