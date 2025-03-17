using NUnit.Framework;
using System;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

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
            .HasAttribute("id-type", typeof(Int64).AssemblyQualifiedName);
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
            .HasAttribute("meta-type", typeof(string).AssemblyQualifiedName);
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
    public void WriteThrowsIfEntityIdColumnIsNotSet()
    {
        Assert.That(() =>
                new MappingTester<MappedObject>()
                    .ForMapping(map => map.ReferencesAny(x => x.Parent)
                        .EntityTypeColumn("AnyType")
                        .IdentityType(x => x.Id)
                        .AddMetaValue<SecondMappedObject>("SMO")),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void WriteThrowsIfEntityTypeColumnIsNotSet()
    {
        Assert.That(() =>
                new MappingTester<MappedObject>()
                    .ForMapping(map => map.ReferencesAny(x => x.Parent)
                        .EntityIdentifierColumn("AnyId")
                        .IdentityType(x => x.Id)
                        .AddMetaValue<SecondMappedObject>("SMO")),
            Throws.TypeOf<InvalidOperationException>());
        ;
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
    public void AnyIsAfterIdElement()
    {
        var mapTest = new MappingTester<MappedObject>()
            .ForMapping(map =>
            {
                map.Id(x => x.Id);
                map.ReferencesAny(x => x.Parent)
                    .EntityIdentifierColumn("AnyId")
                    .EntityTypeColumn("AnyType")
                    .IdentityType(x => x.Id)
                    .AddMetaValue<SecondMappedObject>("SMO");
            });

        mapTest.Element("class/id").ShouldBeInParentAtPosition(0);
    }

    [Test]
    public void SpecificMetaTypeShouldNotClearMetaValues()
    {
        var mapTest = new MappingTester<MappedObject>()
            .ForMapping(map =>
            {
                map.Id(x => x.Id);
                map.ReferencesAny(x => x.Parent)
                    .EntityIdentifierColumn("AnyId")
                    .EntityTypeColumn("AnyType")
                    .IdentityType(x => x.Id)
                    .MetaType<int>()
                    .AddMetaValue<SecondMappedObject>("1");
            });

        mapTest
            .Element("class/any/meta-value")
            .HasAttribute("value", "1")
            .HasAttribute("class", typeof(SecondMappedObject).AssemblyQualifiedName);
    }
}
