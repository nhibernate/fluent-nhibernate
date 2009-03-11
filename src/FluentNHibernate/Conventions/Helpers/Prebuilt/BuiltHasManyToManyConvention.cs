using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltHasManyToManyConvention : BuiltConventionBase<IManyToManyPart>, IHasManyToManyConvention
    {
        public BuiltHasManyToManyConvention(Func<IManyToManyPart, bool> accept, Action<IManyToManyPart> convention)
            : base(accept, convention)
        { }
    }
}