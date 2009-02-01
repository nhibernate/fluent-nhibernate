using FluentNHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg
{
    [TestFixture]
    public class FluentlyTests
    {
        [Test]
        public void ConfigureReturnsFluentConfiguration()
        {
            Fluently.Configure().ShouldBeOfType(typeof(FluentConfiguration));
        }

        [Test]
        public void ConfigureReturnsNewInstance()
        {
            (Fluently.Configure() == Fluently.Configure()).ShouldBeFalse();
        }
    }
}
