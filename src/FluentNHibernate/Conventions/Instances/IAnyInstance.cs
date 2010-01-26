using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IAnyInstance : IAnyInspector
    {
        new IAccessInstance Access { get; }
    }
}