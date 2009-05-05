using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltReferenceConvention : BuiltConventionBase<IManyToOnePart>, IReferenceConvention
    {
        public BuiltReferenceConvention(Func<IManyToOnePart, bool> accept, Action<IManyToOnePart> convention)
            : base(accept, convention)
        { }
    }
}