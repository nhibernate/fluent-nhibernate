using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances;

public class AnyInstance(AnyMapping mapping) : AnyInspector(mapping), IAnyInstance
{
    readonly AnyMapping mapping = mapping;

    public new IAccessInstance Access
    {
        get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
    }
}
