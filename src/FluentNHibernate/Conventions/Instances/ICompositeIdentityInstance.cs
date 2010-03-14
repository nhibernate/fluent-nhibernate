using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface ICompositeIdentityInstance : ICompositeIdentityInspector
    {
        new void UnsavedValue(string unsavedValue);
        new IAccessInstance Access { get; }
        new void Mapped();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ICompositeIdentityInstance Not { get; }

        new IEnumerable<IKeyPropertyInstance> KeyProperties { get; }
        new IEnumerable<IKeyManyToOneInstance> KeyManyToOnes { get; }
    }
}
