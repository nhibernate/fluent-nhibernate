using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class SubClassTester
    {
        [Test]
        public void CreateDiscriminator()
        {
            new MappingTester<SecondMappedObject>()
                .ForMapping(map => map.DiscriminateSubClassesOnColumn<string>("Type"))
                .Element("class/discriminator")
                    .Exists()
                    .HasAttribute("column", "Type")
                    .HasAttribute("type", "String");
        }

        [Test]
        public void CanUseDefaultDiscriminatorValue()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.Map(x => x.Name)))
                .Element("class/subclass")
                    .DoesntHaveAttribute("discriminator-value");
        }

        [Test]
        public void CreateTheSubClassMappings()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("red", sc => { }))
                .Element("//subclass")
                    .Exists()
                    .HasAttribute("name", typeof(SecondMappedObject).AssemblyQualifiedName)
                    .HasAttribute("discriminator-value", "red");
        }

        [Test]
        public void MapsProperty()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.Map(x => x.Name)))
                .Element("//subclass/property").HasAttribute("name", "Name");
        }

        [Test]
        public void SubClassLazy()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.LazyLoad()))
                .Element("//subclass").HasAttribute("lazy", "true");
        }

        [Test]
        public void SubClassNotLazy()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>(sc => sc.Not.LazyLoad()))
                .Element("//subclass").HasAttribute("lazy", "false");
        }

        [Test]
        public void MapsComponent()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.Component(x => x.Component, c => { })))
                .Element("//subclass/component").Exists();
        }

        [Test]
        public void MapsDynamicComponent()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.DynamicComponent(x => x.Dictionary, c => { })))
                .Element("//subclass/dynamic-component").Exists();
        }

        [Test]
        public void MapsHasMany()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.HasMany(x => x.Children)))
                .Element("//subclass/bag").Exists();
        }

        [Test]
        public void MapsHasManyToMany()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.HasManyToMany(x => x.Children)))
                .Element("//subclass/bag").Exists();
        }

        [Test]
        public void MapsHasOne()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.HasOne(x => x.Parent)))
                .Element("//subclass/one-to-one").Exists();
        }

        [Test]
        public void MapsReferences()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.References(x => x.Parent)))
                .Element("//subclass/many-to-one").Exists();
        }

        [Test]
        public void MapsReferencesAny()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc =>
                            sc.ReferencesAny(x => x.Parent)
                                .IdentityType(x => x.Id)
                                .EntityIdentifierColumn("col")
                                .EntityTypeColumn("col")))
                .Element("//subclass/any").Exists();
        }

        [Test]
        public void MapsSubSubclass()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.SubClass<SecondMappedObject>(sc2 => { })))
                .Element("//subclass/subclass").Exists();
        }

        [Test]
        public void SubSubclassIsLast()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc =>
                        {
                            sc.SubClass<SecondMappedObject>(sc2 => { });
                            sc.Map(x => x.Name);
                        }))
                .Element("//subclass/subclass").ShouldBeInParentAtPosition(1);
        }

        [Test]
        public void MapsVersion()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<MappedObject>(sc => sc.Version(x => x.Version)))
                .Element("//subclass/version").Exists();
        }

        [Test]
        public void SubclassShouldNotHaveDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn<string>("Type")
                        .SubClass<SecondMappedObject>("red", m =>
                        {
                            m.Map(x => x.Name);
                            m.SubClass<SecondMappedObject>("blue", m2 =>
                            {});
                        }))
                 .Element("//subclass/discriminator").DoesntExist();
        }

        [Test]
        public void CreateDiscriminatorValueAtClassLevel()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.DiscriminateSubClassesOnColumn("Type", "Foo")
                        .SubClass<SecondMappedObject>("Bar", m => m.Map(x => x.Name)))
                .Element("class")
                    .HasAttribute("discriminator-value", "Foo");
        }

        [Test]
        public void DiscriminatorAssumesStringIfNoTypeSupplied()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.DiscriminateSubClassesOnColumn("Type"))
                .Element("class/discriminator")
                    .HasAttribute("type", "String");
        }
    }
}