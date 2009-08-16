using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IOneToManyInstance : IOneToManyInspector, IRelationshipInstance
    {
        INotFoundInstance NotFound { get; }
    }
}