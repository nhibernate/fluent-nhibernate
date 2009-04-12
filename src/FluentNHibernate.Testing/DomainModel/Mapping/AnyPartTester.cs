using System;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class AnyPartTester
    {
        [Test]
        public void CanCreateAnyReference()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                      .EntityIdentifierColumn("AnyId")
                                      .EntityTypeColumn("AnyType")
                                      .IdentityType(x => x.Id))
                .Element("class/any")
                .HasAttribute("name", "Parent");
        }

        [Test]
        public void ShouldDefaultToEmptyCascade()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                       .EntityIdentifierColumn("AnyId")
                                       .EntityTypeColumn("AnyType")
                                       .IdentityType(x => x.Id))
                .Element("class/any")
                .DoesntHaveAttribute("cascade");
        }

        [Test]
        public void CascadeCanBeSet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                       .EntityIdentifierColumn("AnyId")
                                       .EntityTypeColumn("AnyType")
                                       .IdentityType(x => x.Id)
                                       .Cascade.SaveUpdate())
                .Element("class/any")
                .HasAttribute("cascade", "save-update");
        }

        [Test]
        public void IdTypeCanBeSet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                       .EntityIdentifierColumn("AnyId")
                                       .EntityTypeColumn("AnyType")
                                       .IdentityType(x => x.Id))
                .Element("class/any")
                .HasAttribute("id-type", "Int64");
        }

        [Test]
        public void MetaTypeIsSetByPropertyMap()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                       .EntityIdentifierColumn("AnyId")
                                       .EntityTypeColumn("AnyType")
                                       .IdentityType(x => x.Id))
                .Element("class/any")
                .HasAttribute("meta-type", typeof(SecondMappedObject).AssemblyQualifiedName);
        }

        [Test]
        public void MetaTypeIsSetToStringWhenMetaValuesSet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                       .EntityIdentifierColumn("AnyId")
                                       .EntityTypeColumn("AnyType")
                                       .IdentityType(x => x.Id)
                                       .AddMetaValue<SecondMappedObject>("SMO"))
                .Element("class/any")
                .HasAttribute("meta-type", "String");
        }

        [Test]
        public void MetaValueCanBeSet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                       .EntityIdentifierColumn("AnyId")
                                       .EntityTypeColumn("AnyType")
                                       .IdentityType(x => x.Id)
                                       .AddMetaValue<SecondMappedObject>("SMO"))
                .Element("class/any/meta-value")
                .Exists();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteThrowsIfEntityIdColumnIsNotSet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                       .EntityTypeColumn("AnyType")
                                       .IdentityType(x => x.Id)
                                       .AddMetaValue<SecondMappedObject>("SMO"));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteThrowsIfEntityTypeColumnIsNotSet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                       .EntityIdentifierColumn("AnyId")
                                       .IdentityType(x => x.Id)
                                       .AddMetaValue<SecondMappedObject>("SMO"));

        }

        [Test]
        public void EntityTypeColumnWritesBeforeEntityIdColumn()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                       .EntityIdentifierColumn("AnyId")
                                       .EntityTypeColumn("AnyType")
                                       .IdentityType(x => x.Id))
                .Element("class/any/column")
                .HasAttribute("name", "AnyType")
                .ShouldBeInParentAtPosition(0);
        }

        [Test]
        public void MetaValueWritesBeforeEntityTypeColumn()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                       .EntityIdentifierColumn("AnyId")
                                       .EntityTypeColumn("AnyType")
                                       .IdentityType(x => x.Id)
                                       .AddMetaValue<SecondMappedObject>("SMO"))
                .Element("class/any/meta-value")
                .ShouldBeInParentAtPosition(0);
        }

        [Test]
        public void MetaValuesHaveCorrectValueMappedToClass()
        {
            new MappingTester<MappedObject>()
                 .ForMapping(map => map.ReferencesAny(x => x.Parent)
                                        .EntityIdentifierColumn("AnyId")
                                        .EntityTypeColumn("AnyType")
                                        .IdentityType(x => x.Id)
                                        .AddMetaValue<SecondMappedObject>("SMO"))
                 .Element("class/any/meta-value")
                 .HasAttribute("value", "SMO")
                 .HasAttribute("class", typeof(SecondMappedObject).AssemblyQualifiedName);
        }

        [Test]
        public void PositionIsSetToAnywhere()
        {
            Assert.AreEqual(PartPosition.Anywhere, new AnyPart<SecondMappedObject>(null).PositionOnDocument);
        }

        [Test]
        public void LevelIsSetToOne()
        {
            Assert.AreEqual(1, new AnyPart<SecondMappedObject>(null).LevelWithinPosition);
        }
    }
}