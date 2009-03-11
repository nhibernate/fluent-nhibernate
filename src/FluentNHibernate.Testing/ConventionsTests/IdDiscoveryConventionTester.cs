using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class IdDiscoveryConventionTester
    {
        private IdDiscoveryConvention convention;
        private IConventionFinder conventionFinder;
        private readonly ConventionOverrides Overrides = new ConventionOverrides();

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new IdDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIIdentityParts()
        {
            convention.Accept(MockRepository.GenerateStub<IIdentityPart>())
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
            conventionFinder.Stub(x => x.Find<IIdConvention>())
                .Return(new IIdConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IIdentityPart>(), Overrides);
            conventionFinder.AssertWasCalled(x => x.Find<IIdConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IIdConvention>(),
                MockRepository.GenerateMock<IIdConvention>()
            };
            var id = MockRepository.GenerateStub<IIdentityPart>();

            conventionFinder.Stub(x => x.Find<IIdConvention>())
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
                MockRepository.GenerateMock<IIdConvention>(),
                MockRepository.GenerateMock<IIdConvention>()
            };
            var id = MockRepository.GenerateStub<IIdentityPart>();

            conventionFinder.Stub(x => x.Find<IIdConvention>())
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