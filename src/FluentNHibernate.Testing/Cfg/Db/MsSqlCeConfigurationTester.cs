using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
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

        [Test]
        public void ShouldBeAbleToSpecifyConnectionStringDirectly()
        {
            MsSqlCeConfiguration.Standard
                .ConnectionString("conn")
                .ToProperties().ShouldContain("connection.connection_string", "conn");
        }

        [Test]
        public void MsSqlCe_should_include_40dialect()
        {
            MsSqlCeConfiguration.MsSqlCe40.ToProperties()["dialect"].ShouldEqual("NHibernate.Dialect.MsSqlCe40Dialect, " +
                typeof(ISession).Assembly.FullName);
        }
    }
}