using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class DynamicComponentDiscoveryConventionTester
    {
        private DynamicComponentDiscoveryConvention convention;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new DynamicComponentDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIDynamicComponents()
        {
            convention.Accept(MockRepository.GenerateStub<IDynamicComponent>())
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptAnyOtherIMappingParts()
        {
            convention.Accept(MockRepository.GenerateStub<IComponent>())
                .ShouldBeFalse();
        }

        [Test]
        public void ApplyFindsConventions()
        {
            conventionFinder.Stub(x => x.Find<IDynamicComponentConvention>())
                .Return(new IDynamicComponentConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IDynamicComponent>());
            conventionFinder.AssertWasCalled(x => x.Find<IDynamicComponentConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IDynamicComponentConvention>(),
                MockRepository.GenerateMock<IDynamicComponentConvention>()
            };
            var id = MockRepository.GenerateStub<IDynamicComponent>();

            conventionFinder.Stub(x => x.Find<IDynamicComponentConvention>())
                .Return(conventions);

            convention.Apply(id);

            conventions[0].AssertWasCalled(x => x.Accept(id));
            conventions[1].AssertWasCalled(x => x.Accept(id));
        }

        [Test]
        public void ApplyAppliesAllAcceptedConventions()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IDynamicComponentConvention>(),
                MockRepository.GenerateMock<IDynamicComponentConvention>()
            };
            var id = MockRepository.GenerateStub<IDynamicComponent>();

            conventionFinder.Stub(x => x.Find<IDynamicComponentConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(id)).Return(true);
            conventions[1].Stub(x => x.Accept(id)).Return(false);

            convention.Apply(id);

            // each convention gets Apply called for any properties it returned true for Accept
            conventions[0].AssertWasCalled(x => x.Apply(id));
            conventions[1].AssertWasNotCalled(x => x.Apply(id));
        }
    }
}