using FluentNHibernate.Cfg;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg
{
    [TestFixture]
    public class MsSqlCeConfigurationTester
    {
        [Test]
        public void MsSqlCe_should_default_to_the_MsSqlCe_dialect()
        {
            MsSqlCeConfiguration.Standard.ToProperties()["dialect"].ShouldEqual("NHibernate.Dialect.MsSqlCeDialect, " +
                                                                                 typeof (ISession).Assembly.FullName);
        }
    }
}
