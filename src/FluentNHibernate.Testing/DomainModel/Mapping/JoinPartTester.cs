using System.Collections.Generic;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class JoinPartTester
    {
        [Test]
        public void CreatesJoinElement()
        {
            new MappingTester<JoinTarget>()
                .ForMapping(m => m.WithTable("myTable", t => t.Map(x => x.Name)))
                .Element("class/join").Exists();
        }

        [Test]
        public void JoinElementHasTableName()
        {
            new MappingTester<JoinTarget>()
                .ForMapping(m => m.WithTable("myTable", t => t.Map(x => x.Name)))
                .Element("class/join").HasAttribute("table", "myTable");
        }

        [Test]
        public void PropertiesInJoinAreOutputInTheJoinElement()
        {
            new MappingTester<JoinTarget>()
                .ForMapping(m => m.WithTable("myTable", t => t.Map(x => x.Name)))
                .Element("class/join/property").HasAttribute("name", "Name");
        }

        [Test]
        public void PropertiesDefinedInClassAndPropertiesDefinedInJoinAreSeparateInOutput()
        {
            new MappingTester<JoinTarget>()
                .ForMapping(m =>
                {
                    m.Map(x => x.Name);
                    m.WithTable("myTable", t => t.Map(x => x.CustomerName));
                })
                .Element("class/property").HasAttribute("name", "Name")
                .Element("class/join/property").HasAttribute("name", "CustomerName");
        }

        [Test]
        public void JoinElementAlwaysHasAKey()
        {
            new MappingTester<JoinTarget>()
                .ForMapping(m => m.WithTable("myTable", t => t.Map(x => x.Name)))
                .Element("class/join/key").Exists();
        }

        [Test]
        public void KeyDefaultsToClassNameId()
        {
            new MappingTester<JoinTarget>()
                .ForMapping(m => m.WithTable("myTable", t => t.Map(x => x.Name)))
                .Element("class/join/key/column").HasAttribute("name", "JoinTargetID");
        }

        [Test]
        public void CanOverrideKey()
        {
            new MappingTester<JoinTarget>()
                .ForMapping(m => m.WithTable("myTable", t => t.WithKeyColumn("ID")))
                .Element("class/join/key/column").HasAttribute("name", "ID");
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
                    m.WithTable("myTable", t => t.WithKeyColumn("ID"));
                })
                .Element("class/*[last()]").HasName("join");
        }

        private class JoinTarget
        {
            public string Name { get; set; }
            public string CustomerName { get; set; }
            public IList<JoinTarget> Children { get; set; }
        }
    }
}