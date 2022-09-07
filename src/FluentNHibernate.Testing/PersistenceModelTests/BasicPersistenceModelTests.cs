using System.Collections.Generic;
using FakeItEasy;
using FluentNHibernate.MappingModel;
using NHibernate.Cfg;
using NUnit.Framework;
using static FluentNHibernate.Testing.Cfg.SQLiteFrameworkConfigurationFactory;

namespace FluentNHibernate.Testing.PersistenceModelTests
{
    [TestFixture]
    public class BasicPersistenceModelTests
    {
        private Configuration cfg;

        [SetUp]
        public void CreateConfig()
        {
            cfg = new Configuration();

            CreateStandardInMemoryConfiguration()
                .ConfigureProperties(cfg);
        }

        [Test]
        public void ShouldInvokeMappingApplicationStrategy()
        {
            var mockStrategy = A.Fake<IMappingApplicationStrategy>();

            var model = new PersistenceModel();
            model.MappingApplicationStrategy = mockStrategy;

            model.Configure(cfg);

            A.CallTo(() => mockStrategy.ApplyMappingsToConfiguration(A<IEnumerable<HibernateMapping>>.Ignored, A<Configuration>.Ignored, A<int>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}