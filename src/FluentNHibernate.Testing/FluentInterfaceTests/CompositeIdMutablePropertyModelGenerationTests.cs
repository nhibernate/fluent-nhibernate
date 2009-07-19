using System.Linq;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class CompositeIdMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AccessShouldSetModelAccessPropertyToValue()
        {
            CompositeId<IdentityTarget>()
                .Mapping(m => m.Access.AsField())
                .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void WithKeyPropertyShouldAddToModelKeyPropertiesCollection()
        {
            CompositeId<IdentityTarget>()
                .Mapping(m => m.KeyProperty(x => x.IntId))
                .ModelShouldMatch(x => x.KeyProperties.Count().ShouldEqual(1));
        }

        [Test]
        public void WithKeyReferenceShouldAddToModelKeyManyToOnesCollection()
        {
            CompositeId<IdentityTarget>()
                .Mapping(m => m.KeyReference(x => x.IntId))
                .ModelShouldMatch(x => x.KeyManyToOnes.Count().ShouldEqual(1));
        }

        [Test, Ignore]
        public void ShouldAllowSeparateClassRepresentingCompositeId()
        {
            Assert.Fail();
        }
    }
}