using System;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmConverterContainer : Container
    {
        public HbmConverterContainer()
        {
            Register<IHbmConverterServiceLocator>(c =>
                new HbmConverterServiceLocator(this));

            RegisterConverter<HibernateMapping, HbmMapping>(c =>
                new HbmHibernateMappingConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<ClassMapping, HbmClass>(c =>
                new HbmClassConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<ImportMapping, HbmImport>(c =>
                new HbmImportConverter());

            RegisterConverter<PropertyMapping, HbmProperty>(c =>
                new HbmPropertyConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterIdConverters();
            RegisterComponentConverters();

            /*
            RegisterConverter<NaturalIdMapping, HbmNaturalId>(c =>
                new HbmNaturalIdConverter(c.Resolve<IHbmConverterServiceLocator>()));
            */

            RegisterConverter<ColumnMapping, HbmColumn>(c =>
                new HbmColumnConverter());
            
            RegisterConverter<JoinMapping, HbmJoin>(c =>
                new HbmJoinConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<DiscriminatorMapping, HbmDiscriminator>(c =>
                new HbmDiscriminatorConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<KeyMapping, HbmKey>(c =>
                new HbmKeyConverter(c.Resolve<IHbmConverterServiceLocator>()));

            /*
            RegisterConverter<ParentMapping, HbmParent>(c =>
                new HbmParentConverter());

            RegisterConverter<CompositeElementMapping, HbmCompositeElement>(c =>
                new HbmCompositeElementConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<VersionMapping, HbmVersion>(c =>
                new HbmVersionConverter(c.Resolve<IHbmConverterServiceLocator>()));
            */

            RegisterConverter<CacheMapping, HbmCache>(c =>
                new HbmCacheConverter());

            /*
            RegisterConverter<OneToOneMapping, HbmOneToOne>(c =>
                new HbmOneToOneConverter());
            */

            RegisterCollectionConverters();

            /*
            // FIXME: What does this need to convert as?
            RegisterConverter<IIndexMapping>(c =>
                new HbmIIndexConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<IndexMapping, HbmIndex>(c =>
                new HbmIndexConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<IndexManyToManyMapping, HbmIndexManyToMany>(c =>
                new HbmIndexManyToManyConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<ElementMapping, HbmElement>(c =>
                new HbmElementConverter(c.Resolve<IHbmConverterServiceLocator>()));
            */

            RegisterConverter<OneToManyMapping, HbmOneToMany>(c =>
                new HbmOneToManyConverter());
            
            /*
            RegisterConverter<AnyMapping, HbmAny>(c =>
                new HbmAnyConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<MetaValueMapping, HbmMetaValue>(c =>
                new HbmMetaValueConverter());
            */

            // collection relationships
            RegisterConverter<ICollectionRelationshipMapping, object>(c =>
                new HbmCollectionRelationshipConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<ManyToOneMapping, HbmManyToOne>(c =>
                new HbmManyToOneConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<ManyToManyMapping, HbmManyToMany>(c =>
                new HbmManyToManyConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterSubclassConverters();

            RegisterConverter<FilterMapping, HbmFilter>(c =>
                new HbmFilterConverter());

            RegisterConverter<FilterDefinitionMapping, HbmFilterDef>(c =>
                new HbmFilterDefinitionConverter());

            RegisterConverter<StoredProcedureMapping, HbmCustomSQL>(c =>
                new HbmStoredProcedureConverter());

            /*
            RegisterConverter<TuplizerMapping, HbmTuplizer>(c =>
                new HbmTuplizerConverter());
            */
        }

        private void RegisterIdConverters()
        {
            RegisterConverter<IIdentityMapping, object>(c =>
                new HbmIdentityBasedConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<IdMapping, HbmId>(c =>
                new HbmIdConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<CompositeIdMapping, HbmCompositeId>(c =>
                new HbmCompositeIdConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<GeneratorMapping, HbmGenerator>(c =>
                new HbmGeneratorConverter());

            RegisterConverter<KeyPropertyMapping, HbmKeyProperty>(c =>
                new HbmKeyPropertyConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<KeyManyToOneMapping, HbmKeyManyToOne>(c =>
                new HbmKeyManyToOneConverter(c.Resolve<IHbmConverterServiceLocator>()));
        }

        private void RegisterCollectionConverters()
        {
            RegisterConverter<CollectionMapping, object>(c =>
                new HbmCollectionConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<CollectionMapping, HbmArray>(c =>
                new HbmArrayConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<CollectionMapping, HbmBag>(c =>
                new HbmBagConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<CollectionMapping, HbmList>(c =>
                new HbmListConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<CollectionMapping, HbmMap>(c =>
                new HbmMapConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<CollectionMapping, HbmSet>(c =>
                new HbmSetConverter(c.Resolve<IHbmConverterServiceLocator>()));
        }

        private void RegisterSubclassConverters()
        {
            RegisterConverter<SubclassMapping, object>(c =>
                new HbmSubclassConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<SubclassMapping, HbmSubclass>(c =>
                new HbmBasicSubclassConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<SubclassMapping, HbmJoinedSubclass>(c =>
                new HbmJoinedSubclassConverter(c.Resolve<IHbmConverterServiceLocator>()));

            RegisterConverter<SubclassMapping, HbmUnionSubclass>(c =>
                new HbmUnionSubclassConverter(c.Resolve<IHbmConverterServiceLocator>()));
        }

        private void RegisterComponentConverters()
        {
            /*
            // FIXME: What does this need to convert as?
            // FIXME: Needs an explicit import or to be fully explicit here, depending on whether we need  both IComponentMapping types...
            RegisterConverter<IComponentMapping, HbmComponent>(c =>
                new HbmComponentConverter(c.Resolve<IHbmConverterServiceLocator>()));

            // FIXME: What does this need to convert as?
            RegisterConverter<ReferenceComponentMapping>(c =>
                new HbmReferenceComponentConverter(c.Resolve<IHbmConverterServiceLocator>()));
            */
        }

        private void RegisterConverter<F, H>(Func<Container, object> instantiate)
        {
            Register<IHbmConverter<F, H>>(instantiate);
        }
    }
}
