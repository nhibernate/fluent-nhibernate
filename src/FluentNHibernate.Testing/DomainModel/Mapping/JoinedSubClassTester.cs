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
            .Element("class/joined-subclass").HasAttribute("name", typeof(SubClass).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyCheckConstraintName()
        {
            new MappingTester<SuperClass>()
              .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.CheckConstraint("name")))
              .Element("class/joined-subclass").HasAttribute("check", "name");
        }

        [Test]
        public void CanSpecifyProxyByType()
        {
            new MappingTester<SuperClass>()
              .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.Proxy(typeof(ProxyClass))))
              .Element("class/joined-subclass").HasAttribute("proxy", typeof(ProxyClass).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyProxyByTypeInstance()
        {
            new MappingTester<SuperClass>()
              .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.Proxy<ProxyClass>()))
              .Element("class/joined-subclass").HasAttribute("proxy", typeof(ProxyClass).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyLazy()
        {
            new MappingTester<SuperClass>()
              .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.LazyLoad()))
              .Element("class/joined-subclass").HasAttribute("lazy", "true");
        }

        [Test]
        public void CanSpecifyNotLazy()
        {
            new MappingTester<SuperClass>()
              .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.Not.LazyLoad()))
              .Element("class/joined-subclass").HasAttribute("lazy", "false");
        }

        [Test]
        public void CanSpecifyDynamicUpdate()
        {
            new MappingTester<SuperClass>()
              .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.DynamicUpdate()))
              .Element("class/joined-subclass").HasAttribute("dynamic-update", "true");
        }

        [Test]
        public void CanSpecifyDynamicInsert()
        {
            new MappingTester<SuperClass>()
              .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.DynamicInsert()))
              .Element("class/joined-subclass").HasAttribute("dynamic-insert", "true");
        }

        [Test]
        public void CanSpecifySelectBeforeUpdate()
        {
            new MappingTester<SuperClass>()
              .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.SelectBeforeUpdate()))
              .Element("class/joined-subclass").HasAttribute("select-before-update", "true");
        }

        [Test]
        public void CanSpecifyAbstract()
        {
            new MappingTester<SuperClass>()
              .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.Abstract()))
              .Element("class/joined-subclass").HasAttribute("abstract", "true");
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
                .Element("class/joined-subclass/key/column").HasAttribute("name", "columnName");
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
               .ForMapping(m => m.JoinedSubClass<SubClass>("columnName", sm => sm.Table("TestTable")))
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
                        .SubClass<SubClass>("subclass", sc =>
                            sc.Map(x => x.Name));
                })
                .Element("class/*[last()]").HasName("subclass");
        }

        [Test]
        public void SchemaSuported()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.JoinedSubClass<MappedObjectSubclass>("id", sc => sc.Schema("test")))
                .Element("//joined-subclass").HasAttribute("schema", "test");
        }

        [Test]
        public void MapsComponent()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.JoinedSubClass<MappedObjectSubclass>("id", sc => sc.Component(x => x.Component, c => { })))
                .Element("//joined-subclass/component").Exists();
        }

        [Test]
        public void MapsDynamicComponent()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.JoinedSubClass<MappedObjectSubclass>("id", sc => sc.DynamicComponent(x => x.Dictionary, c => { })))
                .Element("//joined-subclass/dynamic-component").Exists();
        }

        [Test]
        public void MapsHasMany()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.JoinedSubClass<MappedObjectSubclass>("id", sc => sc.HasMany(x => x.Children)))
                .Element("//joined-subclass/bag").Exists();
        }

        [Test]
        public void MapsHasManyToMany()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.JoinedSubClass<MappedObjectSubclass>("id", sc => sc.HasManyToMany(x => x.Children)))
                .Element("//joined-subclass/bag").Exists();
        }

        [Test]
        public void MapsHasOne()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.JoinedSubClass<MappedObjectSubclass>("id", sc => sc.HasOne(x => x.Parent)))
                .Element("//joined-subclass/one-to-one").Exists();
        }

        [Test]
        public void MapsReferences()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.JoinedSubClass<MappedObjectSubclass>("id", sc => sc.References(x => x.Parent)))
                .Element("//joined-subclass/many-to-one").Exists();
        }

        [Test]
        public void MapsReferencesAny()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.JoinedSubClass<MappedObjectSubclass>("id", sc =>
                        sc.ReferencesAny(x => x.Parent)
                            .IdentityType(x => x.Id)
                            .EntityIdentifierColumn("col")
                            .EntityTypeColumn("col")))
                .Element("//joined-subclass/any").Exists();
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

        private class ProxyClass
        {}
    }
}