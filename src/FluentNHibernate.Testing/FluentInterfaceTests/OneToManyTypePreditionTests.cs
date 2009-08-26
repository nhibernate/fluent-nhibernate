using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class OneToManyTypePreditionTests : BaseModelFixture
    {
        [Test]
        public void ShouldPredictBagUsage()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.ShouldBeOfType<BagMapping>());
        }

        [Test]
        public void ShouldPredictSetUsage()
        {
            OneToMany<ChildObject>(x => x.SetOfChildren)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.ShouldBeOfType<SetMapping>());
        }

        [Test]
        public void ShouldPredictSetUsageWithHashSet()
        {
            OneToMany<ChildObject>(x => x.HashSetOfChildren)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.ShouldBeOfType<SetMapping>());
        }

        [Test, Ignore]
        public void ShouldPredictArrayUsage()
        {
            OneToMany<ChildObject>(x => x.ArrayOfChildren)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.ShouldBeOfType<ArrayMapping>());
        }
    }
}