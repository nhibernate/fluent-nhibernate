using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class JoinedSubclassDiscoveryConventionTester
    {
        private JoinedSubclassDiscoveryConvention convention;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new JoinedSubclassDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIJoinedSubclasses()
        {
            convention.Accept(MockRepository.GenerateStub<IJoinedSubclass>())
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptAnyOtherIMappingParts()
        {
            convention.Accept(MockRepository.GenerateStub<ISubclass>())
                .ShouldBeFalse();
        }

        [Test]
        public void ApplyFindsConventions()
        {
            conventionFinder.Stub(x => x.Find<IJoinedSubclassConvention>())
                .Return(new IJoinedSubclassConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IJoinedSubclass>());
            conventionFinder.AssertWasCalled(x => x.Find<IJoinedSubclassConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IJoinedSubclassConvention>(),
                MockRepository.GenerateMock<IJoinedSubclassConvention>()
            };
            var id = MockRepository.GenerateStub<IJoinedSubclass>();

            conventionFinder.Stub(x => x.Find<IJoinedSubclassConvention>())
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
                MockRepository.GenerateMock<IJoinedSubclassConvention>(),
                MockRepository.GenerateMock<IJoinedSubclassConvention>()
            };
            var id = MockRepository.GenerateStub<IJoinedSubclass>();

            conventionFinder.Stub(x => x.Find<IJoinedSubclassConvention>())
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