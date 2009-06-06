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
        void ProcessMap(MapMapping mapping);
        void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping);
        void ProcessSubclass(SubclassMapping subclassMapping);
        void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping);
        void ProcessComponent(IComponentMapping componentMapping);
        void ProcessComponent(ComponentMapping componentMapping);
        void ProcessComponent(DynamicComponentMapping componentMapping);
        void ProcessList(ListMapping listMapping);
        void ProcessIndex(IIndexMapping indexMapping);
        void ProcessIndex(IndexMapping indexMapping);
        void ProcessIndex(IndexManyToManyMapping indexMapping);
        void ProcessParent(ParentMapping parentMapping);
        void ProcessJoin(JoinMapping joinMapping);
        void ProcessCompositeElement(CompositeElementMapping compositeElementMapping);
        void ProcessVersion(VersionMapping mapping);
        void ProcessOneToOne(OneToOneMapping mapping);
        void ProcessAny(AnyMapping mapping);
        void ProcessMetaValue(MetaValueMapping mapping);
        void ProcessKeyProperty(KeyPropertyMapping mapping);
        void ProcessKeyManyToOne(KeyManyToOneMapping mapping);
        void ProcessElement(ElementMapping mapping);
        void ProcessArray(ArrayMapping mapping);

        void Visit(IdMapping mapping);
        void Visit(ClassMapping classMapping);
        void Visit(CacheMapping mapping);
        void Visit(ImportMapping importMapping);
        void Visit(IIdentityMapping identityMapping);
        void Visit(ICollectionMapping collectionMapping);
        void Visit(PropertyMapping propertyMapping);
        void Visit(ManyToOneMapping manyToOneMapping);
        void Visit(KeyMapping keyMapping);
        void Visit(ICollectionRelationshipMapping relationshipMapping);
        void Visit(GeneratorMapping generatorMapping);
        void Visit(ColumnMapping columnMapping);
        void Visit(ISubclassMapping subclassMapping);
        void Visit(JoinedSubclassMapping subclassMapping);
        void Visit(SubclassMapping subclassMapping);
        void Visit(DiscriminatorMapping discriminatorMapping);
        void Visit(IComponentMapping componentMapping);
        void Visit(IIndexMapping indexMapping);
        void Visit(ParentMapping parentMapping);
        void Visit(JoinMapping joinMapping);
        void Visit(CompositeElementMapping compositeElementMapping);
        void Visit(VersionMapping versionMapping);
        void Visit(OneToOneMapping mapping);
        void Visit(OneToManyMapping mapping);
        void Visit(ManyToManyMapping mapping);
        void Visit(AnyMapping mapping);
        void Visit(MetaValueMapping mapping);
        void Visit(KeyPropertyMapping mapping);
        void Visit(KeyManyToOneMapping mapping);
        void Visit(ElementMapping mapping);
        void Visit(ArrayMapping mapping);
    }
}
