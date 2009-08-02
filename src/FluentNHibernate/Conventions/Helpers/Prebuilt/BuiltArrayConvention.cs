using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltArrayConvention : BuiltConventionBase<IArrayInspector, IArrayInstance>, IArrayConvention
    {
        public BuiltArrayConvention(Action<IAcceptanceCriteria<IArrayInspector>> accept, Action<IArrayInstance> convention)
            : base(accept, convention)
        { }
    }
}
