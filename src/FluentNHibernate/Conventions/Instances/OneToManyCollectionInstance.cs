using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class OneToManyCollectionInstance : CollectionInstance, IOneToManyCollectionInstance
    {
        private readonly CollectionMapping mapping;

        public OneToManyCollectionInstance(CollectionMapping mapping)
            : base(mapping)
        {
            nextBool = true;
            this.mapping = mapping;
        }

        IOneToManyInspector IOneToManyCollectionInspector.Relationship
        {
            get { return Relationship; }
        }

        IManyToOneInspector IOneToManyCollectionInspector.OtherSide
        {
            get { return OtherSide; }
        }

        public IManyToOneInstance OtherSide
        {
            get
            {
                var otherSide = mapping.OtherSide as ManyToOneMapping;
                if (otherSide == null)
                    return null;

                return new ManyToOneInstance(otherSide);
            }
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