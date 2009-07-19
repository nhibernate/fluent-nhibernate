using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class GeneratorInspectorMapsToGeneratorMapping
    {
        private GeneratorMapping mapping;
        private IGeneratorInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new GeneratorMapping();
            inspector = new GeneratorInspector(mapping);
        }

        [Test]
        public void ClassMapped()
        {
            mapping.Class = "class";
            inspector.Class.ShouldEqual("class");
        }

        [Test]
        public void ClassIsSet()
        {
            mapping.Class = "class";
            inspector.IsSet(Prop(x => x.Class))
                .ShouldBeTrue();
        }

        [Test]
        public void ClassIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Class))
                .ShouldBeFalse();
        }

        [Test]
        public void ParamsHasSameCountAsMapping()
        {
            mapping.Params.Add("one", "value");
            inspector.Params.Count.ShouldEqual(1);
        }

        [Test]
        public void ParamsIsEmpty()
        {
            inspector.Params.IsEmpty().ShouldBeTrue();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IGeneratorInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}