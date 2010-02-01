using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class StoredProcedureTests
    {
        [Test]
        public void Can_specify_sql_insert()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.SqlInsert("Insert ABC");
                })
                .Element("class/sql-insert")
                .ValueEquals("Insert ABC");
        }

        [Test]
        public void Can_specify_sql_update()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.SqlUpdate("Update ABC");
                })
                .Element("class/sql-update")
                .ValueEquals("Update ABC");
        }

        [Test]
        public void Can_specify_sql_delete()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.SqlDelete("Delete ABC");
                })
                .Element("class/sql-delete")
                .ValueEquals("Delete ABC");
        }

        [Test]
        public void Can_specify_sql_delete_all()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.SqlDeleteAll("Delete ABC");
                })
                .Element("class/sql-delete-all")
                .ValueEquals("Delete ABC");
        }
    }
}
