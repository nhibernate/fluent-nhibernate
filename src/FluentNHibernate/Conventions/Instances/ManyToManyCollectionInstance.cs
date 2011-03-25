using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class ManyToManyCollectionInstance : CollectionInstance, IManyToManyCollectionInstance
    {
        private readonly MappingModel.Collections.CollectionMapping mapping;

        public ManyToManyCollectionInstance(MappingModel.Collections.CollectionMapping mapping)
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
                if (mapping.OtherSide == null || !(mapping.OtherSide is MappingModel.Collections.CollectionMapping))
                    return null;

                return new ManyToManyCollectionInstance((MappingModel.Collections.CollectionMapping)mapping.OtherSide);
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

        public bool HasExplicitTable
        {
            get { return mapping.IsSpecified("TableName"); }
        }

        IManyToManyCollectionInspector IManyToManyCollectionInspector.OtherSide
        {
            get { return OtherSide; }
        }
    }
}