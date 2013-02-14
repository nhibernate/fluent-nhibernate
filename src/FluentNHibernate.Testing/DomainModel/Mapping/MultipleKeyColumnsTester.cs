using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class MultipleKeyColumnsTester
    {
        [Test]
        public void CanHaveMultipleKeyColumns()
        {
            var provider = (IIndeterminateSubclassMappingProvider)new TestMap();
            provider.GetSubclassMapping(SubclassType.JoinedSubclass).Key.Columns.Count().ShouldEqual(2);
        }

        public class Base
        {
            public int Id { get; set; }
        }

        public class TestMap : SubclassMap<Base>
        {
            public TestMap()
            {
                KeyColumn("col1");
                KeyColumn("col2");
            }
        }
    }
}
