using System.Collections.Generic;
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
        private DefaultForeignKeyConvention convention;

        [SetUp]
        public void CreateConvention()
        {
            convention = new DefaultForeignKeyConvention();
        }

        [Test]
        public void ShouldAcceptIfNoColumnNameSet()
        {
            var target = MockRepository.GenerateStub<IOneToManyPart>();
            var columnNames = MockRepository.GenerateStub<IColumnNameCollection>();

            columnNames.Stub(x => x.List())
                .Return(new List<string>());

            target.Stub(x => x.KeyColumnNames)
                .Return(columnNames);

            convention.Accept(target)
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptIfColumnNameSet()
        {
            var target = MockRepository.GenerateStub<IOneToManyPart>();
            var columnNames = MockRepository.GenerateStub<IColumnNameCollection>();

            columnNames.Stub(x => x.List())
                .Return(new List<string>{ "column_name" });

            target.Stub(x => x.KeyColumnNames)
                .Return(columnNames);

            convention.Accept(target)
                .ShouldBeFalse();
        }

        [Test]
        public void ShouldSetKeyColumnName()
        {
            var target = MockRepository.GenerateStub<IOneToManyPart>();
            var columnNames = MockRepository.GenerateMock<IColumnNameCollection>();

            target.Stub(x => x.KeyColumnNames)
                .Return(columnNames);
            target.Stub(x => x.EntityType)
                .Return(typeof(ExampleClass));

            convention.Apply(target);

            columnNames.AssertWasCalled(x => x.Add("ExampleClass_id"));
        }
    }
}