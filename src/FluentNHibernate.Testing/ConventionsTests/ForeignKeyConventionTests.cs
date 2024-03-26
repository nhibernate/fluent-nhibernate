using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests;

[TestFixture]
public class ForeignKeyConventionTests
{
    PersistenceModel model;

    [SetUp]
    public void CreatePersistenceModel()
    {
        model = new PersistenceModel();
        model.Conventions.Add<TestForeignKeyConvention>();
    }

    [Test]
    public void ShouldSetForeignKeyOnManyToOne()
    {
        var classMap = new ClassMap<ExampleClass>();

        classMap.Id(x => x.Id);
        classMap.References(x => x.Parent);

        model.Add(classMap);

        model.BuildMappings()
            .First()
            .Classes.First()
            .References.First()
            .Columns.First().Name.ShouldEqual("Parent!");
    }

    [Test]
    public void ShouldSetForeignKeyOnOneToMany()
    {
        var classMap = new ClassMap<ExampleInheritedClass>();

        classMap.Id(x => x.Id);
        classMap.HasMany(x => x.Children);

        model.Add(classMap);

        model.BuildMappings()
            .First()
            .Classes.First()
            .Collections.First()
            .Key.Columns.First().Name.ShouldEqual("ExampleInheritedClass!");
    }

    [Test]
    public void ShouldSetForeignKeyOnManyToMany()
    {
        var classMap = new ClassMap<ExampleInheritedClass>();

        classMap.Id(x => x.Id);
        classMap.HasManyToMany(x => x.Children);

        model.Add(classMap);

        model.BuildMappings()
            .First()
            .Classes.First()
            .Collections.First()
            .Key.Columns.First().Name.ShouldEqual("ExampleInheritedClass!");
    }

    [Test]
    public void ShouldSetForeignKeyOnJoin()
    {
        var classMap = new ClassMap<ExampleInheritedClass>();

        classMap.Id(x => x.Id);
        classMap.Join("two", m => { });

        model.Add(classMap);

        model.BuildMappings()
            .First()
            .Classes.First()
            .Joins.First()
            .Key.Columns.First().Name.ShouldEqual("ExampleInheritedClass!");
    }

    [Test]
    public void ShouldSetForeignKeyOnJoinedSubclasses()
    {
        var classMap = new ClassMap<ExampleClass>();
        classMap.Id(x => x.Id);

        var subclassMap = new SubclassMap<ExampleInheritedClass>();

        model.Add(classMap);
        model.Add(subclassMap);

        model.BuildMappings()
            .First()
            .Classes.First()
            .Subclasses.First()
            .Key.Columns.First().Name.ShouldEqual("ExampleClass!");
    }

    class TestForeignKeyConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            return property is null ? type.Name + "!" : property.Name + "!";
        }
    }
}
