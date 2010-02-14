using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    public interface IKeyPropertyConvention : IConvention<IKeyPropertyInspector, IKeyPropertyInstance>
    {}
}
