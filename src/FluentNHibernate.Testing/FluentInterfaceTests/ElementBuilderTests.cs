using FluentNHibernate.Mapping.Builders;
using NUnit.Framework;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ElementBuilderTests
    {
        [Test]
        public void CanSetLength()
        {
            var elementMapping = new ElementMapping();
            var part = new ElementBuilder(elementMapping);
            part.Length(50);

            elementMapping.Length.ShouldEqual(50);
        }

        [Test]
        public void CanSetFormula()
        {
            var elementMapping = new ElementMapping();
            var part = new ElementBuilder(elementMapping);
            part.Formula("formula");

            elementMapping.Formula.ShouldEqual("formula");
        }

        [Test]
        public void CanSetPrecision()
        {
            var elementMapping = new ElementMapping();
            var part = new ElementBuilder(elementMapping);
            part.Precision(10);

            elementMapping.Precision.ShouldEqual(10);
        }
    }
}
