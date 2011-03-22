using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IOneToManyInstance : IOneToManyInspector, IRelationshipInstance
    {
        new INotFoundInstance NotFound { get; }
    }
}