using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ColumnPartTests
    {
        private ColumnMapping mapping;
        private ColumnPart columnPart;

        [SetUp]
        public void SetUp()
        {
            this.mapping = new ColumnMapping();
            this.columnPart = new ColumnPart(mapping);
        }

        [Test]
        public void ShouldSetName()
        {
            columnPart.Name("col1");
            mapping.Name.ShouldEqual("col1");
        }

        [Test]
        public void ShouldSetLength()
        {
            columnPart.Length(50);
            mapping.Length.ShouldEqual(50);
        }

        [Test]
        public void ShouldSetAsNullable()
        {
            columnPart.Nullable();
            mapping.NotNull.ShouldBeFalse();
        }

        [Test]
        public void ShouldSetAsNotNullable()
        {
            columnPart.Not.Nullable();
            mapping.NotNull.ShouldBeTrue();
        }



        
    }
}
