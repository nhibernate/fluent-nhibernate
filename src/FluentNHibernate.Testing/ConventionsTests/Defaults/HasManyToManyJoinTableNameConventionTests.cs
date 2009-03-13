using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class HasManyToManyJoinTableNameConventionTests
    {
        private HasManyToManyJoinTableNameConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new HasManyToManyJoinTableNameConvention();
        }

        [Test]
        public void ShouldAcceptIfTableNameNotSet()
        {
            var target = MockRepository.GenerateStub<IManyToManyPart>();

            target.Stub(x => x.TableName)
                .Return(null);

            convention.Accept(target)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptIfTableNameSet()
        {
            var target = MockRepository.GenerateStub<IManyToManyPart>();

            target.Stub(x => x.TableName)
                .Return("table_name");

            convention.Accept(target)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldSetTableName()
        {
            var target = MockRepository.GenerateMock<IManyToManyPart>();

            target.Stub(x => x.ChildType)
                .Return(typeof(ExampleClass));
            target.Stub(x => x.EntityType)
                .Return(typeof(ExampleParentClass));

            convention.Apply(target);

            target.AssertWasCalled(x => x.WithTableName("ExampleClassToExampleParentClass"));
        }
    }
}