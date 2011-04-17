using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection.ValueTypes
{
    [TestFixture]
    public class AccessTests
    {
        [Test]
        public void ShouldBeEqualToAnotherTheSame()
        {
            Access.Field.ShouldEqual(Access.Field);
        }

        [Test]
        public void ShouldNotBeEqualToADifferentOne()
        {
            Access.Field.ShouldNotEqual(Access.Property);
        }

        [Test]
        public void FieldShouldHaveCorrectValue()
        {
            Access.Field.ToString().ShouldEqual("field");
        }

        [Test]
        public void BackFieldShouldHaveCorrectValue()
        {
            Access.BackField.ToString().ShouldEqual("backfield");
        }

        [Test]
        public void PropertyShouldHaveCorrectValue()
        {
            Access.Property.ToString().ShouldEqual("property");
        }

        [Test]
        public void LowerCaseFieldShouldHaveCorrectValue()
        {
            Access.LowerCaseField().ToString().ShouldEqual("field.lowercase");
        }

        [Test]
        public void UnderscoreLowerCaseFieldShouldHaveCorrectValue()
        {
            Access.LowerCaseField(LowerCasePrefix.Underscore).ToString().ShouldEqual("field.lowercase-underscore");
        }

        [Test]
        public void CamelCaseFieldShouldHaveCorrectValue()
        {
            Access.CamelCaseField().ToString().ShouldEqual("field.camelcase");
        }

        [Test]
        public void UnderscoreCamelCaseFieldShouldHaveCorrectValue()
        {
            Access.CamelCaseField(CamelCasePrefix.Underscore).ToString().ShouldEqual("field.camelcase-underscore");
        }

        [Test]
        public void UnderscorePascalCaseFieldShouldHaveCorrectValue()
        {
            Access.PascalCaseField(PascalCasePrefix.Underscore).ToString().ShouldEqual("field.pascalcase-underscore");
        }

        [Test]
        public void MPascalCaseFieldShouldHaveCorrectValue()
        {
            Access.PascalCaseField(PascalCasePrefix.M).ToString().ShouldEqual("field.pascalcase-m");
        }

        [Test]
        public void MUnderscorePascalCaseFieldShouldHaveCorrectValue()
        {
            Access.PascalCaseField(PascalCasePrefix.MUnderscore).ToString().ShouldEqual("field.pascalcase-m-underscore");
        }

        //

        [Test]
        public void ReadOnlyPropertyThroughLowerCaseFieldShouldHaveCorrectValue()
        {
            Access.ReadOnlyPropertyThroughLowerCaseField().ToString().ShouldEqual("nosetter.lowercase");
        }

        [Test]
        public void ReadOnlyPropertyThroughUnderscoreLowerCaseFieldShouldHaveCorrectValue()
        {
            Access.ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix.Underscore).ToString().ShouldEqual("nosetter.lowercase-underscore");
        }

        [Test]
        public void ReadOnlyPropertyThroughCamelCaseFieldShouldHaveCorrectValue()
        {
            Access.ReadOnlyPropertyThroughCamelCaseField().ToString().ShouldEqual("nosetter.camelcase");
        }

        [Test]
        public void ReadOnlyPropertyThroughUnderscoreCamelCaseFieldShouldHaveCorrectValue()
        {
            Access.ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix.Underscore).ToString().ShouldEqual("nosetter.camelcase-underscore");
        }

        [Test]
        public void ReadOnlyPropertyThroughUnderscorePascalCaseFieldShouldHaveCorrectValue()
        {
            Access.ReadOnlyPropertyThroughPascalCaseField(PascalCasePrefix.Underscore).ToString().ShouldEqual("nosetter.pascalcase-underscore");
        }

        [Test]
        public void ReadOnlyPropertyThroughMPascalCaseFieldShouldHaveCorrectValue()
        {
            Access.ReadOnlyPropertyThroughPascalCaseField(PascalCasePrefix.M).ToString().ShouldEqual("nosetter.pascalcase-m");
        }

        [Test]
        public void ReadOnlyPropertyThroughMUnderscorePascalCaseFieldShouldHaveCorrectValue()
        {
            Access.ReadOnlyPropertyThroughPascalCaseField(PascalCasePrefix.MUnderscore).ToString().ShouldEqual("nosetter.pascalcase-m-underscore");
        }

        [Test]
        public void NoOpShouldHaveCorrectValue()
        {
            Access.NoOp.ToString().ShouldEqual("noop");
        }

        [Test]
        public void NoneShouldHaveCorrectValue()
        {
            Access.None.ToString().ShouldEqual("none");
        }
    }
}
