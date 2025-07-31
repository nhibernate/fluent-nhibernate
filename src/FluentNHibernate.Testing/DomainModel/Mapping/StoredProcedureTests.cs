using System;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

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
    
    [Test]
    public void Can_specify_sql_insert_for_subclass()
    {
        var check = "Insert ABC";
        CreateMappingTester(x => x.SqlInsert(check))
            .Element("//subclass/sql-insert")
            .ValueEquals(check);
    }
    
    [Test]
    public void Can_specify_sql_update_for_subclass()
    {
        var check = "Update ABC";
        CreateMappingTester(x => x.SqlUpdate(check))
            .Element("//subclass/sql-update")
            .ValueEquals(check);
    }
    
    [Test]
    public void Can_specify_sql_delete_for_subclass()
    {
        var check = "Delete ABC";
        CreateMappingTester(x => x.SqlDelete(check))
            .Element("//subclass/sql-delete")
            .ValueEquals(check);
    }
    
    [Test]
    public void Can_specify_sql_delete_all_for_subclass()
    {
        var check = "Delete ABC";
        CreateMappingTester(x => x.SqlDeleteAll(check))
            .Element("//subclass/sql-delete-all")
            .ValueEquals(check);
    }

    private MappingTester<MappedObject> CreateMappingTester(Action<SubclassMap<MappedObjectSubclass>> subclassMap)
    {
        return new MappingTester<MappedObject>()
            .SubClassMapping<MappedObjectSubclass>(sc =>
            {
                sc.Map(x => x.SubclassProperty);
                subclassMap.Invoke(sc);
            })
            .ForMapping(m =>
            {
                m.DiscriminateSubClassesOnColumn("test");
                m.Id(x => x.Id);
            });
    }
}
