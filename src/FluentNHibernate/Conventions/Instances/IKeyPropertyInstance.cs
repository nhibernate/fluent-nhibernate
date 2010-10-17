using System.Collections.Generic;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IKeyPropertyInstance : IKeyPropertyInspector
    {
        new IAccessInstance Access { get; }
        new void Length(int length);
    }
}
