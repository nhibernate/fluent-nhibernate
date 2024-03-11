using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances;

public class ManyToManyCollectionInstance : CollectionInstance, IManyToManyCollectionInstance
{
    private readonly CollectionMapping mapping;

    public ManyToManyCollectionInstance(CollectionMapping mapping)
        : base(mapping)
    {
        nextBool = true;
        this.mapping = mapping;
    }

    IManyToManyInspector IManyToManyCollectionInspector.Relationship => Relationship;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new IManyToManyCollectionInstance Not
    {
        get
        {
            nextBool = !nextBool;
            return this;
        }
    }
        
    public IManyToManyCollectionInstance OtherSide
    {
        get
        {
            var otherSide = mapping.OtherSide as CollectionMapping;
            if (otherSide is null)
                return null;

            return new ManyToManyCollectionInstance(otherSide);
        }
    }

    public new IManyToManyInstance Relationship => new ManyToManyInstance((ManyToManyMapping)mapping.Relationship);

    public new Type ChildType => mapping.ChildType;

    IManyToManyCollectionInspector IManyToManyCollectionInspector.OtherSide => OtherSide;
}
