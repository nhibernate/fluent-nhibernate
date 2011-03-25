using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class OneToManyCollectionInstance : CollectionInstance, IOneToManyCollectionInstance
    {
        private readonly MappingModel.Collections.CollectionMapping mapping;

        public OneToManyCollectionInstance(MappingModel.Collections.CollectionMapping mapping)
            : base(mapping)
        {
            nextBool = true;
            this.mapping = mapping;
        }

        IOneToManyInspector IOneToManyCollectionInspector.Relationship
        {
            get { return Relationship; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public new IOneToManyCollectionInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public new IOneToManyInstance Relationship
        {
            get { return new OneToManyInstance((OneToManyMapping)mapping.Relationship); }
        }
    }
}