using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class JoinDiscoveryConventionTester
    {
        private JoinDiscoveryConvention convention;
        private IConventionFinder conventionFinder;
        private readonly ConventionOverrides Overrides = new ConventionOverrides();

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new JoinDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIJoins()
        {
            convention.Accept(MockRepository.GenerateStub<IJoin>())
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptAnyOtherIMappingParts()
        {
            convention.Accept(MockRepository.GenerateStub<IProperty>())
                .ShouldBeFalse();
        }

        [Test]
        public void ApplyFindsConventions()
        {
            conventionFinder.Stub(x => x.Find<IJoinConvention>())
                .Return(new IJoinConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IJoin>(), Overrides);
            conventionFinder.AssertWasCalled(x => x.Find<IJoinConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IJoinConvention>(),
                MockRepository.GenerateMock<IJoinConvention>()
            };
            var id = MockRepository.GenerateStub<IJoin>();

            conventionFinder.Stub(x => x.Find<IJoinConvention>())
                .Return(conventions);

            convention.Apply(id, Overrides);

            conventions[0].AssertWasCalled(x => x.Accept(id));
            conventions[1].AssertWasCalled(x => x.Accept(id));
        }

        [Test]
        public void ApplyAppliesAllAcceptedConventions()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IJoinConvention>(),
                MockRepository.GenerateMock<IJoinConvention>()
            };
            var id = MockRepository.GenerateStub<IJoin>();

            conventionFinder.Stub(x => x.Find<IJoinConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(id)).Return(true);
            conventions[1].Stub(x => x.Accept(id)).Return(false);

            convention.Apply(id, Overrides);

            // each convention gets Apply called for any properties it returned true for Accept
            conventions[0].AssertWasCalled(x => x.Apply(id, Overrides));
            conventions[1].AssertWasNotCalled(x => x.Apply(id, Overrides));
        }
    }
}