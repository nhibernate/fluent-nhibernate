using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
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
                .ModelShouldMatch(x => x.Discriminator.ColumnName.ShouldEqual("col"));
        }

        [Test]
        public void DiscriminateOverloadShouldSetDiscriminatorBaseValueOnClassModel()
        {
            ClassMap<SuperRecord>()
                .Mapping(m => m.DiscriminateSubClassesOnColumn("col", "base-value"))
                .ModelShouldMatch(x => x.DiscriminatorBaseValue.ShouldEqual("base-value"));
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
        public void VersionShouldAddToModelVersionsCollection()
        {
            ClassMap<VersionTarget>()
                .Mapping(m => m.Version(x => x.VersionNumber))
                .ModelShouldMatch(x => x.Versions.Count().ShouldEqual(1));
        }

        [Test]
        public void CacheShouldSetCacheModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Cache.AsReadOnly())
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
    }
}