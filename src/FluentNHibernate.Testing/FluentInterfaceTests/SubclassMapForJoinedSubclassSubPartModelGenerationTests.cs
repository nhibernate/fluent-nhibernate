using System.Linq;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class SubclassMapForJoinedSubclassSubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void ComponentShouldAddToModelComponentsCollection()
        {
            JoinedSubclass<PropertyTarget>()
                .Mapping(m => m.Component(x => x.Component, c => { }))
                .ModelShouldMatch(x => x.Components.Count().ShouldEqual(1));
        }

        [Test]
        public void DynamicComponentShouldAddToModelComponentsCollection()
        {
            JoinedSubclass<PropertyTarget>()
                .Mapping(m => m.DynamicComponent(x => x.ExtensionData, c => { }))
                .ModelShouldMatch(x => x.Components.Count().ShouldEqual(1));
        }

        [Test]
        public void MapShouldAddToModelPropertiesCollection()
        {
            JoinedSubclass<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.Count().ShouldEqual(1));
        }

        [Test]
        public void HasOneShouldAddToOneToOneCollectionOnModel()
        {
            JoinedSubclass<PropertyTarget>()
                .Mapping(m => m.HasOne(x => x.Reference))
                .ModelShouldMatch(x => x.OneToOnes.Count().ShouldEqual(1));
        }

        [Test]
        public void HasOneShouldCorrectOneToOneToCollectionOnModel()
        {
            JoinedSubclass<PropertyTarget>()
                .Mapping(m => m.HasOne(x => x.Reference))
                .ModelShouldMatch(x => x.OneToOnes.First().Name.ShouldEqual("Reference"));
        }

        [Test]
        public void PropertyAddsToPropertiesCollectionOnModel()
        {
            JoinedSubclass<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.Count().ShouldEqual(1));
        }

        [Test]
        public void PropertyAddsToPropertiesCollectionOnModelWithName()
        {
            JoinedSubclass<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.First().Name.ShouldEqual("Name"));
        }

        [Test]
        public void HasManyShouldAddToCollectionsCollectionOnModel()
        {
            JoinedSubclass<OneToManyTarget>()
                .Mapping(m => m.HasMany(x => x.BagOfChildren))
                .ModelShouldMatch(x => x.Collections.Count().ShouldEqual(1));
        }

        [Test]
        public void HasManyToManyShouldAddToCollectionsCollectionOnModel()
        {
            JoinedSubclass<OneToManyTarget>()
                .Mapping(m => m.HasManyToMany(x => x.BagOfChildren))
                .ModelShouldMatch(x => x.Collections.Count().ShouldEqual(1));
        }

        [Test]
        public void ReferencesShouldAddToReferencesCollectionOnModel()
        {
            JoinedSubclass<PropertyTarget>()
                .Mapping(m => m.References(x => x.Reference))
                .ModelShouldMatch(x => x.References.Count().ShouldEqual(1));
        }

        [Test]
        public void ReferencesAnyShouldAddToAnyCollectionOnModel()
        {
            JoinedSubclass<PropertyTarget>()
                .Mapping(m => m.ReferencesAny(x => x.Reference)
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col1")
                    .EntityTypeColumn("col2"))
                .ModelShouldMatch(x => x.Anys.Count().ShouldEqual(1));
        }
    }
}