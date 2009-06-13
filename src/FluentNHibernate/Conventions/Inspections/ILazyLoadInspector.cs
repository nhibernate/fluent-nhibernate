using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface ILazyLoadInspector : IInspector
    {
        Laziness LazyLoad { get; }
    }
}