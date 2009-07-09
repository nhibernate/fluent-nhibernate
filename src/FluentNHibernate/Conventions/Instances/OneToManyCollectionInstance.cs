using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class OneToManyCollectionInstance : CollectionInstance, IOneToManyCollectionInstance
    {
        private bool nextBool;

        public OneToManyCollectionInstance(ICollectionMapping mapping)
            : base(mapping)
        {
            nextBool = true;
        }

        IOneToManyInspector IOneToManyCollectionInspector.Relationship
        {
            get { return Relationship; }
        }
        public IOneToManyCollectionInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }
        public IOneToManyInstance Relationship
        {
            get { throw new NotImplementedException(); }
        }
    }
}