using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel
{
    public interface IMappingModelVisitor
    {
        void ProcessId(IdMapping idMapping);
        void ProcessCompositeId(CompositeIdMapping idMapping);
        void ProcessClass(ClassMapping classMapping);
        void ProcessImport(ImportMapping importMapping);
        void ProcessHibernateMapping(HibernateMapping hibernateMapping);
        void ProcessProperty(PropertyMapping propertyMapping);
        void ProcessManyToOne(ManyToOneMapping  manyToOneMapping);
        void ProcessKey(KeyMapping keyMapping);
        void ProcessIdGenerator(IdGeneratorMapping generatorMapping);
        void ProcessColumn(ColumnMapping columnMapping);
        void ProcessBag(BagMapping bagMapping);
        void ProcessOneToMany(OneToManyMapping oneToManyMapping);
        void ProcessManyToMany(ManyToManyMapping manyToManyMapping);
        void ProcessSet(SetMapping setMapping);
        void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping);
        void ProcessSubclass(SubclassMapping subclassMapping);
        void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping);
        void ProcessComponent(ComponentMapping componentMapping);
        void ProcessList(ListMapping listMapping);
        void ProcessIndex(IndexMapping indexMapping);

        void Visit(ClassMapping classMapping);
        void Visit(ImportMapping importMapping);
        void Visit(IIdentityMapping identityMapping);
        void Visit(ICollectionMapping collectionMapping);
        void Visit(PropertyMapping propertyMapping);
        void Visit(ManyToOneMapping manyToOneMapping);
        void Visit(KeyMapping keyMapping);
        void Visit(ICollectionContentsMapping contentsMapping);
        void Visit(IdGeneratorMapping generatorMapping);
        void Visit(ColumnMapping columnMapping);
        void Visit(ISubclassMapping subclassMapping);
        void Visit(JoinedSubclassMapping subclassMapping);
        void Visit(SubclassMapping subclassMapping);
        void Visit(DiscriminatorMapping discriminatorMapping);
        void Visit(ComponentMapping componentMapping);
        void Visit(IndexMapping indexMapping);
    }
}