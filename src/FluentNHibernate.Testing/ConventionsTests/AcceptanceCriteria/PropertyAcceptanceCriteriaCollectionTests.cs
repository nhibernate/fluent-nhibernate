using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.AcceptanceCriteria
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaCollectionTests
    {
        private IAcceptanceCriteria<IPropertyInspector> acceptance;

        [SetUp]
        public void CreateAcceptanceCriteria()
        {
            acceptance = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        }

        [Test]
        public void CollectionIsEmptyShouldBeTrueWhenUsingExpression()
        {
            acceptance
                .Expect(x => x.Columns.IsEmpty());

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeTrue();
        }

        [Test]
        public void CollectionIsEmptyShouldBeFalseWithItemsWhenUsingExpression()
        {
            acceptance
                .Expect(x => x.Columns.IsEmpty());

            var mapping = new PropertyMapping();
            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            acceptance
                .Matches(new PropertyInspector(mapping))
                .ShouldBeFalse();
        }

        [Test]
        public void CollectionIsNotEmptyShouldBeFalseWhenUsingExpression()
        {
            acceptance
                .Expect(x => x.Columns.IsNotEmpty());

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeFalse();
        }

        [Test]
        public void CollectionIsNotEmptyShouldBeTrueWithItemsWhenUsingExpression()
        {
            acceptance
                .Expect(x => x.Columns.IsNotEmpty());

            var mapping = new PropertyMapping();
            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            acceptance
                .Matches(new PropertyInspector(mapping))
                .ShouldBeTrue();
        }

        [Test]
        public void CollectionContainsWithStringShouldBeFalseWhenNoItemsMatching()
        {
            acceptance
                .Expect(x => x.Columns.Contains("boo"));

            var mapping = new PropertyMapping();
            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            acceptance
                .Matches(new PropertyInspector(mapping))
                .ShouldBeFalse();
        }

        [Test]
        public void CollectionContainsWithStringShouldBeTrueWhenItemsMatching()
        {
            acceptance
                .Expect(x => x.Columns.Contains("Column1"));

            var mapping = new PropertyMapping();
            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            acceptance
                .Matches(new PropertyInspector(mapping))
                .ShouldBeTrue();
        }

        [Test]
        public void CollectionContainsWithLambdaShouldBeFalseWhenNoItemsMatching()
        {
            acceptance
                .Expect(x => x.Columns.Contains(c => c.Name == "boo"));

            var mapping = new PropertyMapping();
            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            acceptance
                .Matches(new PropertyInspector(mapping))
                .ShouldBeFalse();
        }

        [Test]
        public void CollectionContainsWithLambdaShouldBeTrueWhenItemsMatching()
        {
            acceptance
                .Expect(x => x.Columns.Contains(c => c.Name == "Column1"));

            var mapping = new PropertyMapping();
            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            acceptance
                .Matches(new PropertyInspector(mapping))
                .ShouldBeTrue();
        }
    }
}