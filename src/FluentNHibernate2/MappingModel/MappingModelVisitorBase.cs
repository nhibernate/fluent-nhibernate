using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel
{
    public abstract class MappingModelVisitorBase : IMappingModelVisitor
    {
        public virtual void ProcessIdentity(IIdentityMapping idMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessId(IdMapping idMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessCompositeId(CompositeIdMapping idMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessClass(ClassMapping classMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessHibernateMapping(HibernateMapping hibernateMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessProperty(PropertyMapping propertyMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessCollection(ICollectionMapping collectionMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessManyToOne(ManyToOneMapping manyToOneMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessKey(KeyMapping keyMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessIdGenerator(IdGeneratorMapping generatorMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessColumn(ColumnMapping columnMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessBag(BagMapping bagMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessSet(SetMapping setMapping)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ProcessCollectionContents(ICollectionContentsMapping contentsMapping)
        {
            throw new System.NotImplementedException();
        }
    }
}