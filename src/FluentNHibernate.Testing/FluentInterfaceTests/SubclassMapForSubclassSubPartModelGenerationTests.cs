using System;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class SubclassMapForSubclassSubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void ComponentShouldAddToModelComponentsCollection()
        {
            SubclassMapForSubclass<PropertyTarget>()
                .Mapping(m => m.Component(x => x.Component, c => { }))
                .ModelShouldMatch(x => x.Components.Count().ShouldEqual(1));
        }

        [Test]
        public void ComponentWithPropertiesShouldAddToModelComponentsCollection()
        {
            var classMap = new ClassMap<Parent>();

            classMap.Id(x => x.Id);

            var subclassMap = new SubclassMap<Child>();

            subclassMap.Component(x => x.Component, c => c.Map(x => x.Name));

            var model = new PersistenceModel();

            model.Add(classMap);
            model.Add(subclassMap);

            model.BuildMappings()
                .First()
                .Classes.First()
                .Subclasses.First()
                .Components.Count().ShouldEqual(1);
        }

        private class Parent
        {
            public int Id { get; set; }
        }

        private class Child : Parent
        {
            public Component Component { get; set; }
        }

        private class Component
        {
            public string Name { get; set; }
        }

        [Test]
        public void DynamicComponentShouldAddToModelComponentsCollection()
        {
            SubclassMapForSubclass<PropertyTarget>()
                .Mapping(m => m.DynamicComponent(x => x.ExtensionData, c => { }))
                .ModelShouldMatch(x => x.Components.Count().ShouldEqual(1));
        }

        [Test]
        public void MapShouldAddToModelPropertiesCollection()
        {
            SubclassMapForSubclass<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.Count().ShouldEqual(1));
        }

        [Test]
        public void SubClassShouldAddToModelSubclassesCollection()
        {
            SubclassMapForSubclass<SuperRecord>()
                .Mapping(m => m.Subclass<ChildRecord>(x => { }))
                .ModelShouldMatch(x => x.Subclasses.Count().ShouldEqual(1));
        }

        [Test]
        public void HasOneShouldAddToOneToOneCollectionOnModel()
        {
            SubclassMapForSubclass<PropertyTarget>()
                .Mapping(m => m.HasOne(x => x.Reference))
                .ModelShouldMatch(x => x.OneToOnes.Count().ShouldEqual(1));
        }

        [Test]
        public void HasOneShouldCorrectOneToOneToCollectionOnModel()
        {
            SubclassMapForSubclass<PropertyTarget>()
                .Mapping(m => m.HasOne(x => x.Reference))
                .ModelShouldMatch(x => x.OneToOnes.First().Name.ShouldEqual("Reference"));
        }

        [Test]
        public void PropertyAddsToPropertiesCollectionOnModel()
        {
            SubclassMapForSubclass<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.Count().ShouldEqual(1));
        }

        [Test]
        public void PropertyAddsToPropertiesCollectionOnModelWithName()
        {
            SubclassMapForSubclass<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.First().Name.ShouldEqual("Name"));
        }

        [Test]
        public void HasManyShouldAddToCollectionsCollectionOnModel()
        {
            SubclassMapForSubclass<OneToManyTarget>()
                .Mapping(m => m.HasMany(x => x.BagOfChildren))
                .ModelShouldMatch(x => x.Collections.Count().ShouldEqual(1));
        }

        [Test]
        public void HasManyToManyShouldAddToCollectionsCollectionOnModel()
        {
            SubclassMapForSubclass<OneToManyTarget>()
                .Mapping(m => m.HasManyToMany(x => x.BagOfChildren))
                .ModelShouldMatch(x => x.Collections.Count().ShouldEqual(1));
        }

        [Test]
        public void ReferencesShouldAddToReferencesCollectionOnModel()
        {
            SubclassMapForSubclass<PropertyTarget>()
                .Mapping(m => m.References(x => x.Reference))
                .ModelShouldMatch(x => x.References.Count().ShouldEqual(1));
        }

        [Test]
        public void ReferencesAnyShouldAddToAnyCollectionOnModel()
        {
            SubclassMapForSubclass<PropertyTarget>()
                .Mapping(m => m.ReferencesAny(x => x.Reference)
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col1")
                    .EntityTypeColumn("col2"))
                .ModelShouldMatch(x => x.Anys.Count().ShouldEqual(1));
        }
    }
}