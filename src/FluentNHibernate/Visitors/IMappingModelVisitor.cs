using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Visitors;

public interface IMappingModelVisitor
{
    void ProcessId(IdMapping idMapping);
    void ProcessNaturalId(NaturalIdMapping naturalIdMapping);
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
    void ProcessOneToMany(OneToManyMapping oneToManyMapping);
    void ProcessManyToMany(ManyToManyMapping manyToManyMapping);
    void ProcessSubclass(SubclassMapping subclassMapping);
    void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping);
    void ProcessComponent(ComponentMapping mapping);
    void ProcessComponent(ReferenceComponentMapping componentMapping);
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
    void ProcessFilter(FilterMapping mapping);
    void ProcessFilterDefinition(FilterDefinitionMapping mapping);
    void ProcessStoredProcedure(StoredProcedureMapping mapping);
    void ProcessTuplizer(TuplizerMapping mapping);
    void ProcessCollection(MappingModel.Collections.CollectionMapping mapping);

    /// <summary>
    /// This bad boy is the entry point to the visitor
    /// </summary>
    /// <param name="mappings"></param>
    void Visit(IEnumerable<HibernateMapping> mappings);

    void Visit(IdMapping mapping);
    void Visit(NaturalIdMapping naturalIdMapping);
    void Visit(ClassMapping classMapping);
    void Visit(CacheMapping mapping);
    void Visit(ImportMapping importMapping);
    void Visit(IIdentityMapping identityMapping);
    void Visit(MappingModel.Collections.CollectionMapping collectionMapping);
    void Visit(PropertyMapping propertyMapping);
    void Visit(ManyToOneMapping manyToOneMapping);
    void Visit(KeyMapping keyMapping);
    void Visit(ICollectionRelationshipMapping relationshipMapping);
    void Visit(GeneratorMapping generatorMapping);
    void Visit(ColumnMapping columnMapping);
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
    void Visit(FilterMapping mapping);
    void Visit(FilterDefinitionMapping mapping);
    void Visit(StoredProcedureMapping mapping);
    void Visit(TuplizerMapping mapping);
}
