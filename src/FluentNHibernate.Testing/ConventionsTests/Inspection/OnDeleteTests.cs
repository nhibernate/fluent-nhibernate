using FluentNHibernate.Conventions.Inspections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection.ValueTypes
{
    [TestFixture]
    public class OnDeleteTests
    {
        [Test]
        public void ShouldBeEqualToAnotherTheSame()
        {
            OnDelete.Cascade.ShouldEqual(OnDelete.Cascade);
        }

        [Test]
        public void ShouldNotBeEqualToADifferentOne()
        {
            OnDelete.Cascade.ShouldNotEqual(OnDelete.NoAction);
        }

        [Test]
        public void CascadeShouldHaveCorrectValue()
        {
            OnDelete.Cascade.ToString().ShouldEqual("cascade");
        }

        [Test]
        public void NoActionShouldHaveCorrectValue()
        {
            OnDelete.NoAction.ToString().ShouldEqual("noaction");
        }
    }
}