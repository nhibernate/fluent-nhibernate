using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel
{
    public abstract class DefaultMappingModelVisitor : NullMappingModelVisitor
    {
        #region Collections

        protected virtual void ProcessCollection(ICollectionMapping mapping)
        {

        }

        public override void ProcessBag(BagMapping bagMapping)
        {
            ProcessCollection(bagMapping);
        }

        public override void ProcessSet(SetMapping setMapping)
        {
            ProcessCollection(setMapping);
        }

        public override void ProcessList(ListMapping listMapping)
        {
            ProcessCollection(listMapping);
        }

        #endregion

        #region Collection Contents

        protected virtual void ProcessCollectionContents(ICollectionContentsMapping contentsMapping)
        {

        }

        public override void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {
            ProcessCollectionContents(oneToManyMapping);
        }

        #endregion

        #region Identity

        protected virtual void ProcessIdentity(IIdentityMapping idMapping)
        {

        }

        public override void ProcessId(IdMapping idMapping)
        {
            ProcessIdentity(idMapping);
        }

        public override void ProcessCompositeId(CompositeIdMapping idMapping)
        {
            ProcessIdentity(idMapping);
        }
        #endregion

        #region Classes

        protected virtual void ProcessClassBase(ClassMappingBase classMapping)
        {
            
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            ProcessClassBase(classMapping);
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            ProcessClassBase(subclassMapping);
        }

        public override void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping)
        {
            ProcessClassBase(subclassMapping);
        }

        #endregion


        public override void Visit(ClassMapping classMapping)
        {
            classMapping.AcceptVisitor(this);
        }

        public override void Visit(IIdentityMapping identityMapping)
        {
            identityMapping.AcceptVisitor(this);
        }

        public override void Visit(ICollectionMapping collectionMapping)
        {
            collectionMapping.AcceptVisitor(this);
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            propertyMapping.AcceptVisitor(this);
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            manyToOneMapping.AcceptVisitor(this);
        }

        public override void Visit(KeyMapping keyMapping)
        {
            keyMapping.AcceptVisitor(this);
        }

        public override void Visit(ICollectionContentsMapping contentsMapping)
        {
            contentsMapping.AcceptVisitor(this);
        }

        public override void Visit(IdGeneratorMapping generatorMapping)
        {
            generatorMapping.AcceptVisitor(this);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            columnMapping.AcceptVisitor(this);
        }

        public override void Visit(ISubclassMapping subclassMapping)
        {
            subclassMapping.AcceptVisitor(this);
        }

        public override void Visit(JoinedSubclassMapping subclassMapping)
        {
            subclassMapping.AcceptVisitor(this);
        }

        public override void Visit(SubclassMapping subclassMapping)
        {
            subclassMapping.AcceptVisitor(this);
        }

        public override void Visit(DiscriminatorMapping discriminatorMapping)
        {
            discriminatorMapping.AcceptVisitor(this);
        }

        public override void Visit(ComponentMapping componentMapping)
        {
            componentMapping.AcceptVisitor(this);
        }
    }
}