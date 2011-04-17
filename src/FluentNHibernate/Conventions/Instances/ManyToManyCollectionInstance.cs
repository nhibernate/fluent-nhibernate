using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class ManyToManyCollectionInstance : CollectionInstance, IManyToManyCollectionInstance
    {
        private readonly CollectionMapping mapping;

        public ManyToManyCollectionInstance(CollectionMapping mapping)
            : base(mapping)
        {
            nextBool = true;
            this.mapping = mapping;
        }

        IManyToManyInspector IManyToManyCollectionInspector.Relationship
        {
            get { return Relationship; }
        }

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
                if (mapping.OtherSide == null || !(mapping.OtherSide is CollectionMapping))
                    return null;

                return new ManyToManyCollectionInstance((CollectionMapping)mapping.OtherSide);
            }
        }

        public new IManyToManyInstance Relationship
        {
            get { return new ManyToManyInstance((ManyToManyMapping)mapping.Relationship); }
        }
        public new Type ChildType
        {
            get { return mapping.ChildType; }
        }

        IManyToManyCollectionInspector IManyToManyCollectionInspector.OtherSide
        {
            get { return OtherSide; }
        }
    }
}