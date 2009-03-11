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
        private ReferenceForeignKeyConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new ReferenceForeignKeyConvention();
        }

        [Test]
        public void ShouldAcceptIfNoColumnNameSet()
        {
            var target = MockRepository.GenerateStub<IManyToOnePart>();

            target.Stub(x => x.ColumnName)
                .Return(null);

            convention.Accept(target)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptIfColumnNameSet()
        {
            var target = MockRepository.GenerateStub<IManyToOnePart>();

            target.Stub(x => x.ColumnName)
                .Return("column_name");

            convention.Accept(target)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldSetKeyColumnName()
        {
            var target = MockRepository.GenerateMock<IManyToOnePart>();

            target.Stub(x => x.Property)
                .Return(new DummyPropertyInfo("Example", typeof(ExampleClass)));

            convention.Apply(target);

            target.AssertWasCalled(x => x.TheColumnNameIs("Example_id"));
        }
    }
}