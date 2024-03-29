using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

class BuiltKeyManyToOneConvention(Action<IAcceptanceCriteria<IKeyManyToOneInspector>> accept, Action<IKeyManyToOneInstance> convention)
    : BuiltConventionBase<IKeyManyToOneInspector, IKeyManyToOneInstance>(accept, convention), IKeyManyToOneConvention, IKeyManyToOneConventionAcceptance;
