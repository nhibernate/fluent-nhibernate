using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class ConnectionStringBuilderTester
    {
        private ConnectionStringBuilderDouble builder;

        [SetUp]
        public void CreateBuilder()
        {
            builder = new ConnectionStringBuilderDouble();
        }

        [Test]
        public void CanExplicitlySetConnectionString()
        {
            builder.Is("a string");
            builder.ConnectionString.ShouldEqual("a string");
        }

        private class ConnectionStringBuilderDouble : ConnectionStringBuilder
        {
            public string ConnectionString
            {
                get { return Create(); }
            }
        }
    }
}