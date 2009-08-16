using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class FirebirdConfigurationTester
    {
        private FirebirdConfiguration _config;
        private IDictionary<string, string> _props;

        [SetUp]
        public void SetUp()
        {
            _config = new FirebirdConfiguration();
            _props = _config.ToProperties();
        }

        [Test]
        public void should_set_up_default_query_substitutions()
        {
            _props["query.substitutions"].ShouldEqual("true 1, false 0, yes 1, no 0");
        }

        [Test]
        public void should_use_ReadCommitted_as_the_default_isolation_level()
        {
            _props["connection.isolation"].ShouldEqual("ReadCommitted");
        }

        [Test]
        public void should_setup_connection_timout()
        {
            _props["command_timeout"].ShouldEqual("444");
        }

        [Test]
        public void should_default_to_use_outer_joins()
        {
            _props["use_outer_join"].ShouldEqual("true");
        }

        [Test]
        public void ShouldBeAbleToSpecifyConnectionStringDirectly()
        {
            _config.ConnectionString("conn");
            _props = _config.ToProperties();
            _props["connection.connection_string"].ShouldEqual("conn");
        }
    }
}