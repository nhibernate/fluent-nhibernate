using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class OneToManySubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void ComponentShouldSetCompositeElement()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Component(c => c.Map(x => x.Name)))
                .ModelShouldMatch(x => x.CompositeElement.ShouldNotBeNull());
        }
    }
}