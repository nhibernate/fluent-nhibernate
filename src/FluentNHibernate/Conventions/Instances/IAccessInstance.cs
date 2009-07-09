using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IAccessInstance
    {
        IAccessStrategyBuilder Access { get; }
    }
}