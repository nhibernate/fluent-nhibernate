using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class DefaultReferencesConventionTester
    {
        private DefaultReferencesConvention convention;
        private IConventionFinder conventionFinder;
        private readonly ConventionOverrides Overrides = new ConventionOverrides();

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new DefaultReferencesConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIOneToManyParts()
        {
            convention.Accept(MockRepository.GenerateStub<IManyToOnePart>())
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
            conventionFinder.Stub(x => x.Find<IReferencesConvention>())
                .Return(new IReferencesConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IManyToOnePart>(), Overrides);
            conventionFinder.AssertWasCalled(x => x.Find<IReferencesConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IReferencesConvention>(),
                MockRepository.GenerateMock<IReferencesConvention>()
            };
            var relationship = MockRepository.GenerateStub<IManyToOnePart>();

            conventionFinder.Stub(x => x.Find<IReferencesConvention>())
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
                MockRepository.GenerateMock<IReferencesConvention>(),
                MockRepository.GenerateMock<IReferencesConvention>()
            };
            var relationship = MockRepository.GenerateStub<IManyToOnePart>();

            conventionFinder.Stub(x => x.Find<IReferencesConvention>())
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