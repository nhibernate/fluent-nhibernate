using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel
{
    public abstract class MappingModelVisitorBase : IMappingModelVisitor
    {
        public virtual void ProcessClass(ClassMapping classMapping)
        {
            
        }

        public virtual void ProcessHibernateMapping(HibernateMapping hibernateMapping)
        {
            
        }

        public virtual void ProcessProperty(PropertyMapping propertyMapping)
        {
            
        }

        public virtual void ProcessManyToOne(ManyToOneMapping manyToOneMapping)
        {
            
        }

        public virtual void ProcessKey(KeyMapping keyMapping)
        {
            
        }

        public virtual void ProcessIdGenerator(IdGeneratorMapping generatorMapping)
        {
            
        }

        public virtual void ProcessColumn(ColumnMapping columnMapping)
        {
            
        }

        public virtual void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping)
        {

        }

        #region Collections
        public virtual void ProcessCollection(ICollectionMapping collectionMapping)
        {

        }

        public virtual void ProcessBag(BagMapping bagMapping)
        {
            ProcessCollection(bagMapping);
        }

        public virtual void ProcessSet(SetMapping setMapping)
        {
            ProcessCollection(setMapping);
        }        

        #endregion

        #region Collection Contents
        public virtual void ProcessCollectionContents(ICollectionContentsMapping contentsMapping)
        {

        }

        public virtual void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {
            ProcessCollectionContents(oneToManyMapping);
        }

        #endregion

        #region Identity
        public virtual void ProcessIdentity(IIdentityMapping idMapping)
        {

        }

        public virtual void ProcessId(IdMapping idMapping)
        {
            ProcessIdentity(idMapping);
        }

        public virtual void ProcessCompositeId(CompositeIdMapping idMapping)
        {
            ProcessIdentity(idMapping);
        } 
        #endregion


        public virtual void Visit(ClassMapping classMapping)
        {
            classMapping.AcceptVisitor(this);
        }

        public virtual void Visit(IIdentityMapping identityMapping)
        {
            identityMapping.AcceptVisitor(this);
        }

        public virtual void Visit(ICollectionMapping collectionMapping)
        {
            collectionMapping.AcceptVisitor(this);
        }

        public virtual void Visit(PropertyMapping propertyMapping)
        {
            propertyMapping.AcceptVisitor(this);
        }

        public virtual void Visit(ManyToOneMapping manyToOneMapping)
        {
            manyToOneMapping.AcceptVisitor(this);
        }

        public virtual void Visit(KeyMapping keyMapping)
        {
            keyMapping.AcceptVisitor(this);
        }

        public virtual void Visit(ICollectionContentsMapping contentsMapping)
        {
            contentsMapping.AcceptVisitor(this);
        }

        public virtual void Visit(IdGeneratorMapping generatorMapping)
        {
            generatorMapping.AcceptVisitor(this);
        }

        public virtual void Visit(ColumnMapping columnMapping)
        {
            columnMapping.AcceptVisitor(this);
        }

        public virtual void Visit(ISubclassMapping subclassMapping)
        {
            subclassMapping.AcceptVisitor(this);
        }

        public virtual void Visit(JoinedSubclassMapping subclassMapping)
        {
            subclassMapping.AcceptVisitor(this);
        }
    }
}