using FluentNHibernate.Conventions.Inspections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection.ValueTypes
{
    [TestFixture]
    public class PolymorphismTests
    {
        [Test]
        public void ShouldBeEqualToAnotherTheSame()
        {
            Polymorphism.Implicit.ShouldEqual(Polymorphism.Implicit);
        }

        [Test]
        public void ShouldNotBeEqualToADifferentOne()
        {
            Polymorphism.Implicit.ShouldNotEqual(Polymorphism.Explicit);
        }

        [Test]
        public void ImplicitShouldHaveCorrectValue()
        {
            Polymorphism.Implicit.ToString().ShouldEqual("implicit");
        }

        [Test]
        public void ExplicitShouldHaveCorrectValue()
        {
            Polymorphism.Explicit.ToString().ShouldEqual("explicit");
        }
    }
}