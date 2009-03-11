using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltHasManyConvention : BuiltConventionBase<IOneToManyPart>, IHasManyConvention
    {
        public BuiltHasManyConvention(Func<IOneToManyPart, bool> accept, Action<IOneToManyPart> convention)
            : base(accept, convention)
        { }
    }
}