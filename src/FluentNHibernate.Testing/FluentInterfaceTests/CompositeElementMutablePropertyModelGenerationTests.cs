using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class CompositeElementMutablePropertyModelGenerationTests :BaseModelFixture
    {
        [Test]
        public void ShouldSetClassToPropertyType()
        {
            CompositeElement<PropertyTarget>()
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Class.ShouldEqual(new TypeReference(typeof(PropertyTarget))));
        }
    }
}