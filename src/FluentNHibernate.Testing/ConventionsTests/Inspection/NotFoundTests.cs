using FluentNHibernate.Conventions.Inspections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection.ValueTypes
{
    [TestFixture]
    public class NotFoundTests
    {
        [Test]
        public void ShouldBeEqualToAnotherTheSame()
        {
            NotFound.Ignore.ShouldEqual(NotFound.Ignore);
        }

        [Test]
        public void ShouldNotBeEqualToADifferentOne()
        {
            NotFound.Ignore.ShouldNotEqual(NotFound.Exception);
        }

        [Test]
        public void IgnoreShouldHaveCorrectValue()
        {
            NotFound.Ignore.ToString().ShouldEqual("ignore");
        }

        [Test]
        public void ExceptionShouldHaveCorrectValue()
        {
            NotFound.Exception.ToString().ShouldEqual("exception");
        }
    }
}