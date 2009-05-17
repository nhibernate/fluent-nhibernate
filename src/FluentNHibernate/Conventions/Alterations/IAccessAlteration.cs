using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Alterations
{
    public interface IAccessAlteration
    {
        IAccessStrategyBuilder Access { get; }
    }
}