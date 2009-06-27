using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Alterations.Instances
{
    public interface IPropertyInstance : IPropertyInspector, IPropertyAlteration
    {
        new IAccessStrategyBuilder Access { get; }
    }
}