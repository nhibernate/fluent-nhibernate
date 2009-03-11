using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class RelationshipDiscoveryConventionTester
    {
        private RelationshipDiscoveryConvention convention;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new RelationshipDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIRelationsips()
        {
            convention.Accept(MockRepository.GenerateStub<IRelationship>())
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
            conventionFinder.Stub(x => x.Find<IRelationshipConvention>())
                .Return(new IRelationshipConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IRelationship>());
            conventionFinder.AssertWasCalled(x => x.Find<IRelationshipConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IRelationshipConvention>(),
                MockRepository.GenerateMock<IRelationshipConvention>()
            };
            var relationship = MockRepository.GenerateStub<IRelationship>();

            conventionFinder.Stub(x => x.Find<IRelationshipConvention>())
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
                MockRepository.GenerateMock<IRelationshipConvention>(),
                MockRepository.GenerateMock<IRelationshipConvention>()
            };
            var relationship = MockRepository.GenerateStub<IRelationship>();

            conventionFinder.Stub(x => x.Find<IRelationshipConvention>())
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