using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping.TestFixtures.ComponentTypes;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel;
using NHibernate.Cfg;
using NUnit.Framework;
using SuperTypes = FluentNHibernate.Automapping.TestFixtures.SuperTypes;
using FluentNHibernate.Automapping.TestFixtures;

namespace FluentNHibernate.Testing.Automapping.Apm
{
    [TestFixture]
    public class AutoPersistenceModelTests : BaseAutoPersistenceTests
    {
        [Test]
        public void ShouldOnlyOutputOneClass()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleCustomColumn>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class[2]").DoesntExist();
        }

        [Test]
        public void ShouldGenerateValidXml()
        {
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(x => x.AutoMappings.Add(
                    AutoMap.AssemblyOf<ExampleCustomColumn>()
                        .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                    ))
                .BuildConfiguration();
        }

        [Test]
        public void MapsPropertyWithPropertyConvention()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleCustomColumn>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add<XXAppenderPropertyConvention>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/property[@name='LineOne']/column").HasAttribute("name", "LineOneXX");
        }

        [Test]
        public void TestAutoMapsIds()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleCustomColumn>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").Exists();
        }

        [Test]
        public void TestAutoMapsProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//property[@name='ExampleClassId']").Exists();
        }

        [Test]
        public void AutoMapsEnumProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//property[@name='Enum']").Exists();
        }

        [Test]
        public void TestAutoMapIgnoresProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleCustomColumn>(c => c.IgnoreProperty(p => p.ExampleCustomColumnId));

            new AutoMappingTester<ExampleCustomColumn>(autoMapper)
                .Element("//property").DoesntExist();
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingProperty()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(c => c.Map(x => x.LineOne).Column("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//property[@name='LineOne']")
                    .Exists()
                    .HasThisManyChildNodes(1)
                .Element("//property[@name='LineOne']/column").HasAttribute("name", "test");
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingId()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(c => c.Id(x => x.Id).Column("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//id/column").HasAttribute("name", "test");
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingComponent()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(x => x.IsComponentType = type => type == typeof(ExampleParentClass))
                .ForTypesThatDeriveFrom<ExampleClass>(m => m.Component(x => x.Parent, c => c.Map(x => x.ExampleParentClassId).Column("test")));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//component[@name='Parent']/property[@name='ExampleParentClassId']/column").HasAttribute("name", "test");
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingHasMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleParentClass>(m => m.HasMany(x => x.Examples).Inverse());

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag[@name='Examples']").HasAttribute("inverse", "true");
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingHasManyToMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleParentClass>(m => m.HasManyToMany(x => x.Examples).Inverse());

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag[@name='Examples']").HasAttribute("inverse", "true")
                .Element("//bag[@name='Examples']/many-to-many").Exists();
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingHasOne()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(m => m.HasOne(x => x.Parent));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//one-to-one[@name='Parent']").Exists()
                .Element("//bag[@name='Parent']").DoesntExist();
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingReferences()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(m => m.References(x => x.Parent).Access.Field());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one[@name='Parent']").HasAttribute("access", "field");
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingReferencesAny()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(m =>
                    m.ReferencesAny(x => x.Dictionary)
                        .EntityIdentifierColumn("one")
                        .EntityTypeColumn("two")
                        .IdentityType<int>());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//any[@name='Dictionary']").Exists()
                .Element("//map[@name='Dictionary']").DoesntExist();
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingVersion()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(m => m.Version(x => x.Timestamp).Access.Field());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//version[@name='Timestamp']").HasAttribute("access", "field");
        }

        [Test]
        public void TestAutoMapManyToOne()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one").HasAttribute("name", "Parent");
        }

        [Test]
        public void TestAutoMapOneToMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag")
                .HasAttribute("name", "Examples");
        }

        [Test]
        public void TestAutoMapPropertyMergeOverridesId()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(map => map.Id(c => c.Id, "Column"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").HasAttribute("name", "Id")
                .Element("class/id/column").HasAttribute("name", "Column");
        }

        [Test]
        public void TestAutoMapPropertySetPrimaryKeyConvention()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add(PrimaryKey.Name.Is(id => id.Property.Name + "Id"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").HasAttribute("name", "Id")
                .Element("class/id/column").HasAttribute("name", "IdId");
        }

        [Test]
        public void TestAutoMapIdUsesConvention()
        {
            var autoMapper = AutoMap.AssemblyOf<PrivateIdSetterClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add(new TestIdConvention());

            new AutoMappingTester<PrivateIdSetterClass>(autoMapper)
                .Element("class/id/column").HasAttribute("name", "test");
        }

        [Test]
        public void AppliesConventionsToManyToOne()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add(new TestM2OConvention());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one/column").HasAttribute("name", "test");
        }

        [Test]
        public void AppliesConventionsToOneToMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add(new TestO2MConvention());

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag").HasAttribute("table", "test");
        }

        [Test]
        public void TestAutoMapPropertySetFindPrimaryKeyConvention()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t == typeof(ExampleClass))
                .Setup(c => c.FindIdentity = p => p.Name == p.DeclaringType.Name + "Id");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").HasAttribute("name", "ExampleClassId")
                .Element("class/id/column").HasAttribute("name", "ExampleClassId");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Could not find mapping for class 'SuperType'")]
        public void TestInheritanceMappingSkipsSuperTypes()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures.SuperTypes")
                .Setup(c =>
                {
                    c.IsBaseType = b => b == typeof(SuperTypes.SuperType);
                });

            new AutoMappingTester<SuperTypes.SuperType>(autoMapper);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Could not find mapping for class 'SuperType'")]
        public void TestInheritanceSubclassMappingSkipsSuperTypes()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures.SuperTypes")
                .Setup(c =>
                {
                    c.IsBaseType = b => b == typeof(SuperTypes.SuperType);
                    c.SubclassStrategy = t => SubclassStrategy.Subclass;
                });

            new AutoMappingTester<SuperTypes.SuperType>(autoMapper);
        }

        [Test]
        public void TestInheritanceMapping()
        {
            AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");
        }

        [Test]
        public void TestInheritanceSubclassMapping()
        {
            AutoMap.AssemblyOf<ExampleClass>()
                .Setup(x => x.SubclassStrategy = t => SubclassStrategy.Subclass)
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");
        }

        [Test]
        public void TestInheritanceMappingProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/property[@name='ExampleProperty']").Exists();
        }

        [Test]
        public void TestInheritanceSubclassMappingProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Setup(x => x.SubclassStrategy = t => SubclassStrategy.Subclass)
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            var tester = new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass/property[@name='ExampleProperty']").Exists();
        }

        [Test]
        public void TestDoNotAddJoinedSubclassesForConcreteBaseTypes()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(c =>
                           c.IsConcreteBaseType = b =>
                                                  b == typeof(ExampleClass));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass").DoesntExist();
        }

        [Test]
        public void TestDoNotAddSubclassesForConcreteBaseTypes()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(c =>
                {
                    c.IsConcreteBaseType = b => b == typeof(ExampleClass);
                    c.SubclassStrategy = t => SubclassStrategy.Subclass;
                });

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass").DoesntExist();
        }

        [Test]
        public void TestClassIsMappedForConcreteSubClass()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(c =>
                           c.IsConcreteBaseType = b =>
                                                  b == typeof(ExampleClass));

            new AutoMappingTester<ExampleInheritedClass>(autoMapper)
                .Element("class")
                .HasAttribute("name", typeof(ExampleInheritedClass).AssemblyQualifiedName)
                .Exists();
        }

        [Test]
        public void TestClassIsMappedForConcreteSubClassWithSubclass()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(c =>
                {
                    c.IsConcreteBaseType = b => b == typeof(ExampleClass);
                    c.SubclassStrategy = t => SubclassStrategy.Subclass;
                });

            new AutoMappingTester<ExampleInheritedClass>(autoMapper)
                .Element("class")
                .HasAttribute("name", typeof(ExampleInheritedClass).AssemblyQualifiedName)
                .Exists();
        }

        [Test]
        public void TestInheritanceMappingDoesntIncludeBaseTypeProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceMappingDoesntIncludeBaseTypePropertiesWithSubclass()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Setup(x => x.SubclassStrategy = t => SubclassStrategy.Subclass)
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceOverridingMappingProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .ForTypesThatDeriveFrom<ExampleClass>(t => t.JoinedSubClass<ExampleInheritedClass>("OverridenKey", p => p.Map(c => c.ExampleProperty, "columnName")))
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceSubclassOverridingMappingProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Setup(x => x.SubclassStrategy = t => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleClass>(t => t.SubClass<ExampleInheritedClass>("discriminator", p => p.Map(c => c.ExampleProperty, "columnName")))
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestAutoMapClassAppliesConventions()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add(new TestClassConvention());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class").HasAttribute("table", "test");
        }

        [Test]
        public void CanSearchForOpenGenericTypes()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            autoMapper.CompileMappings();
            autoMapper.FindMapping(typeof(SomeOpenGenericType<>));
        }

        [Test]
        public void TypeConventionShouldForcePropertyToBeMapped()
        {
            var autoMapper = AutoMap.AssemblyOf<ClassWithUserType>()
                .Conventions.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ClassWithUserType>(autoMapper)
                .Element("class/property").HasAttribute("name", "Custom");
        }

        [Test]
        public void ComponentTypesAutoMapped()
        {
            var autoMapper = AutoMap.AssemblyOf<Customer>()
                .Setup(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                })
                .Conventions.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='HomeAddress']").Exists()
                .Element("class/component[@name='WorkAddress']").Exists();
        }

        [Test]
        public void ComponentPropertiesAutoMapped()
        {
            var autoMapper = AutoMap.AssemblyOf<Customer>()
                .Setup(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                })
                .Conventions.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/property[@name='Number']").Exists()
                .Element("class/component/property[@name='Street']").Exists();
        }

        [Test]
        public void ComponentPropertiesWithUserTypeAutoMapped()
        {
            var autoMapper = AutoMap.AssemblyOf<Customer>()
                .Setup(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                })
                .Conventions.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/property[@name='Custom']").HasAttribute("type", typeof(CustomUserType).AssemblyQualifiedName);
        }

        [Test]
        public void ComponentPropertiesAssumeComponentColumnPrefix()
        {
            var autoMapper = AutoMap.AssemblyOf<Customer>()
                .Setup(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                    convention.GetComponentColumnPrefix =
                        property => property.Name + "_";
                })
                .Conventions.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='WorkAddress']/property[@name='Number']/column").HasAttribute("name", "WorkAddress_Number");
        }

        [Test]
        public void ComponentColumnConventionReceivesProperty()
        {
            var autoMapper = AutoMap.AssemblyOf<Customer>()
                .Setup(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                    convention.GetComponentColumnPrefix =
                        property => property.Name + "_";
                })
                .Conventions.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='WorkAddress']/property[@name='Number']/column")
                .HasAttribute("name", value => value.StartsWith("WorkAddress_"));
        }

        [Test]
        public void IdIsMappedFromGenericBaseClass()
        {
            var autoMapper = AutoMap.AssemblyOf<ClassUsingGenericBase>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(convention =>
                {
                    convention.IsBaseType =
                        type => type == typeof(EntityBase<>);
                });

            new AutoMappingTester<ClassUsingGenericBase>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "Id");
        }

        [Test]
        public void OverriddenSubclassIsMerged()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            autoMapper.CompileMappings();
            var mappings = autoMapper.BuildMappings();
            var classes = mappings.Select(x => x.Classes.First());

            // no separate mapping for ExampleInheritedClass
            classes.FirstOrDefault(c => c.Type == typeof(ExampleInheritedClass))
                .ShouldBeNull();

            var example = classes.FirstOrDefault(c => c.Type == typeof(ExampleClass));

            example.ShouldNotBeNull();
        }

        [Test]
        public void SubclassShouldntRemapPropertiesMappedInParent()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/property[@name='LineOne']").Exists()
                .Element("class/joined-subclass/property[@name='LineOne']").DoesntExist();
        }

        [Test]
        public void OverriddenSubclassIsAppliedToXml()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/bag")
                .HasAttribute("inverse", "true");
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingProperty()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(c => c.Map(x => x.ExampleProperty).Column("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/property[@name='ExampleProperty']")
                    .Exists()
                    .HasThisManyChildNodes(1)
                .Element("//joined-subclass/property[@name='ExampleProperty']/column").HasAttribute("name", "test");
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingComponent()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(x => x.IsComponentType = type => type == typeof(ExampleParentClass))
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.Component(x => x.Component, c => c.Map(x => x.ExampleParentClassId).Column("test")));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/component[@name='Component']/property[@name='ExampleParentClassId']/column").HasAttribute("name", "test");
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingHasMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/bag[@name='Children']").HasAttribute("inverse", "true");
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingHasManyToMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasManyToMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/bag[@name='Children']").HasAttribute("inverse", "true")
                .Element("//joined-subclass/bag[@name='Children']/many-to-many").Exists();
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingHasOne()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasOne(x => x.Parent));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/one-to-one[@name='Parent']").Exists()
                .Element("//joined-subclass/bag[@name='Parent']").DoesntExist();
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingReferences()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.References(x => x.Parent).Access.Field());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/many-to-one[@name='Parent']").HasAttribute("access", "field");
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingReferencesAny()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m =>
                    m.ReferencesAny(x => x.DictionaryChild)
                        .EntityIdentifierColumn("one")
                        .EntityTypeColumn("two")
                        .IdentityType<int>());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/any[@name='DictionaryChild']").Exists()
                .Element("//joined-subclass/map[@name='DictionaryChild']").DoesntExist();
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingProperty()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(c => c.Map(x => x.ExampleProperty).Column("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/property[@name='ExampleProperty']")
                    .Exists()
                    .HasThisManyChildNodes(1)
                .Element("//subclass/property[@name='ExampleProperty']/column").HasAttribute("name", "test");
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingComponent()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(x =>
                {
                    x.SubclassStrategy = type => SubclassStrategy.Subclass;
                    x.IsComponentType = type => type == typeof(ExampleParentClass);
                })
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.Component(x => x.Component, c => c.Map(x => x.ExampleParentClassId).Column("test")));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/component[@name='Component']/property[@name='ExampleParentClassId']/column").HasAttribute("name", "test");
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingHasMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/bag[@name='Children']").HasAttribute("inverse", "true");
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingHasManyToMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasManyToMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/bag[@name='Children']").HasAttribute("inverse", "true")
                .Element("//subclass/bag[@name='Children']/many-to-many").Exists();
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingHasOne()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasOne(x => x.Parent));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/one-to-one[@name='Parent']").Exists()
                .Element("//subclass/bag[@name='Parent']").DoesntExist();
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingReferences()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.References(x => x.Parent).Access.Field());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/many-to-one[@name='Parent']").HasAttribute("access", "field");
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingReferencesAny()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m =>
                    m.ReferencesAny(x => x.DictionaryChild)
                        .EntityIdentifierColumn("one")
                        .EntityTypeColumn("two")
                        .IdentityType<int>());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/any[@name='DictionaryChild']").Exists()
                .Element("//subclass/map[@name='DictionaryChild']").DoesntExist();
        }

        [Test]
        public void ShouldBeAbleToOverrideKeyColumnNameOfJoinedSubclassInConvention()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add<JoinedSubclassConvention>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/key/column")
                .HasAttribute("name", "test");
        }

        [Test]
        public void ShouldBeAbleToOverrideTableNameOfJoinedSubclassInConvention()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add<JoinedSubclassConvention>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .HasAttribute("table", "test-table");
        }

        [Test]
        public void ShouldBeAbleToIgnoreComponentProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(s => s.IsComponentType = type => type == typeof(ExampleParentClass))
                .ForTypesThatDeriveFrom<ExampleParentClass>(t => t.IgnoreProperty(x => x.ExampleParentClassId));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/component/property[@name='ExampleParentClassId']").DoesntExist();
        }

        [Test]
        public void ShouldBeAbleToMapComponentHasMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(s => s.IsComponentType = type => type == typeof(ExampleParentClass));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/component/bag[@name='Examples']").Exists();
        }

        [Test]
        public void ComponentHasManyShouldHavePrefixedKeyColumn()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(s =>
                {
                    s.IsComponentType = type => type == typeof(ExampleParentClass);
                    s.GetComponentColumnPrefix = prop => prop.Name + "_";
                });

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/component/bag[@name='Examples']/key/column").HasAttribute("name", "Parent_ExampleParentClass_Id");
        }

        [Test]
        public void ShouldBeAbleToIgnorePropertiesRegardlessOfType()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForAllTypes(t => t.IgnoreProperty("Dummy"));

            new AutoMappingTester<ClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist();

            new AutoMappingTester<AnotherClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist();
        }

        [Test]
        public void ShouldBeAbleToIgnoreMultiplePropertiesRegardlessOfType()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForAllTypes(t => t.IgnoreProperties("Dummy", "Dummy1", "Dummy2"));

            new AutoMappingTester<ClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist()
                .Element("class/property[@name='Dummy1']").DoesntExist()
                .Element("class/property[@name='Dummy2']").DoesntExist();

            new AutoMappingTester<AnotherClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist()
                .Element("class/property[@name='Dummy1']").DoesntExist()
                .Element("class/property[@name='Dummy2']").DoesntExist();
        }

        [Test]
        public void ShouldBeAbleToIgnoreMultiplePropertiesByDelegateRegardlessOfType()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .ForAllTypes(t => t.IgnoreProperties(x => x.Name.Contains("Dummy")));

            new AutoMappingTester<ClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist()
                .Element("class/property[@name='Dummy1']").DoesntExist()
                .Element("class/property[@name='Dummy2']").DoesntExist();

            new AutoMappingTester<AnotherClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist()
                .Element("class/property[@name='Dummy1']").DoesntExist()
                .Element("class/property[@name='Dummy2']").DoesntExist();
        }

        [Test]
        public void ShouldAllowIgnoresInOverrides()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .UseOverridesFromAssemblyOf<IgnorerOverride>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/property[@name='LineOne']").DoesntExist();
        }

        [Test]
        public void ShouldntAutoMapComponentsThatArentExcludedByWhereClause()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Setup(x => x.IsComponentType = type => type == typeof(ExampleParentClass))
                .Where(x => x.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            autoMapper.CompileMappings();
            var mappings = autoMapper.BuildMappings();

            var exampleClassMapping = mappings
                .SelectMany(x => x.Classes)
                .First(x => x.Type == typeof(ExampleClass));

            exampleClassMapping.Components.Count().ShouldEqual(1); // has a component

            var exampleParentClassMapping = mappings
                .SelectMany(x => x.Classes)
                .FirstOrDefault(x => x.Type == typeof(ExampleParentClass));

            exampleParentClassMapping.ShouldBeNull();
        }

        [Test]
        public void ShouldEscapeTableNames()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class").HasAttribute("table", "`ExampleClass`");
        }

        [Test]
        public void ShouldUseGuidCombGeneratorForGuidIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithGuidId)));

            new AutoMappingTester<ClassWithGuidId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "guid.comb");
        }

        [Test]
        public void ShouldUseIdentityGeneratorForIntIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithIntId)));

            new AutoMappingTester<ClassWithIntId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "identity");
        }

        [Test]
        public void ShouldUseIdentityGeneratorForLongIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithLongId)));

            new AutoMappingTester<ClassWithLongId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "identity");
        }

        [Test]
        public void ShouldUseAssignedGeneratorForStringIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithStringId)));

            new AutoMappingTester<ClassWithStringId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "assigned");
        }

        [Test]
        public void ShouldOverrideGeneratorWithConventions()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithLongId)))
                .Conventions.Add<TestIdGeneratorConvention>(); ;

            new AutoMappingTester<ClassWithLongId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "assigned");
        }

        private class JoinedSubclassConvention : IJoinedSubclassConvention
        {
            public void Apply(IJoinedSubclassInstance instance)
            {
                instance.Table("test-table");
                instance.Key.Column("test");
            }
        }

        private class TestIdConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.Column("test");
            }
        }

        private class TestIdGeneratorConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.GeneratedBy.Assigned();
            }
        }

        private class TestClassConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.Table("test");
            }
        }

        private class TestM2OConvention : IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                instance.Column("test");
            }
        }

        private class TestO2MConvention : IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.Table("test");
            }
        }
    }

    //public class StubAssembly : Assembly
    //{
    //    private readonly IEnumerable<Type> _types;

    //    public static Assembly Containing<T>()
    //    {
    //        return new StubAssembly(new[] { typeof(T) });
    //    }

    //    public StubAssembly()
    //    {
    //        new Assembly();

    //        _types = types;
    //    }
    //}

    public class IgnorerOverride : IAutoMappingOverride<ExampleClass>
    {
        public void Override(AutoMapping<ExampleClass> mapping)
        {
            mapping.IgnoreProperty(x => x.LineOne);
        }
    }

    public class SomeOpenGenericType<T>
    {}
}
