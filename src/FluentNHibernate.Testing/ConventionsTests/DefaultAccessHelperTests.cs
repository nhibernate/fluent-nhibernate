using System.Linq;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests;

[TestFixture]
public class DefaultAccessHelperTests
{
    PersistenceModel model;

    [SetUp]
    public void CreatePersistenceModel()
    {
        model = new PersistenceModel();
    }

    [Test]
    public void ShouldSetDefaultAccessToValue()
    {
        var classMap = new ClassMap<Target>();
        classMap.Id(x => x.Id);
        model.Add(classMap);
        model.Conventions.Add(DefaultAccess.Field());
        model.BuildMappings()
            .First()
            .DefaultAccess.ShouldEqual("field");
    }

    class Target
    {
        public int Id { get; set; }
    }
}
