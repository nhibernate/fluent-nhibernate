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
            OneToManyMethod(x => x.GetOtherChildren())
                .Mapping(m => {})
                .ModelShouldMatch(x => x.Name.ShouldEqual("otherChildren"));
        }
    }
}