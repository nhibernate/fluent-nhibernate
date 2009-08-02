using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    public interface IListConvention : IConvention<IListInspector, IListInstance>
    {
    }
}
