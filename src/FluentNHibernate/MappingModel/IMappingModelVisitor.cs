using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel
{
    public interface IMappingModelVisitor
    {
        void ProcessId(IdMapping idMapping);
        void ProcessCache(CacheMapping mapping);
        void ProcessCompositeId(CompositeIdMapping idMapping);
        void ProcessClass(ClassMapping classMapping);
        void ProcessImport(ImportMapping importMapping);
        void ProcessHibernateMapping(HibernateMapping hibernateMapping);
        void ProcessProperty(PropertyMapping propertyMapping);
        void ProcessManyToOne(ManyToOneMapping  manyToOneMapping);
        void ProcessKey(KeyMapping keyMapping);
        void ProcessGenerator(GeneratorMapping generatorMapping);
        void ProcessColumn(ColumnMapping columnMapping);
        void ProcessBag(BagMapping bagMapping);
        void ProcessOneToMany(OneToManyMapping oneToManyMapping);
        void ProcessManyToMany(ManyToManyMapping manyToManyMapping);
        void ProcessSet(SetMapping setMapping);
        void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping);
        void ProcessSubclass(SubclassMapping subclassMapping);
        void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping);
        void ProcessComponent(ComponentMappingBase componentMapping);
        void ProcessList(ListMapping listMapping);
        void ProcessIndex(IndexMapping indexMapping);
        void ProcessParent(ParentMapping parentMapping);
        void ProcessJoin(JoinMapping joinMapping);
        void ProcessCompositeElement(CompositeElementMapping compositeElementMapping);
        void ProcessVersion(VersionMapping mapping);

        void Visit(ClassMapping classMapping);
        void Visit(CacheMapping mapping);
        void Visit(ImportMapping importMapping);
        void Visit(IIdentityMapping identityMapping);
        void Visit(ICollectionMapping collectionMapping);
        void Visit(PropertyMapping propertyMapping);
        void Visit(ManyToOneMapping manyToOneMapping);
        void Visit(KeyMapping keyMapping);
        void Visit(ICollectionContentsMapping contentsMapping);
        void Visit(GeneratorMapping generatorMapping);
        void Visit(ColumnMapping columnMapping);
        void Visit(ISubclassMapping subclassMapping);
        void Visit(JoinedSubclassMapping subclassMapping);
        void Visit(SubclassMapping subclassMapping);
        void Visit(DiscriminatorMapping discriminatorMapping);
        void Visit(ComponentMappingBase componentMapping);
        void Visit(IndexMapping indexMapping);
        void Visit(ParentMapping parentMapping);
        void Visit(JoinMapping joinMapping);
        void Visit(CompositeElementMapping compositeElementMapping);
        void Visit(VersionMapping versionMapping);
    }
}
