using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures.ComponentTypes;
using FluentNHibernate.AutoMap.TestFixtures.CustomTypes;
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
using SuperTypes = FluentNHibernate.AutoMap.TestFixtures.SuperTypes;
using FluentNHibernate.AutoMap.TestFixtures;

namespace FluentNHibernate.Testing.AutoMap.Apm
{
    [TestFixture]
    public class AutoPersistenceModelTests : BaseAutoPersistenceTests
    {
        [Test]
        public void ShouldOnlyOutputOneClass()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleCustomColumn>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class[2]").DoesntExist();
        }

        [Test]
        public void ShouldGenerateValidXml()
        {
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(x => x.AutoMappings.Add(
                    AutoPersistenceModel
                        .MapEntitiesFromAssemblyOf<ExampleCustomColumn>()
                        .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                    ))
                .BuildConfiguration();
        }

        [Test]
        public void CanMixMappingTypes()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");
            autoMapper.AddMappingsFromAssembly(typeof(ExampleClass).Assembly);
            autoMapper.Configure(cfg);

            cfg.ClassMappings.ShouldContain(c => c.ClassName == typeof(ExampleClass).AssemblyQualifiedName);
            cfg.ClassMappings.ShouldContain(c => c.ClassName == typeof(Record).AssemblyQualifiedName);
        }

        [Test]
        public void MapsPropertyWithPropertyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleCustomColumn>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionFinder.Add<XXAppenderPropertyConvention>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/property[@name='LineOne']/column").HasAttribute("name", "LineOneXX");
        }

        [Test]
        public void TestAutoMapsIds()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleCustomColumn>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").Exists();
        }

        [Test]
        public void TestAutoMapsProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//property[@name='ExampleClassId']").Exists();
        }

        [Test]
        public void TestAutoMapIgnoresProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleCustomColumn>(c => c.IgnoreProperty(p => p.ExampleCustomColumnId));

            new AutoMappingTester<ExampleCustomColumn>(autoMapper)
                .Element("//property").DoesntExist();
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingProperty()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(c => c.Map(x => x.LineOne).ColumnName("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//property[@name='LineOne']")
                    .Exists()
                    .HasThisManyChildNodes(1)
                .Element("//property[@name='LineOne']/column").HasAttribute("name", "test");
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingId()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(c => c.Id(x => x.Id).ColumnName("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//id/column").HasAttribute("name", "test");
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingComponent()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(x => x.IsComponentType = type => type == typeof(ExampleParentClass))
                .ForTypesThatDeriveFrom<ExampleClass>(m => m.Component(x => x.Parent, c => c.Map(x => x.ExampleParentClassId).ColumnName("test")));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//component[@name='Parent']/property[@name='ExampleParentClassId']/column").HasAttribute("name", "test");
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingHasMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleParentClass>(m => m.HasMany(x => x.Examples).Inverse());

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag[@name='Examples']").HasAttribute("inverse", "true");
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingHasManyToMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleParentClass>(m => m.HasManyToMany(x => x.Examples).Inverse());

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag[@name='Examples']").HasAttribute("inverse", "true")
                .Element("//bag[@name='Examples']/many-to-many").Exists();
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingHasOne()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(m => m.HasOne(x => x.Parent));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//one-to-one[@name='Parent']").Exists()
                .Element("//bag[@name='Parent']").DoesntExist();
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingReferences()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(m => m.References(x => x.Parent).Access.AsField());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one[@name='Parent']").HasAttribute("access", "field");
        }

        [Test]
        public void ForTypesThatDeriveFromShouldOverrideExistingReferencesAny()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
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
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(m => m.Version(x => x.Timestamp).Access.AsField());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//version[@name='Timestamp']").HasAttribute("access", "field");
        }

        [Test]
        public void TestAutoMapManyToOne()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one").HasAttribute("name", "Parent");
        }

        [Test]
        public void TestAutoMapOneToMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag")
                .HasAttribute("name", "Examples");
        }

        [Test]
        public void TestAutoMapPropertyMergeOverridesId()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(map => map.Id(c => c.Id, "Column"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").HasAttribute("name", "Id")
                .Element("class/id/column").HasAttribute("name", "Column");
        }

        [Test]
        public void TestAutoMapPropertySetPrimaryKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionFinder.Add(PrimaryKey.Name.Is(id => id.Property.Name + "Id"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").HasAttribute("name", "Id")
                .Element("class/id/column").HasAttribute("name", "IdId");
        }

        [Test]
        public void TestAutoMapIdUsesConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<PrivateIdSetterClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionFinder.Add(new TestIdConvention());

            new AutoMappingTester<PrivateIdSetterClass>(autoMapper)
                .Element("class/id/column").HasAttribute("name", "test");
        }

        [Test]
        public void AppliesConventionsToManyToOne()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionFinder.Add(new TestM2OConvention());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one/column").HasAttribute("name", "test");
        }

        [Test]
        public void AppliesConventionsToOneToMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionFinder.Add(new TestO2MConvention());

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag").HasAttribute("name", "test");
        }

        [Test]
        public void TestAutoMapPropertySetFindPrimaryKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t == typeof(ExampleClass))
                .WithSetup(c => c.FindIdentity = p => p.Name == p.DeclaringType.Name + "Id");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").HasAttribute("name", "ExampleClassId")
                .Element("class/id/column").HasAttribute("name", "ExampleClassId");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Could not find mapping for class 'SuperType'")]
        public void TestInheritanceMappingSkipsSuperTypes()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures.SuperTypes")
                .WithSetup(c =>
                {
                    c.IsBaseType = b => b == typeof(SuperTypes.SuperType);
                });

            new AutoMappingTester<SuperTypes.SuperType>(autoMapper);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Could not find mapping for class 'SuperType'")]
        public void TestInheritanceSubclassMappingSkipsSuperTypes()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures.SuperTypes")
                .WithSetup(c =>
                {
                    c.IsBaseType = b => b == typeof(SuperTypes.SuperType);
                    c.SubclassStrategy = t => SubclassStrategy.Subclass;
                });

            new AutoMappingTester<SuperTypes.SuperType>(autoMapper);
        }

        [Test]
        public void TestInheritanceMapping()
        {
            AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");
        }

        [Test]
        public void TestInheritanceSubclassMapping()
        {
            AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .WithSetup(x => x.SubclassStrategy = t => SubclassStrategy.Subclass)
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");
        }

        [Test]
        public void TestInheritanceMappingProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/property[@name='ExampleProperty']").Exists();
        }

        [Test]
        public void TestInheritanceSubclassMappingProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .WithSetup(x => x.SubclassStrategy = t => SubclassStrategy.Subclass)
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            var tester = new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass/property[@name='ExampleProperty']").Exists();
        }

        [Test]
        public void TestDoNotAddJoinedSubclassesForConcreteBaseTypes()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(c =>
                           c.IsConcreteBaseType = b =>
                                                  b == typeof(ExampleClass));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass").DoesntExist();
        }

        [Test]
        public void TestDoNotAddSubclassesForConcreteBaseTypes()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(c =>
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
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(c =>
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
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(c =>
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
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceMappingDoesntIncludeBaseTypePropertiesWithSubclass()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .WithSetup(x => x.SubclassStrategy = t => SubclassStrategy.Subclass)
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceOverridingMappingProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .ForTypesThatDeriveFrom<ExampleClass>(t => t.JoinedSubClass<ExampleInheritedClass>("OverridenKey", p => p.Map(c => c.ExampleProperty, "columnName")))
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceSubclassOverridingMappingProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .WithSetup(x => x.SubclassStrategy = t => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleClass>(t => t.SubClass<ExampleInheritedClass>("discriminator", p => p.Map(c => c.ExampleProperty, "columnName")))
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestAutoMapClassAppliesConventions()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionFinder.Add(new TestClassConvention());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class").HasAttribute("table", "test");
        }

        [Test]
        public void CanSearchForOpenGenericTypes()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.CompileMappings();
            autoMapper.FindMapping(typeof(SomeOpenGenericType<>));
        }

        [Test]
        public void TypeConventionShouldForcePropertyToBeMapped()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ClassWithUserType>()
                .ConventionFinder.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ClassWithUserType>(autoMapper)
                .Element("class/property").HasAttribute("name", "Custom");
        }

        [Test]
        public void ComponentTypesAutoMapped()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Customer>()
                .WithSetup(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                })
                .ConventionFinder.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='HomeAddress']").Exists()
                .Element("class/component[@name='WorkAddress']").Exists();
        }

        [Test]
        public void ComponentPropertiesAutoMapped()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Customer>()
                .WithSetup(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                })
                .ConventionFinder.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/property[@name='Number']").Exists()
                .Element("class/component/property[@name='Street']").Exists();
        }

        [Test]
        public void ComponentPropertiesWithUserTypeAutoMapped()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Customer>()
                .WithSetup(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                })
                .ConventionFinder.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/property[@name='Custom']").HasAttribute("type", typeof(CustomUserType).AssemblyQualifiedName);
        }

        [Test]
        public void ComponentPropertiesAssumeComponentColumnPrefix()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Customer>()
                .WithSetup(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                    convention.GetComponentColumnPrefix =
                        property => property.Name + "_";
                })
                .ConventionFinder.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='WorkAddress']/property[@name='Number']/column").HasAttribute("name", "WorkAddress_Number");
        }

        [Test]
        public void ComponentColumnConventionReceivesProperty()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Customer>()
                .WithSetup(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                    convention.GetComponentColumnPrefix =
                        property => property.Name + "_";
                })
                .ConventionFinder.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='WorkAddress']/property[@name='Number']/column")
                .HasAttribute("name", value => value.StartsWith("WorkAddress_"));
        }

        [Test]
        public void IdIsMappedFromGenericBaseClass()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ClassUsingGenericBase>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(convention =>
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
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
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
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/property[@name='LineOne']").Exists()
                .Element("class/joined-subclass/property[@name='LineOne']").DoesntExist();
        }

        [Test]
        public void OverriddenSubclassIsAppliedToXml()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/bag")
                .HasAttribute("inverse", "true");
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingProperty()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(c => c.Map(x => x.ExampleProperty).ColumnName("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/property[@name='ExampleProperty']")
                    .Exists()
                    .HasThisManyChildNodes(1)
                .Element("//joined-subclass/property[@name='ExampleProperty']/column").HasAttribute("name", "test");
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingComponent()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(x => x.IsComponentType = type => type == typeof(ExampleParentClass))
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.Component(x => x.Component, c => c.Map(x => x.ExampleParentClassId).ColumnName("test")));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/component[@name='Component']/property[@name='ExampleParentClassId']/column").HasAttribute("name", "test");
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingHasMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/bag[@name='Children']").HasAttribute("inverse", "true");
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingHasManyToMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasManyToMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/bag[@name='Children']").HasAttribute("inverse", "true")
                .Element("//joined-subclass/bag[@name='Children']/many-to-many").Exists();
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingHasOne()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasOne(x => x.Parent));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/one-to-one[@name='Parent']").Exists()
                .Element("//joined-subclass/bag[@name='Parent']").DoesntExist();
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingReferences()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.References(x => x.Parent).Access.AsField());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/many-to-one[@name='Parent']").HasAttribute("access", "field");
        }

        [Test]
        public void JoinedSubclassForTypesThatDeriveFromShouldOverrideExistingReferencesAny()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
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
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(c => c.Map(x => x.ExampleProperty).ColumnName("test"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/property[@name='ExampleProperty']")
                    .Exists()
                    .HasThisManyChildNodes(1)
                .Element("//subclass/property[@name='ExampleProperty']/column").HasAttribute("name", "test");
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingComponent()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(x =>
                {
                    x.SubclassStrategy = type => SubclassStrategy.Subclass;
                    x.IsComponentType = type => type == typeof(ExampleParentClass);
                })
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.Component(x => x.Component, c => c.Map(x => x.ExampleParentClassId).ColumnName("test")));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/component[@name='Component']/property[@name='ExampleParentClassId']/column").HasAttribute("name", "test");
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingHasMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/bag[@name='Children']").HasAttribute("inverse", "true");
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingHasManyToMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasManyToMany(x => x.Children).Inverse());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/bag[@name='Children']").HasAttribute("inverse", "true")
                .Element("//subclass/bag[@name='Children']/many-to-many").Exists();
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingHasOne()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.HasOne(x => x.Parent));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/one-to-one[@name='Parent']").Exists()
                .Element("//subclass/bag[@name='Parent']").DoesntExist();
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingReferences()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m => m.References(x => x.Parent).Access.AsField());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/many-to-one[@name='Parent']").HasAttribute("access", "field");
        }

        [Test]
        public void SubclassForTypesThatDeriveFromShouldOverrideExistingReferencesAny()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithSetup(x => x.SubclassStrategy = type => SubclassStrategy.Subclass)
                .ForTypesThatDeriveFrom<ExampleInheritedClass>(m =>
                    m.ReferencesAny(x => x.DictionaryChild)
                        .EntityIdentifierColumn("one")
                        .EntityTypeColumn("two")
                        .IdentityType<int>());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//subclass/any[@name='DictionaryChild']").Exists()
                .Element("//subclass/map[@name='DictionaryChild']").DoesntExist();
        }

        private class TestIdConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.ColumnName("test");
            }
        }

        private class TestClassConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.WithTable("test");
            }
        }

        private class TestM2OConvention : IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                instance.ColumnName("test");
            }
        }

        private class TestO2MConvention : IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.Name("test");
            }
        }
    }

    public class SomeOpenGenericType<T>
    {}
}
