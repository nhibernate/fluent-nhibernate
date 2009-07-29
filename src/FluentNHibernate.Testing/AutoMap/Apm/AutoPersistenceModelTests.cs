using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures.ComponentTypes;
using FluentNHibernate.AutoMap.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;
using SuperTypes = FluentNHibernate.AutoMap.TestFixtures.SuperTypes;
using FluentNHibernate.AutoMap.TestFixtures;

namespace FluentNHibernate.Testing.AutoMap.Apm
{
    [TestFixture]
    public class AutoPersistenceModelTests : BaseAutoPersistenceTests
    {
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
                .ConventionDiscovery.Add<XXAppenderPropertyConvention>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/property[@name='LineOne']/column").HasAttribute("name", "LineOneXX");

            CallContext.SetData("XXAppender", null);
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
                .Element("class/id")
                .HasAttribute("name", "Id")
                .HasAttribute("column", "Column");
        }

        [Test]
        public void TestAutoMapPropertySetPrimaryKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionDiscovery.Add(PrimaryKey.Name.Is(id => id.Property.Name + "Id"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "Id")
                .HasAttribute("column", "IdId");
        }

        [Test]
        public void TestAutoMapIdUsesConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<PrivateIdSetterClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionDiscovery.Add(new TestIdConvention());

            new AutoMappingTester<PrivateIdSetterClass>(autoMapper)
                .Element("class/id")
                .HasAttribute("test", "true");
        }

        [Test]
        public void AppliesConventionsToManyToOne()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionDiscovery.Add(new TestM2OConvention());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one")
                .HasAttribute("test", "true");
        }

        [Test]
        public void AppliesConventionsToOneToMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionDiscovery.Add(new TestO2MConvention());

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag")
                .HasAttribute("test", "true");
        }

        [Test]
        public void TestAutoMapPropertySetFindPrimaryKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t == typeof(ExampleClass))
                .WithSetup(c => c.FindIdentity = p => p.Name == p.DeclaringType.Name + "Id" );

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "ExampleClassId")
                .HasAttribute("column", "ExampleClassId");
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
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
        [ExpectedException(typeof(NullReferenceException))]
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

            var tester = new AutoMappingTester<ExampleClass>(autoMapper)
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
                                                  b == typeof (ExampleClass));

            autoMapper.Configure(cfg);

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

            autoMapper.Configure(cfg);

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

            autoMapper.Configure(cfg);

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

            autoMapper.Configure(cfg);

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

            autoMapper.Configure(cfg);

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

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceOverridingMappingProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .ForTypesThatDeriveFrom<ExampleClass>(t => t.JoinedSubClass<ExampleInheritedClass>("OverridenKey", p =>p.Map(c => c.ExampleProperty, "columnName")))
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

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
                .ConventionDiscovery.Add(new TestClassConvention());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class").HasAttribute("test", "true");
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
                .ConventionDiscovery.Add<CustomTypeConvention>()
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
                .ConventionDiscovery.Add<CustomTypeConvention>()
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
                .ConventionDiscovery.Add<CustomTypeConvention>()
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
                .ConventionDiscovery.Add<CustomTypeConvention>()
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
                .ConventionDiscovery.Add<CustomTypeConvention>()
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
                .ConventionDiscovery.Add<CustomTypeConvention>()
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
        public void ShouldBeAbleToOverrideKeyColumnNameOfJoinedSubclassInConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionDiscovery.Add<JoinedSubclassConvention>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/key")
                .HasAttribute("column", "test");
        }

        [Test]
        public void ShouldBeAbleToOverrideTableNameOfJoinedSubclassInConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ConventionDiscovery.Add<JoinedSubclassConvention>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .HasAttribute("table", "test-table");
        }

        [Test]
        public void ShouldBeAbleToIgnorePropertiesRegardlessOfType()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForAllTypes(t => t.IgnoreProperty("Dummy"));

            new AutoMappingTester<ClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist();

            new AutoMappingTester<AnotherClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist();
        }

        [Test]
        public void ShouldBeAbleToIgnoreMultiplePropertiesRegardlessOftype()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
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

        private class JoinedSubclassConvention : IJoinedSubclassConvention
        {
            public bool Accept(IJoinedSubclass target)
            {
                return true;
            }

            public void Apply(IJoinedSubclass target)
            {
                target.WithTableName("test-table");
                target.KeyColumnName("test");
            }
        }

        private class TestIdConvention : IIdConvention
        {
            public bool Accept(IIdentityPart target)
            {
                return true;
            }

            public void Apply(IIdentityPart target)
            {
                target.SetAttribute("test", "true");
            }
        }

        private class TestClassConvention : IClassConvention
        {
            public bool Accept(IClassMap target)
            {
                return true;
            }

            public void Apply(IClassMap target)
            {
                target.SetAttribute("test", "true");
            }
        }

        private class TestM2OConvention : IReferenceConvention
        {
            public bool Accept(IManyToOnePart target)
            {
                return true;
            }

            public void Apply(IManyToOnePart target)
            {
                target.SetAttribute("test", "true");
            }
        }

        private class TestO2MConvention : IHasManyConvention
        {
            public bool Accept(IOneToManyPart target)
            {
                return true;
            }

            public void Apply(IOneToManyPart target)
            {
                target.SetAttribute("test", "true");
            }
        }
    }

    public class SomeOpenGenericType<T>
    {}
}
