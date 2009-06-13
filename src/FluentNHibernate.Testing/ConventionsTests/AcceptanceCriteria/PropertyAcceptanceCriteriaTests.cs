using System;
using System.Linq;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using TestFixture = NUnit.Framework.TestFixtureAttribute;
using SetUp = NUnit.Framework.SetUpAttribute;
using Test = NUnit.Framework.TestAttribute;

namespace FluentNHibernate.Testing.ConventionsTests.AcceptanceCriteria
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaTests
    {
        private IAcceptanceCriteria<IPropertyInspector> acceptance;

        [SetUp]
        public void CreateAcceptanceCriteria()
        {
            acceptance = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        }

        [Test]
        public void ShouldAllowExpectOnProperty()
        {
            acceptance.Expect(x => x.Insert, Is.Set);
        }

        [Test]
        public void ExpectOnPropertyShouldRegister()
        {
            acceptance.Expect(x => x.Insert, Is.Set);

            acceptance.Expectations.Count().ShouldEqual(1);
        }

        [Test]
        public void ExpectOnPropertyShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping() { Insert = true }))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectOnPropertyShouldValidateToFalseIfNotGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeFalse();
        }

        [Test]
        public void MultipleExpectsShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Set);
            acceptance.Expect(x => x.Update, Is.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping() { Insert = true, Update = true }))
                .ShouldBeTrue();
        }

        [Test]
        public void MultipleExpectsShouldValidateToFalseIfOnlyOneMatches()
        {
            acceptance.Expect(x => x.Insert, Is.Set);
            acceptance.Expect(x => x.Update, Is.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping() { Insert = true }))
                .ShouldBeFalse();
        }

        [Test]
        public void MultipleExpectsShouldValidateToFalseIfNoneMatch()
        {
            acceptance.Expect(x => x.Insert, Is.Set);
            acceptance.Expect(x => x.Update, Is.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeFalse();
        }
    }
}