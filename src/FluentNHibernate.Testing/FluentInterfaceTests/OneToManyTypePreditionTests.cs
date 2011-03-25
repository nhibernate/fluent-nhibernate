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
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Collection.ShouldEqual(Collection.Bag));
        }

        [Test]
        public void ShouldPredictSetUsage()
        {
            OneToMany(x => x.SetOfChildren)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Collection.ShouldEqual(Collection.Set));
        }

        [Test]
        public void ShouldPredictSetUsageWithHashSet()
        {
            OneToMany(x => x.HashSetOfChildren)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Collection.ShouldEqual(Collection.Set));
        }

        [Test, Ignore]
        public void ShouldPredictArrayUsage()
        {
            OneToMany(x => x.ArrayOfChildren)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Collection.ShouldEqual(Collection.Array));
        }
    }
}