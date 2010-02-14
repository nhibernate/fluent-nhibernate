using System.Collections.Generic;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class OneToManyMethodAccessTests : BaseModelFixture
    {
        [Test]
        public void ShouldGuessBackingFieldName()
        {
            OneToMany(x => x.GetOtherChildren())
                .Mapping(m => {})
                .ModelShouldMatch(x => x.Name.ShouldEqual("otherChildren"));
        }
    }
}