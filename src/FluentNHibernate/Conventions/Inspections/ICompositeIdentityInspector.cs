using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface ICompositeIdentityInspector : IIdentityInspectorBase
    {
        TypeReference Class { get; }
        IEnumerable<IKeyManyToOneInspector> KeyManyToOnes { get; }
        IEnumerable<IKeyPropertyInspector> KeyProperties { get; }
        bool Mapped { get; }
    }
}