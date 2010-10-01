using System.Linq;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class OneToManyMethodAccessTests : BaseModelFixture
    {
        [Test]
        public void ShouldGuessBackingFieldName()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.GetOtherChildren()));

            mapping.Collections.Single()
                .Name.ShouldEqual("otherChildren");
        }
    }
}