using System.Linq;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ClassMapSubPartModelGenerationTests : BaseClassMapModelFixture
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
    }
}