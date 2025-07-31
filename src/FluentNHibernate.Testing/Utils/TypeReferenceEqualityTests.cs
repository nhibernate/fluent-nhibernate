using System;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Utils;

[TestFixture]
public class TypeReferenceEqualityTests
{
    [Test]
    public void TwoStringReferencesOfSameNameShouldBeEqual()
    {
        (new TypeReference(typeof(int).AssemblyQualifiedName) == new TypeReference(typeof(int).AssemblyQualifiedName)).ShouldBeTrue();
    }

    [Test]
    public void TwoStringReferencesOfDifferentNamesShouldNotBeEqual()
    {
        (new TypeReference(typeof(int).AssemblyQualifiedName) == new TypeReference(typeof(string).AssemblyQualifiedName)).ShouldBeFalse();
    }

    [Test]
    public void TwoTypeReferencesOfSameTypeShouldBeEqual()
    {
        (new TypeReference(typeof(int)) == new TypeReference(typeof(int))).ShouldBeTrue();
    }

    [Test]
    public void TwoTypeReferencesOfDifferentTypesShouldNotBeEqual()
    {
        (new TypeReference(typeof(int)) == new TypeReference(typeof(string))).ShouldBeFalse();
    }

    [Test]
    public void TypeOnLeftReferenceBasedOnSameTypeOnRightShouldBeEqual()
    {
        (typeof(int) == new TypeReference(typeof(int))).ShouldBeTrue();
    }

    [Test]
    public void TypeOnLeftReferenceBasedOnDifferentTypeOnRightShouldNotBeEqual()
    {
        (typeof(int) == new TypeReference(typeof(string))).ShouldBeFalse();
    }

    [Test]
    public void ReferenceOnLeftSameTypeOnRightShouldBeEqual()
    {
        (new TypeReference(typeof(int)) == typeof(int)).ShouldBeTrue();
    }

    [Test]
    public void ReferenceOnLeftDifferentTypeOnRightShouldNotBeEqual()
    {
        (new TypeReference(typeof(string)) == typeof(int)).ShouldBeFalse();
    }

    [Test]
    public void EqualsTwoStringReferencesOfSameNameShouldBeEqual()
    {
        new TypeReference(typeof(int).AssemblyQualifiedName).Equals(new TypeReference(typeof(int).AssemblyQualifiedName)).ShouldBeTrue();
    }

    [Test]
    public void EqualsTwoStringReferencesOfDifferentNamesShouldNotBeEqual()
    {
        new TypeReference(typeof(int).AssemblyQualifiedName).Equals(new TypeReference(typeof(string).AssemblyQualifiedName)).ShouldBeFalse();
    }

    [Test]
    public void EqualsTwoTypeReferencesOfSameTypeShouldBeEqual()
    {
        new TypeReference(typeof(int)).Equals(new TypeReference(typeof(int))).ShouldBeTrue();
    }

    [Test]
    public void EqualsTwoTypeReferencesOfDifferentTypesShouldNotBeEqual()
    {
        new TypeReference(typeof(int)).Equals(new TypeReference(typeof(string))).ShouldBeFalse();
    }

    [Test, Ignore("EqualsTypeOnLeftReferenceBasedOnSameTypeOnRightShouldBeEqual")]
    public void EqualsTypeOnLeftReferenceBasedOnSameTypeOnRightShouldBeEqual()
    {
        // Wish we could do this :(
        typeof(int).Equals(new TypeReference(typeof(int))).ShouldBeTrue();
    }

    [Test]
    public void EqualsTypeOnLeftReferenceBasedOnDifferentTypeOnRightShouldNotBeEqual()
    {
        typeof(int).Equals(new TypeReference(typeof(string))).ShouldBeFalse();
    }

    [Test]
    public void EqualsReferenceOnLeftSameTypeOnRightShouldBeEqual()
    {
        new TypeReference(typeof(int)).Equals(typeof(int)).ShouldBeTrue();
    }

    [Test]
    public void EqualsReferenceOnLeftDifferentTypeOnRightShouldNotBeEqual()
    {
        new TypeReference(typeof(string)).Equals(typeof(int)).ShouldBeFalse();
    }

    [Test]
    public void EmptyShouldEqualEmpty()
    {
        TypeReference.Empty.Equals(TypeReference.Empty).ShouldBeTrue();
    }

    [Test]
    public void EmptyShouldNotEqualNullTypeReference()
    {
        var nullTypeReference = (TypeReference)null;
        TypeReference.Empty.Equals(nullTypeReference).ShouldBeFalse();
    }

    [Test]
    public void EmptyShouldNotEqualNullType()
    {
        var nullType = (Type)null;
        TypeReference.Empty.Equals(nullType).ShouldBeFalse();
    }

    [Test]
    public void EmptyShouldNotEqualIntTypeReference()
    {
        var intTypeReference = new TypeReference(typeof(int));
        TypeReference.Empty.Equals(intTypeReference).ShouldBeFalse();
    }

    [Test]
    public void IntTypeReferenceShouldNotEqualEmpty()
    {
        var intTypeReference = new TypeReference(typeof(int));
        intTypeReference.Equals(TypeReference.Empty).ShouldBeFalse();
    }


}
