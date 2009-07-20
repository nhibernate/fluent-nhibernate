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
        Laziness LazyLoad { get; }
        string Name { get; }
        string OuterJoin { get; }
        string PropertyRef { get; }
    }
}