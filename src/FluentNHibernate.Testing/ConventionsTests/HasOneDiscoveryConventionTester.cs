using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class HasOneDiscoveryConventionTester
    {
        private HasOneDiscoveryConvention convention;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new HasOneDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIOneToOneParts()
        {
            convention.Accept(MockRepository.GenerateStub<IOneToOnePart>())
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptAnyOtherIRelationships()
        {
            convention.Accept(MockRepository.GenerateStub<IManyToManyPart>())
                .ShouldBeFalse();
        }

        [Test]
        public void ApplyFindsConventions()
        {
            conventionFinder.Stub(x => x.Find<IHasOneConvention>())
                .Return(new IHasOneConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IOneToOnePart>());
            conventionFinder.AssertWasCalled(x => x.Find<IHasOneConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IHasOneConvention>(),
                MockRepository.GenerateMock<IHasOneConvention>()
            };
            var relationship = MockRepository.GenerateStub<IOneToOnePart>();

            conventionFinder.Stub(x => x.Find<IHasOneConvention>())
                .Return(conventions);

            convention.Apply(relationship);

            conventions[0].AssertWasCalled(x => x.Accept(relationship));
            conventions[1].AssertWasCalled(x => x.Accept(relationship));
        }

        [Test]
        public void ApplyAppliesAllAcceptedConventions()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IHasOneConvention>(),
                MockRepository.GenerateMock<IHasOneConvention>()
            };
            var relationship = MockRepository.GenerateStub<IOneToOnePart>();

            conventionFinder.Stub(x => x.Find<IHasOneConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(relationship)).Return(true);
            conventions[1].Stub(x => x.Accept(relationship)).Return(false);

            convention.Apply(relationship);

            // each convention gets Apply called for any properties it returned true for Accept
            conventions[0].AssertWasCalled(x => x.Apply(relationship));
            conventions[1].AssertWasNotCalled(x => x.Apply(relationship));
        }
    }
}