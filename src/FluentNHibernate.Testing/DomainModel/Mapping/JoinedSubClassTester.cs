using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class JoinedSubClassTester
    {
        [Test]
        public void CreatesJoinedSubClassElement()
        {
            new MappingTester<SuperClass>()
                .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.Map(x => x.Name)))
                .Element("class/joined-subclass").Exists();
        }

        [Test]
        public void NamesJoinedSubClassElementCorrectly()
        {
            new MappingTester<SuperClass>()
                .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.Map(x => x.Name)))
                .Element("class/joined-subclass").HasAttribute("name", "SubClass");
        }

        [Test]
        public void JoinedSubClassHasKeyElement()
        {
            new MappingTester<SuperClass>()
                .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.Map(x => x.Name)))
                .Element("class/joined-subclass/key").Exists();
        }

        [Test]
        public void JoinedSubClassKeyElementHasCorrectColumn()
        {
            new MappingTester<SuperClass>()
                .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.Map(x => x.Name)))
                .Element("class/joined-subclass/key").HasAttribute("column", "columnName");
        }

        [Test]
        public void PropertiesGetAddedToJoinedSubClassElement()
        {
            new MappingTester<SuperClass>()
                .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.Map(x => x.Name)))
                .Element("class/joined-subclass/property")
                    .Exists()
                    .HasAttribute("name", "Name");
        }

        [Test]
        public void CanSpecifyJoinedSubClassTable()
        {
            new MappingTester<SuperClass>()
               .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.WithTableName("TestTable")))
               .Element("class/joined-subclass")
                   .HasAttribute("table", "TestTable");
        }

        [Test]
        public void SubclassesAreLastInClass()
        {
            new MappingTester<SuperClass>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Type);
                    m.References(x => x.Parent);
                    m.DiscriminateSubClassesOnColumn<string>("class")
                        .SubClass<SubClass>()
                        .IsIdentifiedBy("subclass")
                        .MapSubClassColumns(sc =>
                        {
                            sc.Map(x => x.Name);
                        });
                })
                .Element("class/*[last()]").HasName("subclass");
        }

        private class SuperClass
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public SuperClass Parent { get; set; }
        }

        private class SubClass : SuperClass
        {
            public string Name { get; set; }
        }
    }
}