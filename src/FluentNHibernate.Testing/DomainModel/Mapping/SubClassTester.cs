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
                        .SubClass<SecondMappedObject>("red", sc => sc.Map(x => x.Name)))
                .Element("//subclass")
                    .Exists()
                    .HasAttribute("name", typeof(SecondMappedObject).AssemblyQualifiedName)
                    .HasAttribute("discriminator-value", "red")
                .Element("//subclass/property")
                    .Exists()
                    .HasAttribute("column", "Name");
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