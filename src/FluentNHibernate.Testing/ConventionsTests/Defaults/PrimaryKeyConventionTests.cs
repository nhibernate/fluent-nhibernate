using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class PrimaryKeyConventionTests
    {
        private PrimaryKeyConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new PrimaryKeyConvention();
        }

        [Test]
        public void ShouldAcceptIfNoColumnNameSet()
        {
            var target = MockRepository.GenerateStub<IIdentityPart>();

            target.Stub(x => x.GetColumnName())
                .Return(null);

            convention.Accept(target)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptIfColumnNameSet()
        {
            var target = MockRepository.GenerateStub<IIdentityPart>();

            target.Stub(x => x.GetColumnName())
                .Return("column_name");

            convention.Accept(target)
                .ShouldBeFalse();
        }

        [Test]
        public void SetsColumnName()
        {
            var target = MockRepository.GenerateMock<IIdentityPart>();

            target.Stub(x => x.Property)
                .Return(new DummyPropertyInfo("Id", typeof(int)));

            convention.Apply(target);

            target.AssertWasCalled(x => x.ColumnName("Id"));
        }
    }
}