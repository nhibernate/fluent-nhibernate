using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class TableNameConventionTests
    {
        private TableNameConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new TableNameConvention();
        }

        [Test]
        public void ShouldAcceptIfTableNameNotSet()
        {
            var target = MockRepository.GenerateStub<IClassMap>();

            target.Stub(x => x.TableName)
                .Return(null);

            convention.Accept(target)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptIfTableNameSet()
        {
            var target = MockRepository.GenerateStub<IClassMap>();

            target.Stub(x => x.TableName)
                .Return("table_name");

            convention.Accept(target)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldSetTableName()
        {
            var target = MockRepository.GenerateMock<IClassMap>();

            target.Stub(x => x.EntityType)
                .Return(typeof(ExampleClass));

            convention.Apply(target);

            target.AssertWasCalled(x => x.WithTable("`ExampleClass`"));
        }
    }
}