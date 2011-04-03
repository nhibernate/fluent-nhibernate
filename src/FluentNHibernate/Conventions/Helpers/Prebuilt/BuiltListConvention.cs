using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    [Obsolete("Use BuiltCollectionConvention")]
    internal class BuiltListConvention : BuiltConventionBase<IListInspector, IListInstance>, IListConvention, IListConventionAcceptance
    {
        public BuiltListConvention(Action<IAcceptanceCriteria<IListInspector>> accept, Action<IListInstance> convention)
            : base(accept, convention)
        { }
    }
}
