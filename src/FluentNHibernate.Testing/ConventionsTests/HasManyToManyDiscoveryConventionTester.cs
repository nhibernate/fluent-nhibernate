using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class HasManyToManyDiscoveryConventionTester
    {
        private HasManyToManyDiscoveryConvention convention;
        private IConventionFinder conventionFinder;
        private readonly ConventionOverrides Overrides = new ConventionOverrides();

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new HasManyToManyDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIManyToManyParts()
        {
            convention.Accept(MockRepository.GenerateStub<IManyToManyPart>())
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptAnyOtherIRelationships()
        {
            convention.Accept(MockRepository.GenerateStub<IOneToManyPart>())
                .ShouldBeFalse();
        }

        [Test]
        public void ApplyFindsConventions()
        {
            conventionFinder.Stub(x => x.Find<IHasManyToManyConvention>())
                .Return(new IHasManyToManyConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IManyToManyPart>(), Overrides);
            conventionFinder.AssertWasCalled(x => x.Find<IHasManyToManyConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IHasManyToManyConvention>(),
                MockRepository.GenerateMock<IHasManyToManyConvention>()
            };
            var relationship = MockRepository.GenerateStub<IManyToManyPart>();

            conventionFinder.Stub(x => x.Find<IHasManyToManyConvention>())
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
                MockRepository.GenerateMock<IHasManyToManyConvention>(),
                MockRepository.GenerateMock<IHasManyToManyConvention>()
            };
            var relationship = MockRepository.GenerateStub<IManyToManyPart>();

            conventionFinder.Stub(x => x.Find<IHasManyToManyConvention>())
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