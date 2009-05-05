using System.Collections.Generic;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class ClassMappingPartDiscoveryConventionTester
    {
        private ClassMappingPartDiscoveryConvention convention;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new ClassMappingPartDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ApplyFindsConventions()
        {
            var cm = MockRepository.GenerateStub<IClassMap>();

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
            var cm = MockRepository.GenerateStub<IClassMap>();

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
            var cm = MockRepository.GenerateStub<IClassMap>();
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

        [Test]
        public void ShouldApplyToProperties()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = new ClassMap<Record>();
            var property = cm.Map(x => x.Name);

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(true);

            convention.Apply(cm);
            
            conventions[0].AssertWasCalled(x => x.Apply(property));
        }

        [Test]
        public void ShouldApplyToComponents()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = new ClassMap<PropertyTarget>();
            var component = cm.Component(x => x.Component, c => { });

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(true);

            convention.Apply(cm);

            conventions[0].AssertWasCalled(x => x.Apply(component));
        }

        [Test]
        public void ShouldApplyToDynamicComponents()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = new ClassMap<PropertyTarget>();
            var dynamicComponent = cm.DynamicComponent(x => x.ExtensionData, c => { });

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(true);

            convention.Apply(cm);

            conventions[0].AssertWasCalled(x => x.Apply(dynamicComponent));
        }

        [Test]
        public void ShouldApplyToHasMany()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = new ClassMap<OneToManyTarget>();
            var oneToMany = cm.HasMany(x => x.BagOfChildren);

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(true);

            convention.Apply(cm);

            conventions[0].AssertWasCalled(x => x.Apply(oneToMany));
        }

        [Test]
        public void ShouldApplyToHasManyToMany()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = new ClassMap<ManyToManyTarget>();
            var manyToMany = cm.HasManyToMany(x => x.BagOfChildren);

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(true);

            convention.Apply(cm);

            conventions[0].AssertWasCalled(x => x.Apply(manyToMany));
        }

        [Test]
        public void ShouldApplyToHasOne()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = new ClassMap<PropertyTarget>();
            var one = cm.HasOne(x => x.Reference);

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(true);

            convention.Apply(cm);

            conventions[0].AssertWasCalled(x => x.Apply(one));
        }

        [Test]
        public void ShouldApplyToReferences()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = new ClassMap<PropertyTarget>();
            var references = cm.References(x => x.Reference);

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(true);

            convention.Apply(cm);

            conventions[0].AssertWasCalled(x => x.Apply(references));
        }

        [Test]
        public void ShouldApplyToSubClasses()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = new ClassMap<Record>();
            SubClassPart<ChildRecord> subclass = null;
            
            cm.DiscriminateSubClassesOnColumn("column")
                .SubClass<ChildRecord>(c => { subclass = c; });

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(true);

            convention.Apply(cm);

            conventions[0].AssertWasCalled(x => x.Apply(subclass));
        }

        [Test]
        public void ShouldApplyToJoinedSubclasses()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<IMappingPartConvention>()
            };
            var cm = new ClassMap<SuperRecord>();
            JoinedSubClassPart<ChildRecord> subclass = null;
            
            cm.JoinedSubClass<ChildRecord>("column", sc => { subclass = sc; });

            conventionFinder.Stub(x => x.Find<IMappingPartConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(true);

            convention.Apply(cm);

            conventions[0].AssertWasCalled(x => x.Apply(subclass));
        }
    }
}