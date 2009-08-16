using System;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class PropertyTypeConventionTests
    {
        private PropertyTypeConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new PropertyTypeConvention();
        }

        [Test]
        public void ShouldAcceptProperty()
        {
            var target = MockRepository.GenerateStub<IProperty>();

            target.Stub(x => x.PropertyType)
                .Return(typeof(int));

            convention.Accept(target)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptEnum()
        {
            var target = MockRepository.GenerateStub<IProperty>();

            target.Stub(x => x.PropertyType)
                .Return(typeof(TestEnum));

            convention.Accept(target)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldntAcceptIfTypeExplictlySet()
        {
            Assert.Fail("Awaiting convention DSL");
            //var target = MockRepository.GenerateStub<IProperty>();

            //target.Stub(x => x.HasAttribute("type"))
            //    .Return(true);

            //convention.Accept(target)
            //    .ShouldBeFalse();
        }

        [Test]
        public void ShouldntAcceptEnumWithExplicitType()
        {
            Assert.Fail("Awaiting convention DSL");
            //var target = MockRepository.GenerateStub<IProperty>();

            //target.Stub(x => x.PropertyType)
            //    .Return(typeof(TestEnum));
            //target.Stub(x => x.HasAttribute("type"))
            //    .Return(true);

            //convention.Accept(target)
            //    .ShouldBeFalse();
        }

        [Test]
        public void ShouldntAcceptPropertyWithExplicitType()
        {
            Assert.Fail("Awaiting convention DSL");
            //var target = MockRepository.GenerateStub<IProperty>();

            //target.Stub(x => x.PropertyType)
            //    .Return(typeof(int));
            //target.Stub(x => x.HasAttribute("type"))
            //    .Return(true);

            //convention.Accept(target)
            //    .ShouldBeFalse();
        }

        [Test]
        public void ShouldSetTypeWithShortNameForClrTypes()
        {
            var target = MockRepository.GenerateMock<IProperty>();

            target.Stub(x => x.PropertyType)
                .Return(typeof(String));

            convention.Apply(target);

            target.AssertWasCalled(x => x.CustomTypeIs(typeof(string)));
        }

        [Test]
        public void ShouldSetTypeWithAssemblyQualifiedNameForNonClrTypes()
        {
            var target = MockRepository.GenerateMock<IProperty>();

            target.Stub(x => x.PropertyType)
                .Return(typeof(ExampleClass));

            convention.Apply(target);

            target.AssertWasCalled(x => x.CustomTypeIs(typeof(ExampleClass)));
        }

        private enum TestEnum
        {
            One,
            Two
        }
    }
}