using FluentNHibernate.Conventions.Inspections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection.ValueTypes
{
    [TestFixture]
    public class CascadeTests
    {
        [Test]
        public void ShouldBeEqualToAnotherTheSame()
        {
            Cascade.None.ShouldEqual(Cascade.None);
        }

        [Test]
        public void ShouldNotBeEqualToADifferentOne()
        {
            Cascade.None.ShouldNotEqual(Cascade.All);
        }

        [Test]
        public void NoneShouldHaveCorrectValue()
        {
            Cascade.None.ToString().ShouldEqual("none");
        }

        [Test]
        public void AllShouldHaveCorrectValue()
        {
            Cascade.All.ToString().ShouldEqual("all");
        }

        [Test]
        public void AllDeleteOrphanShouldHaveCorrectValue()
        {
            Cascade.AllDeleteOrphan.ToString().ShouldEqual("all-delete-orphan");
        }

        [Test]
        public void VersionShouldHaveCorrectValue()
        {
            Cascade.Delete.ToString().ShouldEqual("delete");
        }

        [Test]
        public void DirtyShouldHaveCorrectValue()
        {
            Cascade.SaveUpdate.ToString().ShouldEqual("save-update");
        }
    }
}