using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface ICompositeElementInspector : IInspector
    {
        TypeReference Class { get; }
        IParentInspector Parent { get; }
        IEnumerable<IPropertyInspector> Properties { get; }
        IEnumerable<IManyToOneInspector> References { get; }
    }
}