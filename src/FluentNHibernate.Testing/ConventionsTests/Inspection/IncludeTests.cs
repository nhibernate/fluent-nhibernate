using FluentNHibernate.Conventions.Inspections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection.ValueTypes
{
    [TestFixture]
    public class IncludeTests
    {
        [Test]
        public void ShouldBeEqualToAnotherTheSame()
        {
            Include.All.ShouldEqual(Include.All);
        }

        [Test]
        public void ShouldNotBeEqualToADifferentOne()
        {
            Include.All.ShouldNotEqual(Include.NonLazy);
        }

        [Test]
        public void AllShouldHaveCorrectValue()
        {
            Include.All.ToString().ShouldEqual("all");
        }

        [Test]
        public void NonLazyShouldHaveCorrectValue()
        {
            Include.NonLazy.ToString().ShouldEqual("non-lazy");
        }

        [Test]
        public void CustomShouldHaveCorrectValue()
        {
            Include.Custom("xxx").ToString().ShouldEqual("xxx");
        }
    }
}