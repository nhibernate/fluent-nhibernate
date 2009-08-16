using System.Linq;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class CompositeElementSubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void MapShouldAddToModelPropertiesCollection()
        {
            CompositeElement<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.Count().ShouldEqual(1));
        }

        [Test]
        public void ReferencesShouldAddToModelReferencesCollection()
        {
            CompositeElement<PropertyTarget>()
                .Mapping(m => m.References(x => x.Reference))
                .ModelShouldMatch(x => x.References.Count().ShouldEqual(1));
        }
    }
}