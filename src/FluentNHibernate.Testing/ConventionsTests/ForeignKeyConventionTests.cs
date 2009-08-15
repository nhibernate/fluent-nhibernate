using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class ForeignKeyConventionTests
    {
        private PersistenceModel model;

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

            classMap.Join("two", m => {});

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
            var subclassMap = new SubclassMap<ExampleInheritedClass>();

            model.Add(classMap);
            model.Add(subclassMap);

            model.BuildMappings()
                .First()
                .Classes.First()
                .Subclasses.Cast<JoinedSubclassMapping>().First()
                .Key.Columns.First().Name.ShouldEqual("ExampleClass!");
        }

        private class TestForeignKeyConvention : ForeignKeyConvention
        {
            protected override string GetKeyName(PropertyInfo property, Type type)
            {
                return property == null ? type.Name + "!" : property.Name + "!";
            }
        }
    }
}