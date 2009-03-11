using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures.ComponentTypes;
using FluentNHibernate.AutoMap.TestFixtures.CustomCompositeTypes;
using FluentNHibernate.AutoMap.TestFixtures.CustomTypes;
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
        public void CanMixMappingTypes()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");
            autoMapper.addMappingsFromAssembly(typeof(ExampleClass).Assembly);
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
                .WithConvention(convention =>
                    convention.Finder.Add<XXAppenderPropertyConvention>());

            autoMapper.Configure(cfg);

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

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").Exists();
        }

        [Test]
        public void TestAutoMapsProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

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

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleCustomColumn>(autoMapper)
                .Element("//property").DoesntExist();
        }

        [Test]
        public void TestAutoMapManyToOne()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one").HasAttribute("name", "Parent");
        }

        [Test]
        public void TestAutoMapOneToMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

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

            autoMapper.Configure(cfg);

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
                .WithConvention(c => c.GetPrimaryKeyName =
                    id => id.Property.Name + "Id");

            autoMapper.Configure(cfg);

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
                .WithConvention(convention =>
                    convention.IdConvention = id => id.Access.AsLowerCaseField());

            autoMapper.Configure(cfg);

            new AutoMappingTester<PrivateIdSetterClass>(autoMapper)
                .Element("class/id")
                .HasAttribute("access", "field.lowercase");
        }

        [Test]
        public void TestAutoMapPropertySetManyToOneKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c =>
                {
                    c.GetForeignKeyName = p => p.Name + "Id";
                });

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one")
                .HasAttribute("name", "Parent")
                .HasAttribute("column", "ParentId");
        }

        [Test]
        public void TestAutoMapPropertySetOneToManyKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c => c.GetForeignKeyNameForType =  t => t.Name + "Id");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag")
                .HasAttribute("name", "Examples")
                .Element("//key")
                .HasAttribute("column", "ExampleParentClassId");
        }

        [Test]
        public void TestAutoMapPropertySetFindPrimaryKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t == typeof(ExampleClass))
                .WithConvention(c => c.FindIdentity = p => p.Name == p.DeclaringType.Name + "Id" );

            autoMapper.Configure(cfg);

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
                .WithConvention(c =>
                {
                    c.IsBaseType = b => b == typeof(SuperTypes.SuperType);
                });

            autoMapper.Configure(cfg);

            new AutoMappingTester<SuperTypes.SuperType>(autoMapper);
        }

        [Test]
        public void TestInheritanceMapping()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            var tester = new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .HasAttribute("name", typeof(ExampleInheritedClass).AssemblyQualifiedName);

            tester.Element("class/joined-subclass/key")
                .HasAttribute("column", "ExampleClassId");
        }

        [Test]
        public void TestInheritanceMappingProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            var tester = new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/property")
                .HasAttribute("name", "ExampleProperty");
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
        public void TestInheritanceOverridingMappingProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .ForTypesThatDeriveFrom<ExampleClass>(t => t.JoinedSubClass<ExampleInheritedClass>("OverridenKey", p =>p.Map(c => c.ExampleProperty, "columnName")))
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper).ToString();
            
            var tester = new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestAutoMapClassSetCacheConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c => c.DefaultCache = cache => cache.AsReadWrite());

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//cache").HasAttribute("usage", "read-write");
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
                .WithConvention(convention =>
                {
                    convention.Finder.Add<CustomTypeConvention>();
                })
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ClassWithUserType>(autoMapper)
                .Element("class/property").HasAttribute("name", "Custom");
        }

        [Test]
        public void TypeConventionShouldForceCompositePropertyToBeMappedWithCorrectNumberOfColumns()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ClassWithCompositeUserType>()
                .WithConvention(convention =>
                {
                    convention.AddTypeConvention(new CustomCompositeTypeConvention());
                })
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            var mappedColumns = new AutoMappingTester<ClassWithCompositeUserType>(autoMapper)
                .Element("class/property");

            mappedColumns.HasThisManyChildNodes(2);
        }

        [Test]
        public void ComponentTypesAutoMapped()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Customer>()
                .WithConvention(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                    convention.Finder.Add<CustomTypeConvention>();
                })
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='HomeAddress']").Exists()
                .Element("class/component[@name='WorkAddress']").Exists();
        }

        [Test]
        public void ComponentPropertiesAutoMapped()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Customer>()
                .WithConvention(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                    convention.Finder.Add<CustomTypeConvention>();
                })
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/property[@name='Number']").Exists()
                .Element("class/component/property[@name='Street']").Exists();
        }

        [Test]
        public void ComponentPropertiesWithUserTypeAutoMapped()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Customer>()
                .WithConvention(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                    convention.Finder.Add<CustomTypeConvention>();
                })
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/property[@name='Custom']").HasAttribute("type", typeof(CustomUserType).AssemblyQualifiedName);
        }

        [Test]
        public void ComponentPropertiesAssumeComponentColumnPrefix()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Customer>()
                .WithConvention(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                    convention.GetComponentColumnPrefix =
                        property => property.Name + "_";
                    convention.Finder.Add<CustomTypeConvention>();
                })
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='WorkAddress']/property[@name='Number']/column").HasAttribute("name", "WorkAddress_Number");
        }

        [Test]
        public void ComponentColumnConventionReceivesProperty()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Customer>()
                .WithConvention(convention =>
                {
                    convention.IsComponentType =
                        type => type == typeof(Address);
                    convention.GetComponentColumnPrefix =
                        property => property.Name + "_";
                    convention.Finder.Add<CustomTypeConvention>();
                })
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='WorkAddress']/property[@name='Number']/column")
                .HasAttribute("name", value => value.StartsWith("WorkAddress_"));
        }

        [Test]
        public void ForTypesThatDeriveFromTThrowsExceptionIfCalledMoreThanOnceForSameType()
        {
            var ex = Assert.Throws<AutoMappingException>(() => AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(map => { })
                .ForTypesThatDeriveFrom<ExampleClass>(map => { }));

            Assert.That(ex.Message, Is.EqualTo("ForTypesThatDeriveFrom<T> called more than once for 'ExampleClass'. Merge your calls into one."));
        }

        [Test]
        public void IdIsMappedFromGenericBaseClass()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ClassUsingGenericBase>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(convention =>
                {
                    convention.IsBaseType =
                        type => type == typeof(object) || type == typeof(EntityBase<>);
                });

            autoMapper.Configure(cfg);

            new AutoMappingTester<ClassUsingGenericBase>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "Id");
        }
    }

    public class SomeOpenGenericType<T>
    {}
}
