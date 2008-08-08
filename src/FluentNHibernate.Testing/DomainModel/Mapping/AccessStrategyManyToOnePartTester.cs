using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class AccessStrategyManyToOnePartTester
    {
        [Test]
        public void AccessAsProperty_SetsAccessStrategyToProperty()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsProperty())
                .Element("class/bag").HasAttribute("access", "property");
        }

        [Test]
        public void AccessAsField_SetsAccessStrategyToField()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsField())
                .Element("class/bag").HasAttribute("access", "field");
        }

        [Test]
        public void AccessAsCamelCaseField_SetsAccessStrategyToField_and_SetsNamingStrategyToCamelCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsCamelCaseField())
                .Element("class/bag").HasAttribute("access", "field.camelcase");
        }

        [Test]
        public void AccessAsCamelCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToCamelCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsCamelCaseField(Prefix.Underscore))
                .Element("class/bag").HasAttribute("access", "field.camelcase-underscore");
        }

        [Test]
        public void AccessAsLowerCaseField_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsLowerCaseField())
                .Element("class/bag").HasAttribute("access", "field.lowercase");
        }

        [Test]
        public void AccessAsLowerCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsLowerCaseField(Prefix.Underscore))
                .Element("class/bag").HasAttribute("access", "field.lowercase-underscore");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToPascalCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsPascalCaseField(Prefix.Underscore))
                .Element("class/bag").HasAttribute("access", "field.pascalcase-underscore");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithMPrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseM()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsPascalCaseField(Prefix.m))
                .Element("class/bag").HasAttribute("access", "field.pascalcase-m");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithMUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseMUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsPascalCaseField(Prefix.mUnderscore))
                .Element("class/bag").HasAttribute("access", "field.pascalcase-m-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughCamelCaseField_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToCamelCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsReadOnlyPropertyThroughCamelCaseField())
                .Element("class/bag").HasAttribute("access", "nosetter.camelcase");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughCamelCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToCamelCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore))
                .Element("class/bag").HasAttribute("access", "nosetter.camelcase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughLowerCaseField_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToLowerCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsReadOnlyPropertyThroughLowerCaseField())
                .Element("class/bag").HasAttribute("access", "nosetter.lowercase");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughLowerCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToLowerCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsReadOnlyPropertyThroughLowerCaseField(Prefix.Underscore))
                .Element("class/bag").HasAttribute("access", "nosetter.lowercase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsReadOnlyPropertyThroughPascalCaseField(Prefix.Underscore))
                .Element("class/bag").HasAttribute("access", "nosetter.pascalcase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithMPrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseM()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsReadOnlyPropertyThroughPascalCaseField(Prefix.m))
                .Element("class/bag").HasAttribute("access", "nosetter.pascalcase-m");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithMUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseMUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.AsReadOnlyPropertyThroughPascalCaseField(Prefix.mUnderscore))
                .Element("class/bag").HasAttribute("access", "nosetter.pascalcase-m-underscore");
        }

        [Test]
        public void AccessUsingClassName_SetsAccessAttributeToClassName()
        {
            string className = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.Using(className))
                .Element("class/bag").HasAttribute("access", className);
        }

        [Test]
        public void AccessUsingClassType_SetsAccessAttributeToAssemblyQualifiedName()
        {
            var className = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.Using(typeof(FakePropertyAccessor)))
                .Element("class/bag").HasAttribute("access", className);
        }

        [Test]
        public void AccessUsingClassGenericParameter_SetsAccessAttributeToAssemblyQualifiedName()
        {
            var className = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.HasMany<PropertyReferenceTarget>(x => x.References).Access.Using<FakePropertyAccessor>())
                .Element("class/bag").HasAttribute("access", className);
        }
    }
}