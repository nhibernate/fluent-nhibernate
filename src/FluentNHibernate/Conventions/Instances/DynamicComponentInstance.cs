using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Instances;

public class DynamicComponentInstance(ComponentMapping mapping)
    : DynamicComponentInspector(mapping), IDynamicComponentInstance
{
    private bool nextBool = true;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IDynamicComponentInstance Not
    {
        get
        {
            nextBool = !nextBool;
            return this;
        }
    }


    public new IAccessInstance Access
    {
        get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
    }

    public new void Update()
    {
        mapping.Set(x => x.Update, Layer.Conventions, nextBool);
        nextBool = true;
    }

    public new void Insert()
    {
        mapping.Set(x => x.Insert, Layer.Conventions, nextBool);
        nextBool = true;
    }

    public new void Unique()
    {
        mapping.Set(x => x.Unique, Layer.Conventions, nextBool);
        nextBool = true;
    }

    public new void OptimisticLock()
    {
        mapping.Set(x => x.OptimisticLock, Layer.Conventions, nextBool);
        nextBool = true;
    }

    public new IEnumerable<IOneToOneInstance> OneToOnes
    {
        get { return mapping.OneToOnes.Select(x => new OneToOneInstance(x)); }
    }

    public new IEnumerable<IPropertyInstance> Properties
    {
        get { return mapping.Properties.Select(x => new PropertyInstance(x)); }
    }    
}
