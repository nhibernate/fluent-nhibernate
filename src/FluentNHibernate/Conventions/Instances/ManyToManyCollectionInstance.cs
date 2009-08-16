using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class ManyToManyCollectionInstance : CollectionInstance, IManyToManyCollectionInstance
    {
        private readonly ICollectionMapping mapping;

        public ManyToManyCollectionInstance(ICollectionMapping mapping)
            : base(mapping)
        {
            nextBool = true;
            this.mapping = mapping;
        }

        IManyToManyInspector IManyToManyCollectionInspector.Relationship
        {
            get { return Relationship; }
        }
        public IManyToManyCollectionInstance Not
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
                if (mapping.OtherSide == null)
                    return null;

                return new ManyToManyCollectionInstance(mapping.OtherSide);
            }
        }

        public IManyToManyInstance Relationship
        {
            get { return new ManyToManyInstance((ManyToManyMapping)mapping.Relationship); }
        }
        public Type ChildType
        {
            get { return mapping.ChildType; }
        }

        public bool HasExplicitTable
        {
            get { return mapping.IsSpecified(x => x.TableName); }
        }

        IManyToManyCollectionInspector IManyToManyCollectionInspector.OtherSide
        {
            get { return OtherSide; }
        }
    }
}