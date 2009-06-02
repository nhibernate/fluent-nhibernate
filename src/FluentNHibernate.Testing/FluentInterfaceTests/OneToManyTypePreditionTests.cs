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
            OneToMany<OneToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.ShouldBeOfType<BagMapping>());
        }

        [Test]
        public void ShouldPredictSetUsage()
        {
            OneToMany<OneToManyTarget>(x => x.SetOfChildren)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.ShouldBeOfType<SetMapping>());
        }

        [Test]
        public void ShouldPredictListUsage()
        {
            OneToMany<OneToManyTarget>(x => x.ListOfChildren)
                .Mapping(m => m.AsList())
                .ModelShouldMatch(x => x.ShouldBeOfType<ListMapping>());
        }

        [Test]
        public void ShouldPredictMapUsage()
        {
            OneToMany<OneToManyTarget>(x => x.MapOfChildren)
                .Mapping(m => m.AsMap("x"))
                .ModelShouldMatch(x => x.ShouldBeOfType<MapMapping>());
        }

        [Test, Ignore]
        public void ShouldPredictArrayUsage()
        {
            OneToMany<OneToManyTarget>(x => x.ArrayOfChildren)
                .Mapping(m => m.AsElement("x"))
                .ModelShouldMatch(x => x.ShouldBeOfType<ArrayMapping>());
        }
    }
}