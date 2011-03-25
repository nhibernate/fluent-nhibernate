using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    [Obsolete("Use BuiltCollectionConvention")]
    internal class BuiltArrayConvention : BuiltConventionBase<IArrayInspector, IArrayInstance>, IArrayConvention, IArrayConventionAcceptance
    {
        public BuiltArrayConvention(Action<IAcceptanceCriteria<IArrayInspector>> accept, Action<IArrayInstance> convention)
            : base(accept, convention)
        { }
    }
}
