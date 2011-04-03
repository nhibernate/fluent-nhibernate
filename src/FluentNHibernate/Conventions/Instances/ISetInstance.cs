using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    [Obsolete("Use ICollectionInstance")]
    public interface ISetInstance : ISetInspector, ICollectionInstance
    {}
}
