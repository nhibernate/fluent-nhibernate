using System.Linq;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Alterations
{
    [TestFixture, Category("Alteration DSL")]
    public class PropertyAlterationMapsToPropertyMapping
    {
        private PropertyMapping mapping;
        private IPropertyAlteration alteration;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new PropertyMapping();
            alteration = new PropertyInstance(mapping);
        }

        [Test]
        public void AccessMapped()
        {
            alteration.Access.AsField();
            mapping.Access.ShouldEqual("field");
        }

        [Test]
        public void ColumnShouldInheritAttributesFromDefault()
        {
            mapping.AddDefaultColumn(new ColumnMapping { NotNull = true });

            alteration.ColumnName("test");

            mapping.Columns.Count().ShouldEqual(1);
            mapping.Columns.First().Name.ShouldEqual("test");
            mapping.Columns.First().NotNull.ShouldBeTrue(); ;
        }

        [Test]
        public void ColumnShouldHaveAttributesAppliedWhenAddedBeforeSetting()
        {
            mapping.AddDefaultColumn(new ColumnMapping());

            alteration.ColumnName("test");
            alteration.Not.Nullable();

            mapping.Columns.Count().ShouldEqual(1);
            mapping.Columns.First().Name.ShouldEqual("test");
            mapping.Columns.First().NotNull.ShouldBeTrue(); ;
        }

        [Test]
        public void ColumnShouldHaveAttributesAppliedWhenAddedAfterSetting()
        {
            mapping.AddDefaultColumn(new ColumnMapping());

            alteration.Not.Nullable();
            alteration.ColumnName("test");

            mapping.Columns.Count().ShouldEqual(1);
            mapping.Columns.First().Name.ShouldEqual("test");
            mapping.Columns.First().NotNull.ShouldBeTrue(); ;
        }
    }
}