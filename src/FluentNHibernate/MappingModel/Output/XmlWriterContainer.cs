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

            RegisterWriter<ColumnMapping>(c =>
                new XmlColumnWriter());
                
            RegisterWriter<JoinMapping>(c =>
                new XmlJoinWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<DiscriminatorMapping>(c =>
                new XmlDiscriminatorWriter());

            RegisterWriter<KeyMapping>(c =>
                new XmlKeyWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ParentMapping>(c =>
                new XmlParentWriter());

            RegisterWriter<CompositeElementMapping>(c =>
                new XmlCompositeElementWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<VersionMapping>(c =>
                new XmlVersionWriter());

            RegisterWriter<CacheMapping>(c =>
                new XmlCacheWriter());

            RegisterWriter<OneToOneMapping>(c =>
                new XmlOneToOneWriter());

            // collections
            RegisterWriter<ICollectionMapping>(c =>
                new XmlCollectionWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<BagMapping>(c =>
                new XmlBagWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<SetMapping>(c =>
                new XmlSetWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ListMapping>(c =>
                new XmlListWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<MapMapping>(c =>
                new XmlMapWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ArrayMapping>(c =>
                new XmlArrayWriter(c.Resolve<IXmlWriterServiceLocator>()));

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
            RegisterWriter<ISubclassMapping>(c =>
                new XmlInheritanceWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<SubclassMapping>(c =>
                new XmlSubclassWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<JoinedSubclassMapping>(c =>
                new XmlJoinedSubclassWriter(c.Resolve<IXmlWriterServiceLocator>()));
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
                new XmlComponentBaseWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<ComponentMapping>(c =>
                new XmlComponentWriter(c.Resolve<IXmlWriterServiceLocator>()));

            RegisterWriter<DynamicComponentMapping>(c =>
                new XmlDynamicComponentWriter(c.Resolve<IXmlWriterServiceLocator>()));
        }

        private void RegisterWriter<T>(Func<Container, object> instantiate)
        {
            Register<IXmlWriter<T>>(instantiate);
        }
    }
}
