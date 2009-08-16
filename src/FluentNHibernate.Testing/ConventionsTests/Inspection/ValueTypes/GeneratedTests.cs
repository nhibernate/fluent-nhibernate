using FluentNHibernate.Conventions.Inspections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection.ValueTypes
{
    [TestFixture]
    public class GeneratedTests
    {
        [Test]
        public void ShouldBeEqualToAnotherTheSame()
        {
            Generated.Never.ShouldEqual(Generated.Never);
        }

        [Test]
        public void ShouldNotBeEqualToADifferentOne()
        {
            Generated.Never.ShouldNotEqual(Generated.Insert);
        }

        [Test]
        public void NeverShouldHaveCorrectValue()
        {
            Generated.Never.ToString().ShouldEqual("never");
        }

        [Test]
        public void InsertShouldHaveCorrectValue()
        {
            Generated.Insert.ToString().ShouldEqual("insert");
        }

        [Test]
        public void AlwaysShouldHaveCorrectValue()
        {
            Generated.Always.ToString().ShouldEqual("always");
        }
    }
}