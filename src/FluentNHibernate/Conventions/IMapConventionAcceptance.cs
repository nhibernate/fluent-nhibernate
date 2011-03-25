using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    [Obsolete("Use ICollectionConventionAcceptance")]
    public interface IMapConventionAcceptance : IConventionAcceptance<IMapInspector>
    {}
}