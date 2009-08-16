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

        public override void ProcessIndex(IndexManyToManyMapping indexMapping)
        {
            ProcessIndex((IIndexMapping)indexMapping);
        }

        public override void ProcessIndex(IndexMapping indexMapping)
        {
            ProcessIndex((IIndexMapping)indexMapping);
        }

        #endregion

        #region Collection Relationship

        protected virtual void ProcessCollectionContents(ICollectionRelationshipMapping relationshipMapping)
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

        public override void Visit(ICollectionRelationshipMapping relationshipMapping)
        {
            relationshipMapping.AcceptVisitor(this);
        }

        public override void Visit(GeneratorMapping generatorMapping)
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

        public override void Visit(IComponentMapping componentMapping)
        {
            componentMapping.AcceptVisitor(this);
        }

        public override void Visit(JoinMapping joinMapping)
        {
            joinMapping.AcceptVisitor(this);
        }

        public override void Visit(CompositeElementMapping compositeElementMapping)
        {
            compositeElementMapping.AcceptVisitor(this);
        }

        public override void Visit(VersionMapping versionMapping)
        {
            versionMapping.AcceptVisitor(this);
        }

        public override void Visit(OneToOneMapping mapping)
        {
            mapping.AcceptVisitor(this);
        }

        public override void Visit(IIndexMapping indexMapping)
        {
            indexMapping.AcceptVisitor(this);
        }
    }
}