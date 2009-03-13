using System.Collections.Generic;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class ComponentDiscoveryConventionTester
    {
        private ComponentDiscoveryConvention convention;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new ComponentDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptIComponents()
        {
            convention.Accept(MockRepository.GenerateStub<IComponent>())
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptAnyOtherIMappingParts()
        {
            convention.Accept(MockRepository.GenerateStub<IJoin>())
                .ShouldBeFalse();
        }

        [Test]
        public void ApplyFindsConventions()
        {
            conventionFinder.Stub(x => x.Find<IComponentConvention>())
                .Return(new IComponentConvention[] { });

            convention.Apply(MockRepository.GenerateStub<IComponent>());
            conventionFinder.AssertWasCalled(x => x.Find<IComponentConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IComponentConvention>(),
                MockRepository.GenerateMock<IComponentConvention>()
            };
            var id = MockRepository.GenerateStub<IComponent>();

            conventionFinder.Stub(x => x.Find<IComponentConvention>())
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
                MockRepository.GenerateMock<IComponentConvention>(),
                MockRepository.GenerateMock<IComponentConvention>()
            };
            var id = MockRepository.GenerateStub<IComponent>();

            conventionFinder.Stub(x => x.Find<IComponentConvention>())
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