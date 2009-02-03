using FluentNHibernate.Cfg.Db;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class SQLiteConfigurationTester
    {
        [Test]
        public void should_set_up_default_query_substitutions()
        {
            new SQLiteConfiguration().ToProperties()["query.substitutions"].ShouldEqual("true=1;false=0");
        }

        [Test]
        public void in_memory_should_set_up_expected_connection_string()
        {
            new SQLiteConfiguration().InMemory()
                .ToProperties()["connection.connection_string"].ShouldEqual("Data Source=:memory:;Version=3;New=True;");
        }

        [Test]
        public void using_file_should_set_up_expected_connection_string()
        {
            new SQLiteConfiguration().UsingFile("foo")
                .ToProperties()["connection.connection_string"].ShouldEqual("Data Source=foo;Version=3;New=True;");
        }
    }
}