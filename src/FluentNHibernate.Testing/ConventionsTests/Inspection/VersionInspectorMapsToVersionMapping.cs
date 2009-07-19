using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class VersionInspectorMapsToVersionMapping
    {
        private VersionMapping mapping;
        private IVersionInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new VersionMapping();
            inspector = new VersionInspector(mapping);
        }

        [Test]
        public void AccessMapped()
        {
            mapping.Access = "field";
            inspector.Access.ShouldEqual(Access.AsField());
        }

        [Test]
        public void AccessIsSet()
        {
            mapping.Access = "field";
            inspector.IsSet(Prop(x => x.Access))
                .ShouldBeTrue();
        }

        [Test]
        public void AccessIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Access))
                .ShouldBeFalse();
        }

        [Test]
        public void ColumnMapped()
        {
            mapping.Column = "col";
            inspector.Column.ShouldEqual("col");
        }

        [Test]
        public void ColumnIsSet()
        {
            mapping.Column = "col";
            inspector.IsSet(Prop(x => x.Column))
                .ShouldBeTrue();
        }

        [Test]
        public void ColumnIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Column))
                .ShouldBeFalse();
        }

        [Test]
        public void GeneratedMapped()
        {
            mapping.Generated = "gen";
            inspector.Generated.ShouldEqual("gen");
        }

        [Test]
        public void GeneratedIsSet()
        {
            mapping.Generated = "gen";
            inspector.IsSet(Prop(x => x.Generated))
                .ShouldBeTrue();
        }

        [Test]
        public void GeneratedIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Generated))
                .ShouldBeFalse();
        }

        [Test]
        public void NameMapped()
        {
            mapping.Name = "name";
            inspector.Name.ShouldEqual("name");
        }

        [Test]
        public void NameIsSet()
        {
            mapping.Name = "name";
            inspector.IsSet(Prop(x => x.Name))
                .ShouldBeTrue();
        }

        [Test]
        public void NameIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Name))
                .ShouldBeFalse();
        }

        [Test]
        public void TypeMapped()
        {
            mapping.Type = new TypeReference(typeof(string));
            inspector.Type.ShouldEqual(new TypeReference(typeof(string)));
        }

        [Test]
        public void TypeIsSet()
        {
            mapping.Type = new TypeReference(typeof(string));
            inspector.IsSet(Prop(x => x.Type))
                .ShouldBeTrue();
        }

        [Test]
        public void TypeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Type))
                .ShouldBeFalse();
        }

        [Test]
        public void UnsavedValueMapped()
        {
            mapping.UnsavedValue = "test";
            inspector.UnsavedValue.ShouldEqual("test");
        }

        [Test]
        public void UnsavedValueIsSet()
        {
            mapping.UnsavedValue = "test";
            inspector.IsSet(Prop(x => x.UnsavedValue))
                .ShouldBeTrue();
        }

        [Test]
        public void UnsavedValueIsNotSet()
        {
            inspector.IsSet(Prop(x => x.UnsavedValue))
                .ShouldBeFalse();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IVersionInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}