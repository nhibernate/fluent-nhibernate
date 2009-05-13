using System.Linq;
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
    }
}