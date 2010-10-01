using System.Linq;
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
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren));

            mapping.Collections.Single().ShouldBeOfType<BagMapping>();
        }

        [Test]
        public void ShouldPredictSetUsage()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.SetOfChildren));

            mapping.Collections.Single().ShouldBeOfType<SetMapping>();
        }

        [Test]
        public void ShouldPredictSetUsageWithHashSet()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.HashSetOfChildren));
         
            mapping.Collections.Single().ShouldBeOfType<SetMapping>();
        }

        [Test, Ignore]
        public void ShouldPredictArrayUsage()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.ArrayOfChildren));
            
            mapping.Collections.Single().ShouldBeOfType<ArrayMapping>();
        }
    }
}