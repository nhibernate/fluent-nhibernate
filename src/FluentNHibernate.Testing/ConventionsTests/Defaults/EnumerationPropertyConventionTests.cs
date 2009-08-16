using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class EnumerationPropertyConventionTests
    {
        private EnumerationPropertyConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new EnumerationPropertyConvention();
        }

        [Test]
        public void ShouldAcceptEnumProperties()
        {
            var property = MockRepository.GenerateStub<IProperty>();

            property.Stub(x => x.PropertyType)
                .Return(typeof(TestEnum));

            convention.Accept(property)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptNonEnumProperties()
        {
            var property = MockRepository.GenerateStub<IProperty>();

            property.Stub(x => x.PropertyType)
                .Return(typeof(int));

            convention.Accept(property)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldntAcceptEnumPropertiesThatHaveExplicitType()
        {
            Assert.Fail("Awaiting convention DSL");
            //var property = MockRepository.GenerateStub<IProperty>();

            //property.Stub(x => x.PropertyType)
            //    .Return(typeof(TestEnum));
            //property.Stub(x => x.HasAttribute("type"))
            //    .Return(true);

            //convention.Accept(property)
            //    .ShouldBeFalse();
        }

        [Test]
        public void ShouldntAcceptNonEnumPropertiesThatHaveExplicitType()
        {
            Assert.Fail("Awaiting convention DSL");
            //var property = MockRepository.GenerateStub<IProperty>();

            //property.Stub(x => x.PropertyType)
            //    .Return(typeof(int));
            //property.Stub(x => x.HasAttribute("type"))
            //    .Return(true);

            //convention.Accept(property)
            //    .ShouldBeFalse();
        }

        [Test]
        public void ShouldSetType()
        {
            var property = MockRepository.GenerateMock<IProperty>();

            property.Stub(x => x.PropertyType)
                .Return(typeof(TestEnum));

            convention.Apply(property);

            property.AssertWasCalled(x => x.CustomTypeIs(typeof(GenericEnumMapper<TestEnum>)));
        }

        private enum TestEnum
        {
            One,
            Two
        }
    }
}
