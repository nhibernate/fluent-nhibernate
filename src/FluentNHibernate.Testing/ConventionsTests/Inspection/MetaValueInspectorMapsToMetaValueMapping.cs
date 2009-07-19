using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class MetaValueInspectorMapsToMetaValueMapping
    {
        private MetaValueMapping mapping;
        private IMetaValueInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new MetaValueMapping();
            inspector = new MetaValueInspector(mapping);
        }

        [Test]
        public void ClassMapped()
        {
            mapping.Class = new TypeReference(typeof(string));
            inspector.Class.ShouldEqual(new TypeReference(typeof(string)));
        }

        [Test]
        public void ClassIsSet()
        {
            mapping.Class = new TypeReference(typeof(string));
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
        public void ValueMapped()
        {
            mapping.Value = "value";
            inspector.Value.ShouldEqual("value");
        }

        [Test]
        public void ValueIsSet()
        {
            mapping.Value = "value";
            inspector.IsSet(Prop(x => x.Value))
                .ShouldBeTrue();
        }

        [Test]
        public void ValueIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Value))
                .ShouldBeFalse();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IMetaValueInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}