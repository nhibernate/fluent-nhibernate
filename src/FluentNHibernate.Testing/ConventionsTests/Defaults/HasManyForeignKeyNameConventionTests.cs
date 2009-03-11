using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class HasManyForeignKeyNameConventionTests
    {
        private HasManyForeignKeyNameConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new HasManyForeignKeyNameConvention();
        }

        [Test]
        public void ShouldAcceptIfNoColumnNameSet()
        {
            var target = MockRepository.GenerateStub<IOneToManyPart>();

            target.Stub(x => x.ColumnName)
                .Return(null);

            convention.Accept(target)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptIfColumnNameSet()
        {
            var target = MockRepository.GenerateStub<IOneToManyPart>();

            target.Stub(x => x.ColumnName)
                .Return("column_name");

            convention.Accept(target)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldSetKeyColumnName()
        {
            var target = MockRepository.GenerateMock<IOneToManyPart>();

            target.Stub(x => x.ParentType)
                .Return(typeof(ExampleClass));

            convention.Apply(target);

            target.AssertWasCalled(x => x.WithKeyColumn("ExampleClass_id"));
        }
    }
}