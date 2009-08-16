using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ClassMapSubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void MapShouldAddPropertyMappingToPropertiesCollectionOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.Count().ShouldEqual(1));
        }

        [Test]
        public void MapShouldAddPropertyMappingWithCorrectName()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.First().Name.ShouldEqual("Name"));
        }

        [Test]
        public void DiscriminateShouldSetDiscriminatorModel()
        {
            ClassMap<SuperRecord>()
                .Mapping(m => m.DiscriminateSubClassesOnColumn("col"))
                .ModelShouldMatch(x => x.Discriminator.ShouldNotBeNull());
        }

        [Test]
        public void DiscriminateShouldSetDiscriminatorColumnOnModel()
        {
            ClassMap<SuperRecord>()
                .Mapping(m => m.DiscriminateSubClassesOnColumn("col"))
                .ModelShouldMatch(x => x.Discriminator.Column.ShouldEqual("col"));
        }

        [Test]
        public void DiscriminateOverloadShouldSetDiscriminatorBaseValueOnClassModel()
        {
            ClassMap<SuperRecord>()
                .Mapping(m => m.DiscriminateSubClassesOnColumn("col", "base-value"))
                .ModelShouldMatch(x => x.DiscriminatorValue.ShouldEqual("base-value"));
        }

        [Test]
        public void DiscriminateSubClassShouldAddSubclassToModelSubclassesCollection()
        {
            ClassMap<SuperRecord>()
                .Mapping(m => m.DiscriminateSubClassesOnColumn("col").SubClass<ChildRecord>(sc => { }))
                .ModelShouldMatch(x => x.Subclasses.Count().ShouldEqual(1));
        }

        [Test]
        public void ComponentShouldAddToModelComponentsCollection()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Component(x => x.Component, c => { }))
                .ModelShouldMatch(x => x.Components.Count().ShouldEqual(1));
        }

        [Test]
        public void DynamicComponentShouldAddToModelComponentsCollection()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.DynamicComponent(x => x.ExtensionData, c => { }))
                .ModelShouldMatch(x => x.Components.Count().ShouldEqual(1));
        }

        [Test]
        public void JoinedSubclassShouldAddToModelSubclassesCollection()
        {
            ClassMap<SuperRecord>()
                .Mapping(m => m.JoinedSubClass<ChildRecord>("key", c => {}))
                .ModelShouldMatch(x => x.Subclasses.Count().ShouldEqual(1));
        }

        [Test]
        public void JoinedSubclassShouldSetKeyColumnOnModel()
        {
            ClassMap<SuperRecord>()
                .Mapping(m => m.JoinedSubClass<ChildRecord>("key", c => { }))
                .ModelShouldMatch(x => ((JoinedSubclassMapping)x.Subclasses.First()).Key.Columns.First().Name.ShouldEqual("key"));
        }

        [Test]
        public void VersionShouldSetModelVersion()
        {
            ClassMap<VersionTarget>()
                .Mapping(m => m.Version(x => x.VersionNumber))
                .ModelShouldMatch(x => x.Version.ShouldNotBeNull());
        }

        [Test]
        public void CacheShouldSetCacheModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Cache.ReadOnly())
                .ModelShouldMatch(x => x.Cache.ShouldNotBeNull());
        }

        [Test]
        public void HasOneShouldAddToOneToOneCollectionOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.HasOne(x => x.Reference))
                .ModelShouldMatch(x => x.OneToOnes.Count().ShouldEqual(1));
        }

        [Test]
        public void HasOneShouldCorrectOneToOneToCollectionOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.HasOne(x => x.Reference))
                .ModelShouldMatch(x => x.OneToOnes.First().Name.ShouldEqual("Reference"));
        }

        [Test]
        public void PropertyAddsToPropertiesCollectionOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.Count().ShouldEqual(1));
        }

        [Test]
        public void PropertyAddsToPropertiesCollectionOnModelWithName()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.First().Name.ShouldEqual("Name"));
        }

        [Test]
        public void HasManyShouldAddToCollectionsCollectionOnModel()
        {
            ClassMap<OneToManyTarget>()
                .Mapping(m => m.HasMany(x => x.BagOfChildren))
                .ModelShouldMatch(x => x.Collections.Count().ShouldEqual(1));
        }

        [Test]
        public void HasManyToManyShouldAddToCollectionsCollectionOnModel()
        {
            ClassMap<OneToManyTarget>()
                .Mapping(m => m.HasManyToMany(x => x.BagOfChildren))
                .ModelShouldMatch(x => x.Collections.Count().ShouldEqual(1));
        }

        [Test]
        public void ReferencesShouldAddToReferencesCollectionOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.References(x => x.Reference))
                .ModelShouldMatch(x => x.References.Count().ShouldEqual(1));
        }

        [Test]
        public void ReferencesWithExplicitTypeShouldUseSpecifiedType()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.References<PropertyReferenceTargetProxy>(x => x.Reference))
                .ModelShouldMatch(x => x.References.First().Class.GetUnderlyingSystemType().ShouldEqual(typeof(PropertyReferenceTargetProxy)));
        }

        [Test]
        public void ReferencesAnyShouldAddToAnyCollectionOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.ReferencesAny(x => x.Reference)
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col1")
                    .EntityTypeColumn("col2"))
                .ModelShouldMatch(x => x.Anys.Count().ShouldEqual(1));
        }

        [Test]
        public void IdShouldSetIdPropertyOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Id(x => x.Id))
                .ModelShouldMatch(x => x.Id.ShouldBeOfType<IdMapping>());
        }

        [Test]
        public void CompositeIdShouldSetIdPropertyOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.CompositeId()
                    .KeyProperty(x => x.Id))
                .ModelShouldMatch(x => x.Id.ShouldBeOfType<CompositeIdMapping>());
        }
    }
}