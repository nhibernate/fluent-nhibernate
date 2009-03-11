using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class DefaultPropertyConventionTester
    {
        private DefaultPropertyConvention convention;
        private IConventionFinder conventionFinder;
        private readonly ConventionOverrides Overrides = new ConventionOverrides();

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new DefaultPropertyConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIPropertys()
        {
            convention.Accept(MockRepository.GenerateStub<IProperty>())
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptAnyOtherIMappingParts()
        {
            convention.Accept(MockRepository.GenerateStub<IOneToManyPart>())
                .ShouldBeFalse();
        }

        [Test]
        public void ApplyFindsConventions()
        {
            conventionFinder.Stub(x => x.Find<IPropertyConvention>())
                .Return(new IPropertyConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IProperty>(), Overrides);
            conventionFinder.AssertWasCalled(x => x.Find<IPropertyConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IPropertyConvention>(),
                MockRepository.GenerateMock<IPropertyConvention>()
            };
            var property = MockRepository.GenerateStub<IProperty>();

            conventionFinder.Stub(x => x.Find<IPropertyConvention>())
                .Return(conventions);

            convention.Apply(property, Overrides);

            conventions[0].AssertWasCalled(x => x.Accept(property));
            conventions[1].AssertWasCalled(x => x.Accept(property));
        }

        [Test]
        public void ApplyAppliesAllAcceptedConventions()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IPropertyConvention>(),
                MockRepository.GenerateMock<IPropertyConvention>()
            };
            var property = MockRepository.GenerateStub<IProperty>();

            conventionFinder.Stub(x => x.Find<IPropertyConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(property)).Return(true);
            conventions[1].Stub(x => x.Accept(property)).Return(false);

            convention.Apply(property, Overrides);

            // each convention gets Apply called for any properties it returned true for Accept
            conventions[0].AssertWasCalled(x => x.Apply(property, Overrides));
            conventions[1].AssertWasNotCalled(x => x.Apply(property, Overrides));
        }
    }
}