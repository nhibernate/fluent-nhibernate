using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Inspections;

public interface IParentInspector : IInspector
{
    string Name { get; }
    Access Access { get; }
}
