using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltSetConvention : BuiltConventionBase<ISetInspector, ISetInstance>, ISetConvention
    {
        public BuiltSetConvention(Action<IAcceptanceCriteria<ISetInspector>> accept, Action<ISetInstance> convention)
            : base(accept, convention)
        { }
    }
}
