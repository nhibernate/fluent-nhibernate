using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    public interface IIndexConvention : IConvention<IIndexInspector, IIndexInstance>
    {
    }
}
