using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class VersionConventionBuilder : IConventionBuilder<IVersionConvention, IVersionInspector, IVersionInstance>
    {
        public IVersionConvention Always(Action<IVersionInstance> convention)
        {
            return new BuiltVersionConvention(x => {}, convention);
        }

        public IVersionConvention When(Action<IAcceptanceCriteria<IVersionInspector>> expectations, Action<IVersionInstance> convention)
        {
            return new BuiltVersionConvention(expectations, convention);
        }
    }
}