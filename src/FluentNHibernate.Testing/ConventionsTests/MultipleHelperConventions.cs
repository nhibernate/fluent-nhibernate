using System.Linq;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests;

[TestFixture]
public class MultipleHelperConventions
{
    PersistenceModel model;

    [SetUp]
    public void CreatePersistenceModel()
    {
        model = new PersistenceModel();
    }

    [Test]
    public void AlwaysShouldSetDefaultLazyToTrue()
    {
        var classMap = new ClassMap<Target>();
        classMap.Id(x => x.Id);
        model.Add(classMap);
        model.Conventions.Add(DefaultLazy.Always());
        model.Conventions.Add(DefaultCascade.All());
            
        var mapping = model.BuildMappings().First();

        mapping.DefaultLazy.ShouldBeTrue();
        mapping.DefaultCascade.ShouldEqual("all");
    }

    class Target
    {
        public int Id { get; set; }
    }
}
