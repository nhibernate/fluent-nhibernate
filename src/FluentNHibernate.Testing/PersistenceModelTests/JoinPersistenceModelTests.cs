using System.Linq;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;
using NUnit.Framework;
using static FluentNHibernate.Testing.Cfg.SQLiteFrameworkConfigurationFactory;

namespace FluentNHibernate.Testing.PersistenceModelTests;

[TestFixture]
public class JoinPersistenceModelTests
{
    Configuration cfg;

    [SetUp]
    public void CreateConfig()
    {
        cfg = new Configuration();

        CreateStandardInMemoryConfiguration()
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

    class Target
    {
        public int Id { get; set; }
        public string Property { get; set; }
    }
}
