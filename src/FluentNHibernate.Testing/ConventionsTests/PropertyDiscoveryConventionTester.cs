using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class PropertyDiscoveryConventionTester
    {
        private PropertyDiscoveryConvention convention;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new PropertyDiscoveryConvention(conventionFinder);
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

            convention.Apply(MockRepository.GenerateStub<IProperty>());
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

            convention.Apply(property);

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

            convention.Apply(property);

            // each convention gets Apply called for any properties it returned true for Accept
            conventions[0].AssertWasCalled(x => x.Apply(property));
            conventions[1].AssertWasNotCalled(x => x.Apply(property));
        }
    }
}