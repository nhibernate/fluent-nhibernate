using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltKeyPropertyConvention : BuiltConventionBase<IKeyPropertyInspector, IKeyPropertyInstance>, IKeyPropertyConvention, IKeyPropertyConventionAcceptance
    {
        public BuiltKeyPropertyConvention(Action<IAcceptanceCriteria<IKeyPropertyInspector>> accept, Action<IKeyPropertyInstance> convention)
            :base(accept, convention)
	    {}
    }
}