using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Testing.Automapping;
using FluentNHibernate.Testing.Fixtures.AutoMappingAlterations.Model;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Overrides
{
    [TestFixture]
    public class ParentOverridesWithSubclasses
    {
        [Test]
        public void ShouldntDuplicatePropertiesWhenParentOverridden()
        {
            var model = AutoMap.Source(new StubTypeSource(new[] {typeof(Parent), typeof(Child), typeof(Property)}))
                .Override<Parent>(o => o.Map(x => x.Name));

            model.CompileMappings();
            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.Properties.First().Name.ShouldEqual("Name");
            classMapping.Subclasses.First().Properties.ShouldNotContain(x => x.Name == "Name");
        }

        [Test]
        public void ShouldntDuplicateCollectionsWhenParentOverridden()
        {
            var model = AutoMap.Source(new StubTypeSource(new[] { typeof(Parent), typeof(Child), typeof(Property) }))
                .Override<Parent>(o => o.HasMany(x => x.Properties));

            model.CompileMappings();
            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.Collections.First().Name.ShouldEqual("Properties");
            classMapping.Subclasses.First().Collections.Count().ShouldEqual(0);
        }

        [Test]
        public void ShouldApplyOverrideToSubclassOnly()
        {
            var model = AutoMap.Source(new StubTypeSource(new[] { typeof(Parent), typeof(Child), typeof(Property) }))
                .Override<Child>(o => o.Map(x => x.AnotherProperty).Access.Field());

            model.CompileMappings();
            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.Subclasses.First().Properties.ShouldContain(x => x.Name == "AnotherProperty" && x.Access == "field");
        }

        [Test]
        public void ShouldIgnorePropertiesInParentAndAllowOverridesInChild()
        {
            var model = AutoMap.Source(new StubTypeSource(new[] { typeof(Parent), typeof(Child), typeof(Property) }))
                .Override<Parent>(o => o.IgnoreProperty(x => x.Name))
                .Override<Child>(o => o.Map(x => x.Name).Access.Field());

            model.CompileMappings();
            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.Properties.Count().ShouldEqual(0);
            classMapping.Subclasses.First().Properties.ShouldContain(x => x.Name == "Name" && x.Access == "field");
        }

        [Test]
        public void ShouldIgnorePropertiesInChild()
        {
            var model = AutoMap.Source(new StubTypeSource(new[] {typeof(Parent), typeof(Child), typeof(Property)}))
                .Override<Child>(o => o.IgnoreProperty(x => x.AnotherProperty));

            model.CompileMappings();
            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.Subclasses.First().Properties.ShouldNotContain(x => x.Name == "AnotherProperty");
        }
    }

    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Property> Properties { get; set; }
    }

    public class Child : Parent
    {
        public string AnotherProperty { get; set; }
    }

    public class Property
    {
        public int Id { get; set; }
    }
}