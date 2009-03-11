using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class HasManyDiscoveryConventionTester
    {
        private HasManyDiscoveryConvention convention;
        private IConventionFinder conventionFinder;
        private readonly ConventionOverrides Overrides = new ConventionOverrides();

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new HasManyDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIOneToManyParts()
        {
            convention.Accept(MockRepository.GenerateStub<IOneToManyPart>())
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
            conventionFinder.Stub(x => x.Find<IHasManyConvention>())
                .Return(new IHasManyConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IOneToManyPart>(), Overrides);
            conventionFinder.AssertWasCalled(x => x.Find<IHasManyConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IHasManyConvention>(),
                MockRepository.GenerateMock<IHasManyConvention>()
            };
            var relationship = MockRepository.GenerateStub<IOneToManyPart>();

            conventionFinder.Stub(x => x.Find<IHasManyConvention>())
                .Return(conventions);

            convention.Apply(relationship, Overrides);

            conventions[0].AssertWasCalled(x => x.Accept(relationship));
            conventions[1].AssertWasCalled(x => x.Accept(relationship));
        }

        [Test]
        public void ApplyAppliesAllAcceptedConventions()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IHasManyConvention>(),
                MockRepository.GenerateMock<IHasManyConvention>()
            };
            var relationship = MockRepository.GenerateStub<IOneToManyPart>();

            conventionFinder.Stub(x => x.Find<IHasManyConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(relationship)).Return(true);
            conventions[1].Stub(x => x.Accept(relationship)).Return(false);

            convention.Apply(relationship, Overrides);

            // each convention gets Apply called for any properties it returned true for Accept
            conventions[0].AssertWasCalled(x => x.Apply(relationship, Overrides));
            conventions[1].AssertWasNotCalled(x => x.Apply(relationship, Overrides));
        }
    }
}