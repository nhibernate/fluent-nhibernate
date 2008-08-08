using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class DefaultAccessStrategyClassMapTester
    {
        [Test]
        public void AccessAsProperty_SetsAccessStrategyToProperty()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsProperty())
                .RootElement.HasAttribute("default-access", "property");
        }

        [Test]
        public void AccessAsField_SetsAccessStrategyToField()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsField())
                .RootElement.HasAttribute("default-access", "field");
        }

        [Test]
        public void AccessAsCamelCaseField_SetsAccessStrategyToField_and_SetsNamingStrategyToCamelCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsCamelCaseField())
                .RootElement.HasAttribute("default-access", "field.camelcase");
        }

        [Test]
        public void AccessAsCamelCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToCamelCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsCamelCaseField(Prefix.Underscore))
                .RootElement.HasAttribute("default-access", "field.camelcase-underscore");
        }

        [Test]
        public void AccessAsLowerCaseField_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsLowerCaseField())
                .RootElement.HasAttribute("default-access", "field.lowercase");
        }

        [Test]
        public void AccessAsLowerCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsLowerCaseField(Prefix.Underscore))
                .RootElement.HasAttribute("default-access", "field.lowercase-underscore");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToPascalCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsPascalCaseField(Prefix.Underscore))
                .RootElement.HasAttribute("default-access", "field.pascalcase-underscore");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithMPrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseM()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsPascalCaseField(Prefix.m))
                .RootElement.HasAttribute("default-access", "field.pascalcase-m");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithMUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseMUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsPascalCaseField(Prefix.mUnderscore))
                .RootElement.HasAttribute("default-access", "field.pascalcase-m-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughCamelCaseField_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToCamelCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsReadOnlyPropertyThroughCamelCaseField())
                .RootElement.HasAttribute("default-access", "nosetter.camelcase");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughCamelCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToCamelCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore))
                .RootElement.HasAttribute("default-access", "nosetter.camelcase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughLowerCaseField_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToLowerCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsReadOnlyPropertyThroughLowerCaseField())
                .RootElement.HasAttribute("default-access", "nosetter.lowercase");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughLowerCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToLowerCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsReadOnlyPropertyThroughLowerCaseField(Prefix.Underscore))
                .RootElement.HasAttribute("default-access", "nosetter.lowercase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsReadOnlyPropertyThroughPascalCaseField(Prefix.Underscore))
                .RootElement.HasAttribute("default-access", "nosetter.pascalcase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithMPrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseM()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsReadOnlyPropertyThroughPascalCaseField(Prefix.m))
                .RootElement.HasAttribute("default-access", "nosetter.pascalcase-m");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithMUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseMUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.AsReadOnlyPropertyThroughPascalCaseField(Prefix.mUnderscore))
                .RootElement.HasAttribute("default-access", "nosetter.pascalcase-m-underscore");
        }

        [Test]
        public void AccessUsingClassName_SetsAccessAttributeToClassName()
        {
            string className = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.Using(className))
                .RootElement.HasAttribute("default-access", className);
        }

        [Test]
        public void AccessUsingClassType_SetsAccessAttributeToAssemblyQualifiedName()
        {
            var className = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.Using(typeof(FakePropertyAccessor)))
                .RootElement.HasAttribute("default-access", className);
        }

        [Test]
        public void AccessUsingClassGenericParameter_SetsAccessAttributeToAssemblyQualifiedName()
        {
            var className = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.DefaultAccess.Using<FakePropertyAccessor>())
                .RootElement.HasAttribute("default-access", className);
        }
    }
}