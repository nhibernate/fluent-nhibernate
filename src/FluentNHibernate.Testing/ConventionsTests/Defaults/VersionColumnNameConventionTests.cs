using System;
using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests.Defaults
{
    [TestFixture]
    public class VersionColumnNameConventionTests
    {
        private VersionColumnNameConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new VersionColumnNameConvention();
        }

        [Test]
        public void ShouldAcceptIfTableNameNotSet()
        {
            var target = MockRepository.GenerateStub<IVersion>();

            target.Stub(x => x.GetColumnName())
                .Return(null);

            convention.Accept(target)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptIfTableNameSet()
        {
            var target = MockRepository.GenerateStub<IVersion>();

            target.Stub(x => x.GetColumnName())
                .Return("column_name");

            convention.Accept(target)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldSetTableName()
        {
            var target = MockRepository.GenerateMock<IVersion>();

            target.Stub(x => x.Property)
                .Return(new DummyPropertyInfo("Version", typeof(DateTime)));

            convention.Apply(target);

            target.AssertWasCalled(x => x.ColumnName("Version"));
        }
    }
}