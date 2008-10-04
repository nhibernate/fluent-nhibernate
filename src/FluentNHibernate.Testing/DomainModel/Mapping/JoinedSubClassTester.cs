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
                .ForMapping(m => m.JoinedSubClass<SubClassA>("columnName", sm => sm.Map(x => x.Name)))
                .Element("class/joined-subclass").Exists();
        }

        [Test]
        public void NamesJoinedSubClassElementCorrectly()
        {
            new MappingTester<SuperClass>()
                .ForMapping(m => m.JoinedSubClass<SubClassA>("columnName", sm => sm.Map(x => x.Name)))
                .Element("class/joined-subclass").HasAttribute("name", "SubClassA");
        }

        [Test]
        public void JoinedSubClassHasKeyElement()
        {
            new MappingTester<SuperClass>()
                .ForMapping(m => m.JoinedSubClass<SubClassA>("columnName", sm => sm.Map(x => x.Name)))
                .Element("class/joined-subclass/key").Exists();
        }

        [Test]
        public void JoinedSubClassKeyElementHasCorrectColumn()
        {
            new MappingTester<SuperClass>()
                .ForMapping(m => m.JoinedSubClass<SubClassA>("columnName", sm => sm.Map(x => x.Name)))
                .Element("class/joined-subclass/key").HasAttribute("column", "columnName");
        }

        [Test]
        public void PropertiesGetAddedToJoinedSubClassElement()
        {
            new MappingTester<SuperClass>()
                .ForMapping(m => m.JoinedSubClass<SubClassA>("columnName", sm => sm.Map(x => x.Name)))
                .Element("class/joined-subclass/property")
                    .Exists()
                    .HasAttribute("name", "Name");
        }

        [Test]
        public void CanSpecifyJoinedSubClassTable()
        {
            new MappingTester<SuperClass>()
               .ForMapping(m => m.JoinedSubClass<SubClassA>("columnName", sm => sm.WithTableName("TestTable")))
               .Element("class/joined-subclass")
                   .HasAttribute("table", "TestTable");
        }
    }

    public class SuperClass
    {}

    public class SubClassA : SuperClass
    {
        public string Name { get; set; }
    }
}