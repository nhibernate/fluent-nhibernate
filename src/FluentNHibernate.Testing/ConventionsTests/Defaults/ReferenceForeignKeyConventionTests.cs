using System;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class ReferenceForeignKeyConventionTests
    {
        private DefaultForeignKeyConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new DefaultForeignKeyConvention();
        }

        [Test]
        public void ShouldAcceptIfNoColumnNameSet()
        {
            //var target = MockRepository.GenerateStub<IManyToOnePart>();

            //target.Stub(x => x.GetColumnName())
            //    .Return(null);

            //convention.Accept(target)
            //    .ShouldBeTrue();
            throw new NotImplementedException("Awaiting convention DSL");
        }

        [Test]
        public void ShouldntAcceptIfColumnNameSet()
        {
            //var target = MockRepository.GenerateStub<IManyToOnePart>();

            //target.Stub(x => x.GetColumnName())
            //    .Return("column_name");

            //convention.Accept(target)
            //    .ShouldBeFalse();
            throw new NotImplementedException("Awaiting convention DSL");
        }

        [Test]
        public void ShouldSetKeyColumnName()
        {
            var target = MockRepository.GenerateMock<IManyToOnePart>();

            target.Stub(x => x.Property)
                .Return(new DummyPropertyInfo("Example", typeof(ExampleClass)));

            convention.Apply(target);

            target.AssertWasCalled(x => x.ColumnName("Example_id"));
        }
    }
}