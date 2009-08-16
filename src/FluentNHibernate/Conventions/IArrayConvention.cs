using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    public interface IArrayConvention : IConvention<IArrayInspector, IArrayInstance>
    {
    }
}
