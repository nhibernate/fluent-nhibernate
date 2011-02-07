using System;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class CompositeIdentityPartTester
    {
        [Test]
        public void Defaults()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId().KeyProperty(x => x.LongId))
                .Element("class/composite-id/key-property")
                    .HasAttribute("name", "LongId");
        }

        [Test]
        public void KeyPropertyExplicitColumnName()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId().KeyProperty(x => x.LongId, "SomeColumn"))
                .Element("class/composite-id/key-property/column")
                    .HasAttribute("name", "SomeColumn");
        }

        [Test]
        public void KeyPropertyTypeIsSetToTypeName()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId().KeyProperty(x => x.LongId))
                .Element("class/composite-id/key-property")
                    .HasAttribute("type", typeof(long).AssemblyQualifiedName);
        }

        [Test]
        public void KeyPropertyTypeIsSetToFullTypeNameIfTypeGeneric()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId().KeyProperty(x => x.NullableLongId))
                .Element("class/composite-id/key-property")
                    .HasAttribute("type", typeof(long?).AssemblyQualifiedName);
        }

        [Test]
        public void KeyManyToOneDefaults()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId().KeyReference(x => x.Child))
                .Element("class/composite-id/key-many-to-one")
                    .HasAttribute("name", "Child")
                    .HasAttribute("class", typeof(CompIdChild).AssemblyQualifiedName);
        }

        [Test]
        public void KeyManyToOneLazySetToProxyForTrue()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId().KeyReference(x => x.Child, m => m.Lazy()))
                .Element("class/composite-id/key-many-to-one")
                    .HasAttribute("lazy", "proxy");
        }

        [Test]
        public void KeyManyToOneExplicitColumnName()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId().KeyReference(x => x.Child, "SomeColumn"))
                .Element("class/composite-id/key-many-to-one/column").HasAttribute("name", "SomeColumn");
        }

        [Test]
        public void KeyManyToOneForeignKey()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId().KeyReference(x => x.Child, p => p.ForeignKey("fk1"), "SomeColumn"))
                .Element("class/composite-id/key-many-to-one").HasAttribute("foreign-key", "fk1");
        }

        [Test]
        public void KeyPropertyEnumShouldBeStringByDefault()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c =>
                    c.CompositeId()
                        .KeyProperty(x => x.EnumProperty))
                .Element("class/composite-id/key-property")
                    .HasAttribute("type", typeof(GenericEnumMapper<SomeEnum>).AssemblyQualifiedName);
        }

        [Test]
        public void MixedKeyPropertyAndManyToOne()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId()
                    .KeyProperty(x => x.LongId)
                    .KeyReference(x => x.Child))
                .Element("class/composite-id/key-property")
                    .HasAttribute("name", "LongId")
                .RootElement.Element("class/composite-id/key-many-to-one")
                    .HasAttribute("name", "Child");
        }

        [Test]
        public void IdIsAlwaysFirstElementInClass()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(m =>
                {
                    m.Map(x => x.DummyProp); // just a property in this case
                    m.CompositeId()
                        .KeyProperty(x => x.LongId)
                        .KeyReference(x => x.Child);
                })
                .Element("class/*[1]").HasName("composite-id");
        }

        [Test]
        public void ComponentNamesAreSet()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId(x => x.Child).KeyProperty(x => x.ChildId))
                .Element("class/composite-id/key-property")
                .HasAttribute("name", "ChildId")
                .RootElement.Element("class/composite-id")
                .HasAttribute("name", "Child");

        }

        [Test]
        public void KeyPropertyCustomType()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId(x => x.Child).KeyProperty(x => x.ChildId, kp => kp.Type(typeof(int))))
                .Element("class/composite-id/key-property")
                .HasAttribute("type", typeof(int).AssemblyQualifiedName);
        }

        [Test]
        public void ComponentCompositeIdWillSetNameAndClass()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId().ComponentCompositeIdentifier(x => x.Key))
                .Element("class/composite-id")
                .HasAttribute("name", "Key")
                .HasAttribute("class", typeof(ComponentKey).AssemblyQualifiedName);
        }

        [Test]
        public void MixedKeyPropertyAndManyToOneOrdering()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(c => c.CompositeId()
                    .KeyReference(x => x.Child)
                    .KeyProperty(x => x.LongId))
                .Element("class/composite-id/*[1]")
                    .HasName("key-many-to-one")
                .RootElement.Element("class/composite-id/*[2]")
                    .HasName("key-property");
        }

        public class CompIdTarget
        {
            public virtual long LongId { get; set; }
            public virtual long? NullableLongId { get; set; }
            public virtual ComponentKey Key { get; set; }
            public virtual CompIdChild Child { get; set; }
            public virtual string DummyProp { get; set; }
            public virtual SomeEnum EnumProperty { get; set; }
        }

        public enum SomeEnum
        {}

        public class CompIdChild
        {
            public virtual long ChildId { get; set; }
        }

        public class ComponentKey
        {
            public virtual int KeyCol1 { get; set; }
            public virtual int KeyCol2 { get; set; }
        }
    }
}
