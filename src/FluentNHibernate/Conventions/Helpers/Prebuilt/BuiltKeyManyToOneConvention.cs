using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltKeyManyToOneConvention : BuiltConventionBase<IKeyManyToOneInspector, IKeyManyToOneInstance>, IKeyManyToOneConvention, IKeyManyToOneConventionAcceptance
    {
        public BuiltKeyManyToOneConvention(Action<IAcceptanceCriteria<IKeyManyToOneInspector>> accept, Action<IKeyManyToOneInstance> convention)
            :base(accept, convention)
	    {}
    }
}