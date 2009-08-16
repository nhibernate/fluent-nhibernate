using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltListConvention : BuiltConventionBase<IListInspector, IListInstance>, IListConvention
    {
        public BuiltListConvention(Action<IAcceptanceCriteria<IListInspector>> accept, Action<IListInstance> convention)
            : base(accept, convention)
        { }
    }
}
