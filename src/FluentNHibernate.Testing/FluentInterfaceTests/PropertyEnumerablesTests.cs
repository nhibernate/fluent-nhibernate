using System.Linq;
using NHibernate.Type;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class PropertyEnumerablesTests : BaseModelFixture
    {
        [Test]
        public void ShouldSetNullableEnumsToNullable()
        {
            Property<Target>(x => x.NullableEnum)
                .Mapping(m => {})
                .ModelShouldMatch(x =>
                {
                    x.Columns.First().NotNull.ShouldBeFalse();
                });
        }

        [Test]
        public void ShouldSetNullableEnumsToUseEnumStringType()
        {
            Property<Target>(x => x.NullableEnum)
                .Mapping(m => {})
                .ModelShouldMatch(x => x.Type.GetUnderlyingSystemType().ShouldEqual(typeof(EnumStringType<Enum>)));
        }

        [Test]
        public void ShouldSetEnumsToUseEnumStringType()
        {
            Property<Target>(x => x.Enum)
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Type.GetUnderlyingSystemType().ShouldEqual(typeof(EnumStringType<Enum>)));
        }

        private class Target
        {
            public Enum? NullableEnum { get; set; }
            public Enum Enum { get; set; }
        }

        private enum Enum
        {
            
        }
    }
}