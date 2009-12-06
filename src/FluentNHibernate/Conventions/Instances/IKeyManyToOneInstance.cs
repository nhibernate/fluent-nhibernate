using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IKeyManyToOneInstance : IKeyManyToOneInspector
    {
        new IAccessInstance Access { get; }
        new void ForeignKey(string name);
        void Lazy();
        new INotFoundInstance NotFound { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IKeyManyToOneInstance Not { get; }
    }
}
