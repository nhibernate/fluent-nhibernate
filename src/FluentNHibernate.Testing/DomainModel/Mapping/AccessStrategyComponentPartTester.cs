using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class AccessStrategyComponentPartTester
    {
        [Test]
        public void AccessAsProperty_SetsAccessStrategyToProperty()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsProperty())
                .Element("class/component").HasAttribute("access", "property");
        }

        [Test]
        public void AccessAsField_SetsAccessStrategyToField()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsField())
                .Element("class/component").HasAttribute("access", "field");
        }

        [Test]
        public void AccessAsCamelCaseField_SetsAccessStrategyToField_and_SetsNamingStrategyToCamelCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsCamelCaseField())
                .Element("class/component").HasAttribute("access", "field.camelcase");
        }

        [Test]
        public void AccessAsCamelCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToCamelCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsCamelCaseField(Prefix.Underscore))
                .Element("class/component").HasAttribute("access", "field.camelcase-underscore");
        }

        [Test]
        public void AccessAsLowerCaseField_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsLowerCaseField())
                .Element("class/component").HasAttribute("access", "field.lowercase");
        }

        [Test]
        public void AccessAsLowerCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsLowerCaseField(Prefix.Underscore))
                .Element("class/component").HasAttribute("access", "field.lowercase-underscore");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToPascalCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsPascalCaseField(Prefix.Underscore))
                .Element("class/component").HasAttribute("access", "field.pascalcase-underscore");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithMPrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseM()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsPascalCaseField(Prefix.m))
                .Element("class/component").HasAttribute("access", "field.pascalcase-m");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithMUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseMUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsPascalCaseField(Prefix.mUnderscore))
                .Element("class/component").HasAttribute("access", "field.pascalcase-m-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughCamelCaseField_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToCamelCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsReadOnlyPropertyThroughCamelCaseField())
                .Element("class/component").HasAttribute("access", "nosetter.camelcase");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughCamelCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToCamelCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore))
                .Element("class/component").HasAttribute("access", "nosetter.camelcase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughLowerCaseField_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToLowerCase()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsReadOnlyPropertyThroughLowerCaseField())
                .Element("class/component").HasAttribute("access", "nosetter.lowercase");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughLowerCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToLowerCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsReadOnlyPropertyThroughLowerCaseField(Prefix.Underscore))
                .Element("class/component").HasAttribute("access", "nosetter.lowercase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsReadOnlyPropertyThroughPascalCaseField(Prefix.Underscore))
                .Element("class/component").HasAttribute("access", "nosetter.pascalcase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithMPrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseM()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsReadOnlyPropertyThroughPascalCaseField(Prefix.m))
                .Element("class/component").HasAttribute("access", "nosetter.pascalcase-m");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithMUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseMUnderscore()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.AsReadOnlyPropertyThroughPascalCaseField(Prefix.mUnderscore))
                .Element("class/component").HasAttribute("access", "nosetter.pascalcase-m-underscore");
        }

        [Test]
        public void AccessUsingClassName_SetsAccessAttributeToClassName()
        {
            string className = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.Using(className))
                .Element("class/component").HasAttribute("access", className);
        }

        [Test]
        public void AccessUsingClassType_SetsAccessAttributeToAssemblyQualifiedName()
        {
            var className = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.Using(typeof(FakePropertyAccessor)))
                .Element("class/component").HasAttribute("access", className);
        }

        [Test]
        public void AccessUsingClassGenericParameter_SetsAccessAttributeToAssemblyQualifiedName()
        {
            var className = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            new MappingTester<PropertyTarget>()
                .ForMapping(c => c.Component(x => x.Component, m => m.Map(x => x.Name)).Access.Using<FakePropertyAccessor>())
                .Element("class/component").HasAttribute("access", className);
        }
    }
}