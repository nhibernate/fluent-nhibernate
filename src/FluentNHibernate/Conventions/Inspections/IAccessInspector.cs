using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Inspections;

public interface IAccessInspector
{
    Access Access { get; }
}
