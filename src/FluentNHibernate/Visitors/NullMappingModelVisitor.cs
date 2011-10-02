using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Visitors
{
    public abstract class NullMappingModelVisitor : IMappingModelVisitor
    {
        public virtual void ProcessId(IdMapping idMapping)
        {

        }

        public virtual void ProcessNaturalId(NaturalIdMapping naturalIdMapping)
        {

        }

        public virtual void ProcessCache(CacheMapping mapping)
        {
            
        }

        public virtual void ProcessCompositeId(CompositeIdMapping idMapping)
        {

        }

        public virtual void ProcessClass(ClassMapping classMapping)
        {

        }

        public virtual void ProcessImport(ImportMapping importMapping)
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

        public virtual void ProcessGenerator(GeneratorMapping generatorMapping)
        {

        }

        public virtual void ProcessColumn(ColumnMapping columnMapping)
        {

        }

        public virtual void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {

        }

        public virtual void ProcessManyToMany(ManyToManyMapping manyToManyMapping)
        {
            
        }

        public virtual void ProcessSubclass(SubclassMapping subclassMapping)
        {

        }

        public virtual void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping)
        {
            
        }

        public virtual void ProcessComponent(ComponentMapping mapping)
        {
            
        }

        public virtual void ProcessComponent(ReferenceComponentMapping componentMapping)
        {
            
        }

        public virtual void ProcessJoin(JoinMapping joinMapping)
        {

        }

        public virtual void ProcessCompositeElement(CompositeElementMapping compositeElementMapping)
        {
            
        }

        public virtual void ProcessVersion(VersionMapping mapping)
        {
            
        }

        public virtual void ProcessOneToOne(OneToOneMapping mapping)
        {
            
        }

        public virtual void ProcessAny(AnyMapping mapping)
        {
            
        }

        public virtual void ProcessMetaValue(MetaValueMapping mapping)
        {
            
        }

        public virtual void ProcessKeyProperty(KeyPropertyMapping mapping)
        {
            
        }

        public virtual void ProcessKeyManyToOne(KeyManyToOneMapping mapping)
        {
            
        }

        public virtual void ProcessElement(ElementMapping mapping)
        {
            
        }

        public virtual void ProcessFilter(FilterMapping mapping)
        {

        }

        public virtual void ProcessFilterDefinition(FilterDefinitionMapping mapping)
        {

        }

        public virtual void ProcessStoredProcedure(StoredProcedureMapping mapping)
        {
            
        }

        public virtual void ProcessIndex(IIndexMapping indexMapping)
        {

        }

        public virtual void ProcessIndex(IndexMapping indexMapping)
        {
            
        }

        public virtual void ProcessIndex(IndexManyToManyMapping indexMapping)
        {

        }

        public virtual void ProcessParent(ParentMapping parentMapping)
        {
            
        }

        public virtual void ProcessTuplizer(TuplizerMapping tuplizerMapping)
        {
            
        }

        public virtual void ProcessCollection(MappingModel.Collections.CollectionMapping mapping)
        {
            
        }

        public virtual void Visit(IEnumerable<HibernateMapping> mappings)
        {
            
        }

        public virtual void Visit(IdMapping mapping)
        {
            
        }

        public virtual void Visit(NaturalIdMapping naturalIdMapping)
        {

        }

        public virtual void Visit(ClassMapping classMapping)
        {

        }

        public virtual void Visit(CacheMapping mapping)
        {
            
        }

        public virtual void Visit(ImportMapping importMapping)
        {

        }

        public virtual void Visit(IIdentityMapping identityMapping)
        {

        }

        public virtual void Visit(CollectionMapping collectionMapping)
        {

        }

        public virtual void Visit(PropertyMapping propertyMapping)
        {

        }

        public virtual void Visit(ManyToOneMapping manyToOneMapping)
        {

        }

        public virtual void Visit(KeyMapping keyMapping)
        {

        }

        public virtual void Visit(ICollectionRelationshipMapping relationshipMapping)
        {

        }

        public virtual void Visit(GeneratorMapping generatorMapping)
        {

        }

        public virtual void Visit(ColumnMapping columnMapping)
        {

        }

        public virtual void Visit(SubclassMapping subclassMapping)
        {
            
        }

        public virtual void Visit(DiscriminatorMapping discriminatorMapping)
        {
            
        }

        public virtual void Visit(IComponentMapping componentMapping)
        {
            
        }

        public virtual void Visit(IIndexMapping indexMapping)
        {
            
        }
        public virtual void Visit(ParentMapping parentMapping)
        {
            
        }

        public virtual void Visit(JoinMapping joinMapping)
        {
            
        }

        public virtual void Visit(CompositeElementMapping compositeElementMapping)
        {
            
        }

        public virtual void Visit(VersionMapping versionMapping)
        {

        }

        public virtual void Visit(OneToOneMapping mapping)
        {

        }

        public virtual void Visit(OneToManyMapping mapping)
        {
            
        }

        public virtual void Visit(ManyToManyMapping mapping)
        {

        }

        public virtual void Visit(AnyMapping mapping)
        {

        }

        public virtual void Visit(MetaValueMapping mapping)
        {
            
        }

        public virtual void Visit(KeyPropertyMapping mapping)
        {
            
        }

        public virtual void Visit(KeyManyToOneMapping mapping)
        {
            
        }

        public virtual void Visit(ElementMapping mapping)
        {
            
        }

        public virtual void Visit(FilterMapping mapping)
        {

        }

        public virtual void Visit(FilterDefinitionMapping mapping)
        {

        }

        public virtual void Visit(StoredProcedureMapping mapping)
        {
            
        }

        public virtual void Visit(TuplizerMapping mapping)
        {
            
        }
    }
}
