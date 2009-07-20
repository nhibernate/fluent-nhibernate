using FluentNHibernate.Conventions.Inspections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection.ValueTypes
{
    [TestFixture]
    public class FetchTests
    {
        [Test]
        public void ShouldBeEqualToAnotherTheSame()
        {
            Fetch.Select.ShouldEqual(Fetch.Select);
        }

        [Test]
        public void ShouldNotBeEqualToADifferentOne()
        {
            Fetch.Select.ShouldNotEqual(Fetch.Join);
        }

        [Test]
        public void SelectShouldHaveCorrectValue()
        {
            Fetch.Select.ToString().ShouldEqual("select");
        }

        [Test]
        public void JoinShouldHaveCorrectValue()
        {
            Fetch.Join.ToString().ShouldEqual("join");
        }
    }
}