using FluentNHibernate.Conventions.Inspections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection.ValueTypes
{
    [TestFixture]
    public class OptimisticLockTests
    {
        [Test]
        public void ShouldBeEqualToAnotherTheSame()
        {
            OptimisticLock.None.ShouldEqual(OptimisticLock.None);
        }

        [Test]
        public void ShouldNotBeEqualToADifferentOne()
        {
            OptimisticLock.None.ShouldNotEqual(OptimisticLock.All);
        }

        [Test]
        public void NoneShouldHaveCorrectValue()
        {
            OptimisticLock.None.ToString().ShouldEqual("none");
        }

        [Test]
        public void AllShouldHaveCorrectValue()
        {
            OptimisticLock.All.ToString().ShouldEqual("all");
        }

        [Test]
        public void VersionShouldHaveCorrectValue()
        {
            OptimisticLock.Version.ToString().ShouldEqual("version");
        }

        [Test]
        public void DirtyShouldHaveCorrectValue()
        {
            OptimisticLock.Dirty.ToString().ShouldEqual("dirty");
        }
    }
}