using System;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlWriterContainer : Container
    {
        public XmlWriterContainer()
        {
            Register<IXmlWriterServiceLocator>(c =>
                new XmlWriterServiceLocator(this));

            RegisterWriter<HibernateMapping>(c =>
                new XmlHibernateMappingWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ClassMapping>(c =>
                new XmlClassWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ImportMapping>(c =>
                new XmlImportWriter());

            RegisterWriter<PropertyMapping>(c =>
                new XmlPropertyWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterIdWriters();
            RegisterComponentWriters();

            RegisterWriter<NaturalIdMapping>(c =>
                new XmlNaturalIdWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ColumnMapping>(c =>
                new XmlColumnWriter());
                
            RegisterWriter<JoinMapping>(c =>
                new XmlJoinWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<DiscriminatorMapping>(c =>
                new XmlDiscriminatorWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<KeyMapping>(c =>
                new XmlKeyWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ParentMapping>(c =>
                new XmlParentWriter());

            RegisterWriter<CompositeElementMapping>(c =>
                new XmlCompositeElementWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<VersionMapping>(c =>
                new XmlVersionWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<CacheMapping>(c =>
                new XmlCacheWriter());

            RegisterWriter<OneToOneMapping>(c =>
                new XmlOneToOneWriter());

            // collections
            RegisterWriter<CollectionMapping>(c =>
                new XmlCollectionWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<IIndexMapping>(c =>
                new XmlIIndexWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<IndexMapping>(c =>
                new XmlIndexWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<IndexManyToManyMapping>(c =>
                new XmlIndexManyToManyWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ElementMapping>(c =>
                new XmlElementWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<OneToManyMapping>(c =>
                new XmlOneToManyWriter());

            RegisterWriter<AnyMapping>(c =>
                new XmlAnyWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<MetaValueMapping>(c =>
                new XmlMetaValueWriter());

            // collection relationships
            RegisterWriter<ICollectionRelationshipMapping>(c =>
                new XmlCollectionRelationshipWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ManyToOneMapping>(c =>
                new XmlManyToOneWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ManyToManyMapping>(c =>
                new XmlManyToManyWriter(c.Resolve<IXmlWriterServiceLocator>()));

            // subclasses
            RegisterWriter<SubclassMapping>(c =>
                new XmlSubclassWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<FilterMapping>(c =>
                new XmlFilterWriter());

            RegisterWriter<FilterDefinitionMapping>(c =>
                new XmlFilterDefinitionWriter());

            RegisterWriter<StoredProcedureMapping>(c =>
                new XmlStoredProcedureWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<TuplizerMapping>(c =>
                new XmlTuplizerWriter());
        }

        private void RegisterIdWriters()
        {
            RegisterWriter<IIdentityMapping>(c =>
                new XmlIdentityBasedWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<IdMapping>(c =>
                new XmlIdWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<CompositeIdMapping>(c =>
                new XmlCompositeIdWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<GeneratorMapping>(c =>
                new XmlGeneratorWriter());

            RegisterWriter<KeyPropertyMapping>(c =>
                new XmlKeyPropertyWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<KeyManyToOneMapping>(c =>
                new XmlKeyManyToOneWriter(c.Resolve<IXmlWriterServiceLocator>()));
        }

        private void RegisterComponentWriters()
        {
            RegisterWriter<IComponentMapping>(c =>
                new XmlComponentWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ReferenceComponentMapping>(c =>
                new XmlReferenceComponentWriter(c.Resolve<IXmlWriterServiceLocator>()));
        }

        private void RegisterWriter<T>(Func<Container, object> instantiate)
        {
            Register<IXmlWriter<T>>(instantiate);
        }
    }
}
