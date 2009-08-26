using FluentNHibernate.Conventions.Inspections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection.ValueTypes
{
    [TestFixture]
    public class SchemaActionTests
    {
        [Test]
        public void ShouldBeEqualToAnotherTheSame()
        {
            SchemaAction.None.ShouldEqual(SchemaAction.None);
        }

        [Test]
        public void ShouldNotBeEqualToADifferentOne()
        {
            SchemaAction.None.ShouldNotEqual(SchemaAction.All);
        }

        [Test]
        public void NoneShouldHaveCorrectValue()
        {
            SchemaAction.None.ToString().ShouldEqual("none");
        }

        [Test]
        public void AllShouldHaveCorrectValue()
        {
            SchemaAction.All.ToString().ShouldEqual("all");
        }

        [Test]
        public void DropShouldHaveCorrectValue()
        {
            SchemaAction.Drop.ToString().ShouldEqual("drop");
        }

        [Test]
        public void ExportShouldHaveCorrectValue()
        {
            SchemaAction.Export.ToString().ShouldEqual("export");
        }

        [Test]
        public void UpdateShouldHaveCorrectValue()
        {
            SchemaAction.Update.ToString().ShouldEqual("update");
        }

        [Test]
        public void ValidateShouldHaveCorrectValue()
        {
            SchemaAction.Validate.ToString().ShouldEqual("validate");
        }

        [Test]
        public void CustomShouldHaveCorrectValue()
        {
            SchemaAction.Custom("a,b").ToString().ShouldEqual("a,b");
        }
    }
}