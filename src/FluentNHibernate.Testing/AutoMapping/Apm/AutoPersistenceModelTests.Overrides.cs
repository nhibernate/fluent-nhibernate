using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm
{
    [TestFixture]
    public partial class AutoPersistenceModelTests : BaseAutoPersistenceTests
    {
        [Test]
        public void NaturalIdOverrideShouldOverrideExistingProperty() 
        { 
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>() 
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures") 
                .Override<ExampleClass>(c => c.NaturalId().Property(x => x.LineOne, "test")); 

            new AutoMappingTester<ExampleClass>(autoMapper) 
                .Element("//natural-id/property[@name='LineOne']") 
                .Exists() 
                .HasThisManyChildNodes(1) 
                .Element("//natural-id/property[@name='LineOne']/column").HasAttribute("name", "test"); 
        } 

        [Test]
        public void OverrideShouldOverrideExistingProperty()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleClass>(c => c.Map(x => x.LineOne).Column("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//property[@name='LineOne']")
                .Exists()
                .HasThisManyChildNodes(1)
                .Element("//property[@name='LineOne']/column").HasAttribute("name", "test");
        }

        [Test]
        public void OverrideShouldOverrideExistingId()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleClass>(c => c.Id(x => x.Id).Column("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//id/column").HasAttribute("name", "test");
        }

        [Test]
        public void OverrideShouldOverrideExistingHasMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleParentClass>(m => m.HasMany(x => x.Examples).Inverse());

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag[@name='Examples']").HasAttribute("inverse", "true");
        }

        [Test]
        public void OverrideShouldOverrideExistingHasManyToMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleParentClass>(m => m.HasManyToMany(x => x.Examples).Inverse());

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag[@name='Examples']").HasAttribute("inverse", "true")
                .Element("//bag[@name='Examples']/many-to-many").Exists();
        }

        [Test]
        public void OverrideShouldOverrideExistingHasOne()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleClass>(m => m.HasOne(x => x.Parent));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//one-to-one[@name='Parent']").Exists()
                .Element("//bag[@name='Parent']").DoesntExist();
        }

        [Test]
        public void OverrideShouldOverrideExistingReferences()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleClass>(m => m.References(x => x.Parent).Access.Field());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one[@name='Parent']").HasAttribute("access", "field");
        }

        [Test]
        public void OverrideShouldOverrideExistingReferencesAny()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleClass>(m =>
                    m.ReferencesAny(x => x.Dictionary)
                        .EntityIdentifierColumn("one")
                        .EntityTypeColumn("two")
                        .IdentityType<int>());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//any[@name='Dictionary']").Exists()
                .Element("//map[@name='Dictionary']").DoesntExist();
        }

        [Test]
        public void OverrideShouldOverrideExistingVersion()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleClass>(m => m.Version(x => x.Timestamp).Access.Field());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//version[@name='Timestamp']").HasAttribute("access", "field");
        }

        [Test]
        public void TestAutoMapPropertyMergeOverridesId()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleClass>(map => map.Id(c => c.Id, "Column"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").HasAttribute("name", "Id")
                .Element("class/id/column").HasAttribute("name", "Column");
        }

        [Test]
        public void OverriddenSubclassIsMerged()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            autoMapper.BuildMappings();
            var mappings = autoMapper.BuildMappings();
            var classes = mappings.Select(x => x.Classes.First());

            // no separate mapping for ExampleInheritedClass
            classes.FirstOrDefault(c => c.Type == typeof(ExampleInheritedClass))
                .ShouldBeNull();

            var example = classes.FirstOrDefault(c => c.Type == typeof(ExampleClass));

            example.ShouldNotBeNull();
        }

        [Test]
        public void OverriddenSubclassIsAppliedToXml()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/bag")
                .HasAttribute("inverse", "true");
        }

        [Test]
        public void JoinedSubclassOverrideShouldOverrideExistingProperty()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleInheritedClass>(c => c.Map(x => x.ExampleProperty).Column("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/property[@name='ExampleProperty']")
                    .Exists()
                    .HasThisManyChildNodes(1)
                .Element("//joined-subclass/property[@name='ExampleProperty']/column").HasAttribute("name", "test");
        }

        [Test]
        public void JoinedSubclassOverrideShouldOverrideExistingHasMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/bag[@name='Children']").HasAttribute("inverse", "true");
        }

        [Test]
        public void JoinedSubclassOverrideShouldOverrideExistingHasManyToMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleInheritedClass>(m => m.HasManyToMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/bag[@name='Children']").HasAttribute("inverse", "true")
                .Element("//joined-subclass/bag[@name='Children']/many-to-many").Exists();
        }

        [Test]
        public void JoinedSubclassOverrideShouldOverrideExistingHasOne()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleInheritedClass>(m => m.HasOne(x => x.Parent));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/one-to-one[@name='Parent']").Exists()
                .Element("//joined-subclass/bag[@name='Parent']").DoesntExist();
        }

        [Test]
        public void JoinedSubclassOverrideShouldOverrideExistingReferences()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleInheritedClass>(m => m.References(x => x.Parent).Access.Field());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/many-to-one[@name='Parent']").HasAttribute("access", "field");
        }

        [Test]
        public void JoinedSubclassOverrideShouldOverrideExistingReferencesAny()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleInheritedClass>(m =>
                    m.ReferencesAny(x => x.DictionaryChild)
                        .EntityIdentifierColumn("one")
                        .EntityTypeColumn("two")
                        .IdentityType<int>());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/any[@name='DictionaryChild']").Exists()
                .Element("//joined-subclass/map[@name='DictionaryChild']").DoesntExist();
        }

        [Test]
        public void SubclassOverrideShouldOverrideExistingProperty()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
#pragma warning disable 612,618
                .Setup(x => x.IsDiscriminated = type => true)
#pragma warning restore 612,618
                .Override<ExampleInheritedClass>(c => c.Map(x => x.ExampleProperty).Column("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/property[@name='ExampleProperty']")
                    .Exists()
                    .HasThisManyChildNodes(1)
                .Element("//subclass/property[@name='ExampleProperty']/column").HasAttribute("name", "test");
        }

        [Test]
        public void SubclassOverrideShouldOverrideExistingComponent()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
#pragma warning disable 612,618
                .Setup(x =>
                {
                    x.IsDiscriminated = type => true;
                    x.IsComponentType = type => type == typeof(ExampleParentClass);
                })
#pragma warning restore 612,618
                .Override<ExampleInheritedClass>(m => m.Component(x => x.Component, c => c.Map(x => x.ExampleParentClassId).Column("test")));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/component[@name='Component']/property[@name='ExampleParentClassId']/column").HasAttribute("name", "test");
        }

        [Test]
        public void SubclassOverrideShouldOverrideExistingHasMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
#pragma warning disable 612,618
                .Setup(x => x.IsDiscriminated = type => true)
#pragma warning restore 612,618
                .Override<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/bag[@name='Children']").HasAttribute("inverse", "true");
        }

        [Test]
        public void SubclassOverrideShouldOverrideExistingHasManyToMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
#pragma warning disable 612,618
                .Setup(x => x.IsDiscriminated = type => true)
#pragma warning restore 612,618
                .Override<ExampleInheritedClass>(m => m.HasManyToMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/bag[@name='Children']").HasAttribute("inverse", "true")
                .Element("//subclass/bag[@name='Children']/many-to-many").Exists();
        }

        [Test]
        public void SubclassOverrideShouldOverrideExistingHasOne()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
#pragma warning disable 612,618
                .Setup(x => x.IsDiscriminated = type => true)
#pragma warning restore 612,618
                .Override<ExampleInheritedClass>(m => m.HasOne(x => x.Parent));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/one-to-one[@name='Parent']").Exists()
                .Element("//subclass/bag[@name='Parent']").DoesntExist();
        }

        [Test]
        public void SubclassOverrideShouldOverrideExistingReferences()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
#pragma warning disable 612,618
                .Setup(x => x.IsDiscriminated = type => true)
#pragma warning restore 612,618
                .Override<ExampleInheritedClass>(m => m.References(x => x.Parent).Access.Field());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/many-to-one[@name='Parent']").HasAttribute("access", "field");
        }

        [Test]
        public void SubclassOverrideShouldOverrideExistingReferencesAny()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
#pragma warning disable 612,618
                .Setup(x => x.IsDiscriminated = type => true)
#pragma warning restore 612,618
                .Override<ExampleInheritedClass>(m =>
                    m.ReferencesAny(x => x.DictionaryChild)
                        .EntityIdentifierColumn("one")
                        .EntityTypeColumn("two")
                        .IdentityType<int>());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/any[@name='DictionaryChild']").Exists()
                .Element("//subclass/map[@name='DictionaryChild']").DoesntExist();
        }
    }
}