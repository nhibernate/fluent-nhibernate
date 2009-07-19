using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IOneToOneInspector : IInspector
    {
        Access Access { get; }
        string Cascade { get; }
        bool Constrained { get; }
        string Fetch { get; }
        string ForeignKey { get; }
        Laziness LazyLoad { get; }
        string Name { get; }
        string OuterJoin { get; }
        string PropertyRef { get; }
    }
}