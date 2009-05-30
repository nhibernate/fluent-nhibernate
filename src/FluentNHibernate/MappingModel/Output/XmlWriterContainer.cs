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
            RegisterWriter<HibernateMapping>(c =>
                new XmlHibernateMappingWriter(c.Resolve<IXmlWriter<ClassMapping>>(), c.Resolve<IXmlWriter<ImportMapping>>()));

            RegisterWriter<ClassMapping>(c =>
                new XmlClassWriter(
                    c.Resolve<IXmlWriter<PropertyMapping>>(),
                    c.Resolve<IXmlWriter<DiscriminatorMapping>>(),
                    c.Resolve<IXmlWriter<ISubclassMapping>>(),
                    c.Resolve<IXmlWriter<ComponentMapping>>(),
                    c.Resolve<IXmlWriter<DynamicComponentMapping>>(),
                    c.Resolve<IXmlWriter<JoinMapping>>(),
                    c.Resolve<IXmlWriter<VersionMapping>>(),
                    c.Resolve<IXmlWriter<IIdentityMapping>>(),
                    c.Resolve<IXmlWriter<OneToOneMapping>>()));

            RegisterWriter<ImportMapping>(c =>
                new XmlImportWriter());

            RegisterWriter<PropertyMapping>(c =>
                new XmlPropertyWriter(c.Resolve<IXmlWriter<ColumnMapping>>()));

            RegisterWriter<ComponentMapping>(c =>
                new XmlComponentWriter(
                   c.Resolve<IXmlWriter<PropertyMapping>>(),
                   c.Resolve<IXmlWriter<ParentMapping>>(),
                   c.Resolve<IXmlWriter<VersionMapping>>(),
                   c.Resolve<IXmlWriter<OneToOneMapping>>()));

            RegisterWriter<DynamicComponentMapping>(c =>
                new XmlDynamicComponentWriter(
                    c.Resolve<IXmlWriter<PropertyMapping>>(),
                    c.Resolve<IXmlWriter<ParentMapping>>(),
                    c.Resolve<IXmlWriter<VersionMapping>>(),
                    c.Resolve<IXmlWriter<OneToOneMapping>>()));

            RegisterWriter<ColumnMapping>(c =>
                new XmlColumnWriter());
                
            RegisterWriter<JoinMapping>(c =>
                new XmlJoinWriter(
                    c.Resolve<IXmlWriter<PropertyMapping>>(),
                    c.Resolve<IXmlWriter<KeyMapping>>()));

            RegisterWriter<DiscriminatorMapping>(c =>
                new XmlDiscriminatorWriter());

            RegisterWriter<KeyMapping>(c =>
                new XmlKeyWriter(c.Resolve<IXmlWriter<ColumnMapping>>()));

            RegisterWriter<ParentMapping>(c =>
                new XmlParentWriter());

            RegisterWriter<CompositeElementMapping>(c =>
                new XmlCompositeElementWriter(c.Resolve<IXmlWriter<PropertyMapping>>()));

            RegisterWriter<VersionMapping>(c =>
                new XmlVersionWriter());

            RegisterWriter<CacheMapping>(c =>
                new XmlCacheWriter());

            RegisterWriter<IIdentityMapping>(c =>
                new XmlIdentityBasedWriter(c.Resolve<IXmlWriter<IdMapping>>()));

            RegisterWriter<IdMapping>(c =>
                new XmlIdWriter(
                    c.Resolve<IXmlWriter<GeneratorMapping>>(),
                    c.Resolve<IXmlWriter<ColumnMapping>>()));

            RegisterWriter<GeneratorMapping>(c =>
                new XmlGeneratorWriter());

            RegisterWriter<OneToOneMapping>(c =>
                new XmlOneToOneWriter());

            // collections
            RegisterWriter<ICollectionMapping>(c =>
                new XmlCollectionWriter(
                    c.Resolve<IXmlWriter<BagMapping>>(),
                    c.Resolve<IXmlWriter<SetMapping>>(),
                    c.Resolve<IXmlWriter<ListMapping>>(),
                    c.Resolve<IXmlWriter<MapMapping>>()));

            RegisterWriter<BagMapping>(c =>
                new XmlBagWriter(
                    c.Resolve<IXmlWriter<KeyMapping>>(),
                    c.Resolve<IXmlWriter<ICollectionRelationshipMapping>>(),
                    c.Resolve<IXmlWriter<CacheMapping>>()));

            RegisterWriter<SetMapping>(c =>
                new XmlSetWriter(
                    c.Resolve<IXmlWriter<KeyMapping>>(),
                    c.Resolve<IXmlWriter<ICollectionRelationshipMapping>>(),
                    c.Resolve<IXmlWriter<CacheMapping>>()));

            RegisterWriter<ListMapping>(c =>
                new XmlListWriter(
                    c.Resolve<IXmlWriter<KeyMapping>>(),
                    c.Resolve<IXmlWriter<ICollectionRelationshipMapping>>(),
                    c.Resolve<IXmlWriter<CacheMapping>>()));

            RegisterWriter<MapMapping>(c =>
                new XmlMapWriter(
                    c.Resolve<IXmlWriter<KeyMapping>>(),
                    c.Resolve<IXmlWriter<ICollectionRelationshipMapping>>(),
                    c.Resolve<IXmlWriter<CacheMapping>>()));

            RegisterWriter<OneToManyMapping>(c =>
                new XmlOneToManyWriter());

            RegisterWriter<AnyMapping>(c =>
                new XmlAnyWriter(c.Resolve<IXmlWriter<ColumnMapping>>()));

            // collection relationships
            RegisterWriter<ICollectionRelationshipMapping>(c =>
                new XmlCollectionRelationshipWriter(
                    c.Resolve<IXmlWriter<OneToManyMapping>>(),
                    c.Resolve<IXmlWriter<ManyToManyMapping>>()));

            RegisterWriter<ManyToOneMapping>(c =>
                new XmlManyToOneWriter());

            RegisterWriter<ManyToManyMapping>(c =>
                new XmlManyToManyWriter(c.Resolve<IXmlWriter<ColumnMapping>>()));

            // subclasses
            RegisterWriter<ISubclassMapping>(c =>
                new XmlInheritanceWriter(c.Resolve<IXmlWriter<SubclassMapping>>(), c.Resolve<IXmlWriter<JoinedSubclassMapping>>()));

            RegisterWriter<SubclassMapping>(c =>
                new XmlSubclassWriter(
                    c.Resolve<IXmlWriter<PropertyMapping>>(),
                    c.Resolve<IXmlWriter<ComponentMapping>>(),
                    c.Resolve<IXmlWriter<DynamicComponentMapping>>(),
                    c.Resolve<IXmlWriter<VersionMapping>>(),
                    c.Resolve<IXmlWriter<OneToOneMapping>>()));

            RegisterWriter<JoinedSubclassMapping>(c =>
                new XmlJoinedSubclassWriter(
                    c.Resolve<IXmlWriter<PropertyMapping>>(),
                    c.Resolve<IXmlWriter<KeyMapping>>(),
                    c.Resolve<IXmlWriter<ComponentMapping>>(),
                    c.Resolve<IXmlWriter<DynamicComponentMapping>>(),
                    c.Resolve<IXmlWriter<VersionMapping>>(),
                    c.Resolve<IXmlWriter<OneToOneMapping>>()));
        }

        private void RegisterWriter<T>(Func<Container, object> instantiate)
        {
            Register<IXmlWriter<T>>(instantiate);
        }
    }
}
