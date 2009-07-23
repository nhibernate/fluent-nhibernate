using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class ArrayInspectorMapsToArrayMapping
    {
        private ArrayMapping mapping;
        private IArrayInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new ArrayMapping();
            inspector = new ArrayInspector(mapping);
        }

        [Test]
        public void MapsIndexToInspector()
        {
            mapping.Index = new IndexMapping();
            inspector.Index.ShouldBeOfType<IIndexInspector>();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IArrayInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}