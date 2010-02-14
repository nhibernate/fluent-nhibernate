using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IExposedThroughPropertyInspector : IInspector
    {
        Member Property { get; }
    }
}