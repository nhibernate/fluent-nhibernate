using System.Collections.Generic;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

[TestFixture]
public class JoinPartTester
{
    [Test]
    public void CreatesJoinElement()
    {
        new MappingTester<JoinTarget>()
            .ForMapping(m => m.Join("myTable", t => t.Map(x => x.Name)))
            .Element("class/join").Exists();
    }

    [Test]
    public void JoinElementHasTableName()
    {
        new MappingTester<JoinTarget>()
            .ForMapping(m => m.Join("myTable", t => t.Map(x => x.Name)))
            .Element("class/join").HasAttribute("table", "myTable");
    }

    [Test]
    public void PropertiesInJoinAreOutputInTheJoinElement()
    {
        new MappingTester<JoinTarget>()
            .ForMapping(m => m.Join("myTable", t => t.Map(x => x.Name)))
            .Element("class/join/property").HasAttribute("name", "Name");
    }

    [Test]
    public void PropertiesDefinedInClassAndPropertiesDefinedInJoinAreSeparateInOutput()
    {
        new MappingTester<JoinTarget>()
            .ForMapping(m =>
            {
                m.Map(x => x.Name);
                m.Join("myTable", t => t.Map(x => x.CustomerName));
            })
            .Element("class/property").HasAttribute("name", "Name")
            .Element("class/join/property").HasAttribute("name", "CustomerName");
    }

    [Test]
    public void JoinElementAlwaysHasAKey()
    {
        new MappingTester<JoinTarget>()
            .ForMapping(m => m.Join("myTable", t => t.Map(x => x.Name)))
            .Element("class/join/key").Exists();
    }

    [Test]
    public void KeyDefaultsToClassNameId()
    {
        new MappingTester<JoinTarget>()
            .ForMapping(m => m.Join("myTable", t => t.Map(x => x.Name)))
            .Element("class/join/key/column").HasAttribute("name", "JoinTarget_id");
    }

    [Test]
    public void CanOverrideKey()
    {
        new MappingTester<JoinTarget>()
            .ForMapping(m => m.Join("myTable", t => t.KeyColumn("ID")))
            .Element("class/join/key/column").HasAttribute("name", "ID");
    }

    [Test]
    public void CanOverrideKeyInConvention()
    {
        new MappingTester<JoinTarget>()
            .Conventions(x => x.Add(new JoinConvention()))
            .ForMapping(m => m.Join("myTable", t => t.Map(x => x.Name)))
            .Element("class/join/key/column").HasAttribute("name", "JoinTargetID");
    }

    class JoinConvention : IJoinConvention
    {
        public void Apply(IJoinInstance instance)
        {
            instance.Key.Column(instance.EntityType.Name + "ID");
        }
    }

    [Test]
    public void JoinIsAlwaysLastInTheClassElement()
    {
        // not absolutely certain if this is necessary, but my mappings wouldn't compile with a 'set' after the 'join'
        new MappingTester<JoinTarget>()
            .ForMapping(m =>
            {
                m.Map(x => x.Name);
                m.HasMany(x => x.Children);
                m.Join("myTable", t => t.KeyColumn("ID"));
            })
            .Element("class/*[last()]").HasName("join");
    }

    [Test]
    public void JoinContainsAnSqlInsertElement()
    {
        new MappingTester<JoinTarget>()
            .ForMapping(m => m.Join("myTable", t => t.SqlInsert("INSERT INTO myTable VALUES(?)"))) // Now it's not important whether statement is correct or not.
            .Element("class/join/sql-insert").Exists();
    }

    [Test]
    public void JoinContainsAnSqlUpdateElement()
    {
        new MappingTester<JoinTarget>()
            .ForMapping(m => m.Join("myTable", t => t.SqlUpdate("UPDATE myTable SET Name = ? WHERE Id = ?"))) // Now it's not important whether statement is correct or not.
            .Element("class/join/sql-update").Exists();
    }

    [Test]
    public void JoinContainsAnSqlDeleteElement()
    {
        new MappingTester<JoinTarget>()
            .ForMapping(m => m.Join("myTable", t => t.SqlDelete("DELETE FROM myTable WHERE Id = ?"))) // Now it's not important whether statement is correct or not.
            .Element("class/join/sql-delete").Exists();
    }

    class JoinTarget
    {
        public string Name { get; set; }
        public string CustomerName { get; set; }
        public IList<JoinTarget> Children { get; set; }
    }
}
