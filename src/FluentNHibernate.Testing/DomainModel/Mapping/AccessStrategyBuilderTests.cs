using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using NHibernate.Properties;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class AccessStrategyBuilderTests
    {
        private MappingPartStub mapping;
        private AccessStrategyBuilder<MappingPartStub> builder;

        [SetUp]
        public void SetUp()
        {
            mapping = new MappingPartStub();
            builder = new AccessStrategyBuilder<MappingPartStub>(this.mapping);
        }

        [Test]
        public void AsProperty_UsesPropertyAsAccessStrategy()
        {
            builder.AsProperty();

            Assert.IsTrue(mapping.HasAttribute("access", "property"));
        }

        [Test]
        public void AsField_UsesFieldAsAccessStrategy()
        {
            builder.AsField();

            Assert.IsTrue(mapping.HasAttribute("access", "field"));
        }

        [Test]
        public void AsCamelCaseField_UsesFieldAsAccessStrategy_AndCamelCaseAsNamingStrategy()
        {
            builder.AsCamelCaseField();

            Assert.IsTrue(mapping.HasAttribute("access", "field.camelcase"));
        }

        [Test]
        public void AsCamelCaseField_WithNonePrefix_UsesFieldAsAccessStrategy_AndCamelCaseAsNamingStrategy_AndHasNoPrefix()
        {
            builder.AsCamelCaseField(Prefix.None);

            Assert.IsTrue(mapping.HasAttribute("access", "field.camelcase"));
        }

        [Test, ExpectedException(typeof(InvalidPrefixException), ExpectedMessage = "m is not a valid prefix for a CamelCase Field.")]
        public void AsCamelCaseField_WithMPrefix_ThrowsInvalidPrefixException()
        {
            builder.AsCamelCaseField(Prefix.m);
        }

        [Test, ExpectedException(typeof(InvalidPrefixException), ExpectedMessage = "m_ is not a valid prefix for a CamelCase Field.")]
        public void AsCamelCaseField_WithMUnderscorePrefix_ThrowsInvalidPrefixException()
        {
            builder.AsCamelCaseField(Prefix.mUnderscore);
        }

        [Test]
        public void AsCamelCaseField_WithUnderscorePrefix_UsesFieldAsAccessStrategy_AndCamelCaseAsNamingStrategy_AndHasUnderscorePrefix()
        {
            builder.AsCamelCaseField(Prefix.Underscore);

            Assert.IsTrue(mapping.HasAttribute("access", "field.camelcase-underscore"));
        }

        [Test]
        public void AsLowerCaseField_UsesFieldAsAccessStrategy_AndLowerCaseAsNamingStrategy()
        {
            builder.AsLowerCaseField();

            Assert.IsTrue(mapping.HasAttribute("access", "field.lowercase"));
        }

        [Test]
        public void AsLowerCaseField_WithNonePrefix_UsesFieldAsAccessStrategy_AndLowerCaseAsNamingStrategy_AndHasNoPrefix()
        {
            builder.AsLowerCaseField(Prefix.None);

            Assert.IsTrue(mapping.HasAttribute("access", "field.lowercase"));
        }

        [Test, ExpectedException(typeof(InvalidPrefixException), ExpectedMessage = "m is not a valid prefix for a LowerCase Field.")]
        public void AsLowerCaseField_WithMPrefix_ThrowsInvalidPrefixException()
        {
            builder.AsLowerCaseField(Prefix.m);
        }

        [Test, ExpectedException(typeof(InvalidPrefixException), ExpectedMessage = "m_ is not a valid prefix for a LowerCase Field.")]
        public void AsLowerCaseField_WithMUnderscorePrefix_ThrowsInvalidPrefixException()
        {
            builder.AsLowerCaseField(Prefix.mUnderscore);
        }

        [Test]
        public void AsLowerCaseField_WithUnderscorePrefix_UsesFieldAsAccessStrategy_AndLowerCaseAsNamingStrategy_AndHasUnderscorePrefix()
        {
            builder.AsLowerCaseField(Prefix.Underscore);

            Assert.IsTrue(mapping.HasAttribute("access", "field.lowercase-underscore"));
        }

        [Test, ExpectedException(typeof(InvalidPrefixException), ExpectedMessage = "None is not a valid prefix for a PascalCase Field.")]
        public void AsPascalCaseField_WithNonePrefix_ThrowsInvalidPrefixException()
        {
            builder.AsPascalCaseField(Prefix.None);
        }

        [Test]
        public void AsPascalCaseField_WithUnderscorePrefix_UsesFieldAsAccessStrategy_AndPascalCaseAsNamingStrategy_AndHasUnderscorePrefix()
        {
            builder.AsPascalCaseField(Prefix.Underscore);

            Assert.IsTrue(mapping.HasAttribute("access", "field.pascalcase-underscore"));
        }

        [Test]
        public void AsPascalCaseField_WithMPrefix_UsesFieldAsAccessStrategy_AndPascalCaseAsNamingStrategy_AndHasMPrefix()
        {
            builder.AsPascalCaseField(Prefix.m);

            Assert.IsTrue(mapping.HasAttribute("access", "field.pascalcase-m"));
        }

        [Test]
        public void AsPascalCaseField_WithMUnderscorePrefix_UsesFieldAsAccessStrategy_AndPascalCaseAsNamingStrategy_AndHasMUnderscorePrefix()
        {
            builder.AsPascalCaseField(Prefix.mUnderscore);

            Assert.IsTrue(mapping.HasAttribute("access", "field.pascalcase-m-underscore"));
        }

        // nosetter

        [Test]
        public void AsReadOnlyPropertyThroughCamelCaseField_UsesNoSetterAsAccessStrategy_AndCamelCaseAsNamingStrategy()
        {
            builder.AsReadOnlyPropertyThroughCamelCaseField();

            Assert.IsTrue(mapping.HasAttribute("access", "nosetter.camelcase"));
        }

        [Test]
        public void AsReadOnlyPropertyThroughCamelCaseField_WithNonePrefix_UsesNoSetterAsAccessStrategy_AndCamelCaseAsNamingStrategy_AndHasNoPrefix()
        {
            builder.AsReadOnlyPropertyThroughCamelCaseField(Prefix.None);

            Assert.IsTrue(mapping.HasAttribute("access", "nosetter.camelcase"));
        }

        [Test, ExpectedException(typeof(InvalidPrefixException), ExpectedMessage = "m is not a valid prefix for a CamelCase Field.")]
        public void AsReadOnlyPropertyThroughCamelCaseField_WithMPrefix_ThrowsInvalidPrefixException()
        {
            builder.AsReadOnlyPropertyThroughCamelCaseField(Prefix.m);
        }

        [Test, ExpectedException(typeof(InvalidPrefixException), ExpectedMessage = "m_ is not a valid prefix for a CamelCase Field.")]
        public void AsReadOnlyPropertyThroughCamelCaseField_WithMUnderscorePrefix_ThrowsInvalidPrefixException()
        {
            builder.AsReadOnlyPropertyThroughCamelCaseField(Prefix.mUnderscore);
        }

        [Test]
        public void AsReadOnlyPropertyThroughCamelCaseField_WithUnderscorePrefix_UsesNoSetterAsAccessStrategy_AndCamelCaseAsNamingStrategy_AndHasUnderscorePrefix()
        {
            builder.AsReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore);

            Assert.IsTrue(mapping.HasAttribute("access", "nosetter.camelcase-underscore"));
        }

        [Test]
        public void AsReadOnlyPropertyThroughLowerCaseField_UsesNoSetterAsAccessStrategy_AndLowerCaseAsNamingStrategy()
        {
            builder.AsReadOnlyPropertyThroughLowerCaseField();

            Assert.IsTrue(mapping.HasAttribute("access", "nosetter.lowercase"));
        }

        [Test]
        public void AsReadOnlyPropertyThroughLowerCaseField_WithNonePrefix_UsesNoSetterAsAccessStrategy_AndLowerCaseAsNamingStrategy_AndHasNoPrefix()
        {
            builder.AsReadOnlyPropertyThroughLowerCaseField(Prefix.None);

            Assert.IsTrue(mapping.HasAttribute("access", "nosetter.lowercase"));
        }

        [Test, ExpectedException(typeof(InvalidPrefixException), ExpectedMessage = "m is not a valid prefix for a LowerCase Field.")]
        public void AsReadOnlyPropertyThroughLowerCaseField_WithMPrefix_ThrowsInvalidPrefixException()
        {
            builder.AsReadOnlyPropertyThroughLowerCaseField(Prefix.m);
        }

        [Test, ExpectedException(typeof(InvalidPrefixException), ExpectedMessage = "m_ is not a valid prefix for a LowerCase Field.")]
        public void AsReadOnlyPropertyThroughLowerCaseField_WithMUnderscorePrefix_ThrowsInvalidPrefixException()
        {
            builder.AsReadOnlyPropertyThroughLowerCaseField(Prefix.mUnderscore);
        }

        [Test]
        public void AsReadOnlyPropertyThroughLowerCaseField_WithUnderscorePrefix_UsesNoSetterAsAccessStrategy_AndLowerCaseAsNamingStrategy_AndHasUnderscorePrefix()
        {
            builder.AsReadOnlyPropertyThroughLowerCaseField(Prefix.Underscore);

            Assert.IsTrue(mapping.HasAttribute("access", "nosetter.lowercase-underscore"));
        }

        [Test, ExpectedException(typeof(InvalidPrefixException), ExpectedMessage = "None is not a valid prefix for a PascalCase Field.")]
        public void AsReadOnlyPropertyThroughPascalCaseField_WithNonePrefix_ThrowsInvalidPrefixException()
        {
            builder.AsReadOnlyPropertyThroughPascalCaseField(Prefix.None);
        }

        [Test]
        public void AsReadOnlyPropertyThroughPascalCaseField_WithUnderscorePrefix_UsesNoSetterAsAccessStrategy_AndPascalCaseAsNamingStrategy_AndHasUnderscorePrefix()
        {
            builder.AsReadOnlyPropertyThroughPascalCaseField(Prefix.Underscore);

            Assert.IsTrue(mapping.HasAttribute("access", "nosetter.pascalcase-underscore"));
        }

        [Test]
        public void AsReadOnlyPropertyThroughPascalCaseField_WithMPrefix_UsesNoSetterAsAccessStrategy_AndPascalCaseAsNamingStrategy_AndHasMPrefix()
        {
            builder.AsReadOnlyPropertyThroughPascalCaseField(Prefix.m);

            Assert.IsTrue(mapping.HasAttribute("access", "nosetter.pascalcase-m"));
        }

        [Test]
        public void AsReadOnlyPropertyThroughPascalCaseField_WithMUnderscorePrefix_UsesNoSetterAsAccessStrategy_AndPascalCaseAsNamingStrategy_AndHasMUnderscorePrefix()
        {
            builder.AsReadOnlyPropertyThroughPascalCaseField(Prefix.mUnderscore);

            Assert.IsTrue(mapping.HasAttribute("access", "nosetter.pascalcase-m-underscore"));
        }

        [Test]
        public void Using_WithString_SetsAccessToStringValue()
        {
            string className = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            builder.Using(className);

            Assert.IsTrue(mapping.HasAttribute("access", className));
        }

        [Test]
        public void Using_WithType_SetsAccessToAssemblyQualifiedNameOfType()
        {
            string assemblyQualifiedName = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            builder.Using(typeof(FakePropertyAccessor));

            Assert.IsTrue(mapping.HasAttribute("access", assemblyQualifiedName));
        }

        [Test]
        public void UsingGeneric_WithType_SetsAccessToAssemblyQualifiedNameOfType()
        {
            string assemblyQualifiedName = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            builder.Using<FakePropertyAccessor>();

            Assert.IsTrue(mapping.HasAttribute("access", assemblyQualifiedName));
        }
    }

    internal class MappingPartStub : IMappingPart
    {
        private readonly Dictionary<string, string> attributes = new Dictionary<string, string>();

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public void SetAttribute(string name, string value)
        {
            attributes.Add(name, value);
        }

        public void SetAttributes(Attributes attrs)
        {
            foreach (var key in attrs.Keys)
            {
                SetAttribute(key, attrs[key]);
            }
        }

        public int Level
        {
            get { throw new System.NotImplementedException(); }
        }

        public PartPosition Position
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool HasAttribute(string name, string value)
        {
            if (!attributes.ContainsKey(name)) return false;
            
            return attributes[name] == value;
        }
    }
}