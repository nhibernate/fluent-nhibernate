using System.Linq;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class JoinedSubclassSubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void ComponentShouldAddToModelComponentsCollection()
        {
            JoinedSubClass<PropertyTarget>()
                .Mapping(m => m.Component(x => x.Component, c => { }))
                .ModelShouldMatch(x => x.Components.Count().ShouldEqual(1));
        }

        [Test]
        public void DynamicComponentShouldAddToModelComponentsCollection()
        {
            JoinedSubClass<PropertyTarget>()
                .Mapping(m => m.DynamicComponent(x => x.ExtensionData, c => { }))
                .ModelShouldMatch(x => x.Components.Count().ShouldEqual(1));
        }

        [Test]
        public void MapShouldAddToModelPropertiesCollection()
        {
            JoinedSubClass<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.Count().ShouldEqual(1));
        }

        [Test]
        public void VersionShouldAddToModelVersionsCollection()
        {
            JoinedSubClass<VersionTarget>()
                .Mapping(m => m.Version(x => x.VersionNumber))
                .ModelShouldMatch(x => x.Versions.Count().ShouldEqual(1));
        }

        [Test]
        public void HasOneShouldAddToOneToOneCollectionOnModel()
        {
            JoinedSubClass<PropertyTarget>()
                .Mapping(m => m.HasOne(x => x.Reference))
                .ModelShouldMatch(x => x.OneToOnes.Count().ShouldEqual(1));
        }

        [Test]
        public void HasOneShouldCorrectOneToOneToCollectionOnModel()
        {
            JoinedSubClass<PropertyTarget>()
                .Mapping(m => m.HasOne(x => x.Reference))
                .ModelShouldMatch(x => x.OneToOnes.First().Name.ShouldEqual("Reference"));
        }
    }
}