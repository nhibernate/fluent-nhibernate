using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.PersistenceModelTests
{
    [TestFixture]
    public class JoinPersistenceModelTests
    {
        private Configuration cfg;

        [SetUp]
        public void CreateConfig()
        {
            cfg = new Configuration();

            SQLiteConfiguration.Standard.InMemory()
                .ConfigureProperties(cfg);

        }

        [Test]
        public void ShouldntDuplicateJoinMapping()
        {
            var model = new PersistenceModel();
            var classMap = new ClassMap<Target>();

            classMap.Id(x => x.Id);
            classMap.Join("other", m => m.Map(x => x.Property));

            model.Add(classMap);
            model.Configure(cfg);

            cfg.ClassMappings.First()
                .JoinClosureIterator.Count().ShouldEqual(1);
        }

        private class Target
        {
            public int Id { get; set; }
            public string Property { get; set; }
        }
    }
}