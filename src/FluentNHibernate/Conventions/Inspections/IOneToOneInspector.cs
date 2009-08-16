using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IOneToOneInspector : IInspector
    {
        Access Access { get; }
        Cascade Cascade { get; }
        bool Constrained { get; }
        Fetch Fetch { get; }
        string ForeignKey { get; }
        bool LazyLoad { get; }
        string Name { get; }
        string PropertyRef { get; }
    }
}