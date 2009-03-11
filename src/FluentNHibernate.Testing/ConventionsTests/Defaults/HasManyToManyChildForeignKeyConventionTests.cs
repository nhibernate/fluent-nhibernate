using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class HasManyToManyChildForeignKeyConventionTests
    {
        private HasManyToManyChildForeignKeyConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new HasManyToManyChildForeignKeyConvention();
        }

        [Test]
        public void ShouldAcceptIfNoColumnNameSet()
        {
            var target = MockRepository.GenerateStub<IManyToManyPart>();

            target.Stub(x => x.ChildKeyColumn)
                .Return(null);

            convention.Accept(target)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptIfColumnNameSet()
        {
            var target = MockRepository.GenerateStub<IManyToManyPart>();

            target.Stub(x => x.ChildKeyColumn)
                .Return("column_name");

            convention.Accept(target)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldSetKeyColumnName()
        {
            var target = MockRepository.GenerateMock<IManyToManyPart>();

            target.Stub(x => x.ChildType)
                .Return(typeof(ExampleClass));

            convention.Apply(target);

            target.AssertWasCalled(x => x.WithChildKeyColumn("ExampleClass_id"));
        }
    }
}