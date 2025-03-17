﻿using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders;

[Obsolete("Use CollectionConventionBuilder")]
class MapConventionBuilder : IConventionBuilder<IMapConvention, IMapInspector, IMapInstance>
{
    public IMapConvention Always(Action<IMapInstance> convention)
    {
        return new BuiltMapConvention(accept => { }, convention);
    }

    public IMapConvention When(Action<IAcceptanceCriteria<IMapInspector>> expectations, Action<IMapInstance> convention)
    {
        return new BuiltMapConvention(expectations, convention);
    }
}
