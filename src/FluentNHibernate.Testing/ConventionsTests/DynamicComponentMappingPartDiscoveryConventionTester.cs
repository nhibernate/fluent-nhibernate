using System.Collections.Generic;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class DynamicComponentMappingPartDiscoveryConventionTester
    {
        private DynamicComponentMappingPartDiscoveryConvention convention;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new DynamicComponentMappingPartDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ApplyFindsConventions()
        {
            var cm = MockRepository.GenerateStub<IDynamicComponent>();

            cm.Stub(x => x.Parts).Return(new List<IMappingPart>());

            convention.Apply(cm);
            conventionFinder.AssertWasCalled(x => x.Find<IMappingPartConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventions()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>(),
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = MockRepository.GenerateStub<IDynamicComponent>();

            cm.Stub(x => x.Parts)
                .Return(new[]
                {
                    MockRepository.GenerateStub<IMappingPart>(),
                    MockRepository.GenerateStub<IMappingPart>(),
                    MockRepository.GenerateStub<IMappingPart>()
                });

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            convention.Apply(cm);

            // each convention gets Accept called
            foreach (var part in cm.Parts)
            {
                var p = part;
                conventions[0].AssertWasCalled(x => x.Accept(p));
                conventions[1].AssertWasCalled(x => x.Accept(p));
            }
        }

        [Test]
        public void ApplyAppliesAllAcceptedConvention()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>(),
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = MockRepository.GenerateStub<IDynamicComponent>();
            var part = MockRepository.GenerateStub<IMappingPart>();

            cm.Stub(x => x.Parts).Return(new[] { part });

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(part)).Return(true);
            conventions[1].Stub(x => x.Accept(part)).Return(false);

            convention.Apply(cm);

            // each convention gets Apply called for any parts it returned true for Accept
            conventions[0].AssertWasCalled(x => x.Apply(part));
            conventions[1].AssertWasNotCalled(x => x.Apply(part));
        }
    }
}