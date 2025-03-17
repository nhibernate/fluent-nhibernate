using System.Collections.Generic;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class HasManyToManyMethodAccessorBackingFieldNamingConventionTests
    {
        private HasManyToManyMethodAccessorBackingFieldNamingConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new HasManyToManyMethodAccessorBackingFieldNamingConvention();
        }

        [Test]
        public void ShouldAcceptMethods()
        {
            var target = MockRepository.GenerateStub<IManyToManyPart>();

            target.Stub(x => x.IsMethodAccess)
                .Return(true);

            convention.Accept(target)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptProperties()
        {
            var target = MockRepository.GenerateStub<IManyToManyPart>();

            target.Stub(x => x.IsMethodAccess)
                .Return(false);

            convention.Accept(target)
                .ShouldBeFalse();
        }

        [Test, Ignore]
        public void ShouldSetName()
        {
            //var target = MockRepository.GenerateMock<IManyToManyPart>();

            //target.Stub(x => x.Member)
            //    .Return(new DummyMethodInfo("GetExamples", typeof(IList<ExampleClass>)));

            //convention.Apply(target);

            //target.AssertWasCalled(x => x.SetAttribute("name", "Examples"));
        }
    }
}
