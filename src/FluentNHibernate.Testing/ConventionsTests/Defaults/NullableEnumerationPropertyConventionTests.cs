using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class NullableEnumerationPropertyConventionTests
    {
        private NullableEnumerationPropertyConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new NullableEnumerationPropertyConvention();
        }

        [Test]
        public void ShouldAcceptNullableEnumProperties()
        {
            var property = MockRepository.GenerateStub<IProperty>();

            property.Stub(x => x.PropertyType)
                .Return(typeof(TestEnum?));

            convention.Accept(property)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptNonEnumNullableProperties()
        {
            var property = MockRepository.GenerateStub<IProperty>();

            property.Stub(x => x.PropertyType)
                .Return(typeof(int?));

            convention.Accept(property)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldntAcceptNonNullableEnumProperties()
        {
            var property = MockRepository.GenerateStub<IProperty>();

            property.Stub(x => x.PropertyType)
                .Return(typeof(TestEnum));

            convention.Accept(property)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldSetType()
        {
            var property = MockRepository.GenerateMock<IProperty>();

            property.Stub(x => x.PropertyType)
                .Return(typeof(TestEnum?));

            convention.Apply(property);

            property.AssertWasCalled(x => x.CustomTypeIs(typeof(GenericEnumMapper<TestEnum>)));
        }

        [Test]
        public void ShouldSetNullable()
        {
            var property = MockRepository.GenerateMock<IProperty>();

            property.Stub(x => x.PropertyType)
                .Return(typeof(TestEnum?));

            convention.Apply(property);

            property.AssertWasCalled(x => x.Nullable());
        }

        private enum TestEnum
        {
            One,
            Two
        }
    }
}