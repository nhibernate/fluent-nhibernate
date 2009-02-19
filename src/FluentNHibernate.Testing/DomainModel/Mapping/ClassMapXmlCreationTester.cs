using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using NUnit.Framework;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassMapXmlCreationTester
    {
        private XmlDocument document;

        private XmlElement elementForProperty(string propertyName)
        {
            string xpath = string.Format("class/property[@name='{0}']", propertyName);
            return (XmlElement) document.DocumentElement.SelectSingleNode(xpath);
        }

        [Test]
        public void BasicManyToManyMapping()
        {
        	new MappingTester<MappedObject>()
        		.ForMapping(map => map.HasManyToMany(x => x.Children))
        		.Element("class/bag")
        			.HasAttribute("name", "Children")
					.DoesntHaveAttribute("cascade")
        		.Element("class/bag/key")
        			.HasAttribute("column", "MappedObject_id")
        		.Element("class/bag/many-to-many")
        			.HasAttribute("class", typeof (ChildObject).AssemblyQualifiedName)
        			.HasAttribute("column", "ChildObject_id")
					.DoesntHaveAttribute("fetch");
        }
        
        [Test]
        public void ManyToManyAsSet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).AsSet())
                .Element("class/set")
                    .HasAttribute("name", "Children")
                    .HasAttribute("table", typeof(ChildObject).Name + "To" + typeof(MappedObject).Name)
                .Element("class/set/key")
                    .HasAttribute("column", "MappedObject_id")
                .Element("class/set/many-to-many")
                    .HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
                    .HasAttribute("column", "ChildObject_id");
		}

		[Test]
		public void ManyToManyAsBag()
		{
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).AsBag())
                .Element("class/bag")
                    .HasAttribute("name", "Children")
                    .HasAttribute("table", typeof(ChildObject).Name + "To" + typeof(MappedObject).Name)
                .Element("class/bag/key")
                    .HasAttribute("column", "MappedObject_id")
                .Element("class/bag/many-to-many")
                    .HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
                    .HasAttribute("column", "ChildObject_id");
		}
		
		[Test]
		public void ManyToManyAsSetWithChildForeignKey()
		{
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).AsSet().WithChildKeyColumn("TheKids_ID"))
                .Element("class/set")
                    .HasAttribute("name", "Children")
                    .HasAttribute("table", typeof(ChildObject).Name + "To" + typeof(MappedObject).Name)
                .Element("class/set/key")
                    .HasAttribute("column", "MappedObject_id")
                .Element("class/set/many-to-many")
                    .HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
                    .HasAttribute("column", "TheKids_ID");
		}

		[Test]
		public void ManyToManyAsSetWithParentForeignKey()
		{
			new MappingTester<MappedObject>()
				.ForMapping(m => m.HasManyToMany(x => x.Children).AsSet().WithParentKeyColumn("TheParentID"))
				.Element("class/set")
					.HasAttribute("name", "Children")
					.HasAttribute("table", typeof(ChildObject).Name + "To" + typeof(MappedObject).Name)
				.Element("class/set/key")
					.HasAttribute("column", "TheParentID")
				.Element("class/set/many-to-many")
					.HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
					.HasAttribute("column", "ChildObject_id");
		}

		[Test]
		public void ManyToManyAsSetWithJoinFetchMode()
		{
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).AsSet().FetchType.Join())
                .Element("class/set")
                    .HasAttribute("name", "Children")
                    .HasAttribute("table", typeof(ChildObject).Name + "To" + typeof(MappedObject).Name)
                .Element("class/set/key")
                    .HasAttribute("column", "MappedObject_id")
                .Element("class/set/many-to-many")
                    .HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
                    .HasAttribute("column", "ChildObject_id")
                    .HasAttribute("fetch", "join");
		}

		[Test]
        public void BasicOneToManyMapping()
        {
			new MappingTester<MappedObject>()
				.ForMapping(map => map.HasMany(x => x.Children))
				.Element("class/bag")
					.HasAttribute("name", "Children")
				.Element("class/bag/key")
					.HasAttribute("column", "MappedObject_id")
				.Element("class/bag/one-to-many")
					.HasAttribute("class", typeof (ChildObject).AssemblyQualifiedName);
        }

        [Test]
        public void AdvancedOneToManyMapping()
        {
            var map = new ClassMap<MappedObject>();
            map.HasMany(x => x.Children).LazyLoad().Inverse();

            document = map.CreateMapping(new MappingVisitor());

            var element =
                (XmlElement)document.DocumentElement.SelectSingleNode("class/bag[@name='Children']");

            element.AttributeShouldEqual("lazy", "true");
            element.AttributeShouldEqual("inverse", "true");
        }

        [Test]
        public void AdvancedOneToManyMapping_NotLazy_NotInverse()
        {
            var map = new ClassMap<MappedObject>();
            map.HasMany(x => x.Children)
                .Not.LazyLoad()
                .Not.Inverse();

            document = map.CreateMapping(new MappingVisitor());

            var element =
                (XmlElement)document.DocumentElement.SelectSingleNode("class/bag[@name='Children']");

            element.AttributeShouldEqual("lazy", "false");
            element.AttributeShouldEqual("inverse", "false");
        }

        [Test]
        public void AdvancedManyToManyMapping()
        {
            var map = new ClassMap<MappedObject>();
            map.HasManyToMany(x => x.Children).LazyLoad().Inverse();

            document = map.CreateMapping(new MappingVisitor());

            var element =
                (XmlElement)document.DocumentElement.SelectSingleNode("class/bag[@name='Children']");

            element.AttributeShouldEqual("lazy", "true");
            element.AttributeShouldEqual("inverse", "true");
        }

        [Test]
        public void AdvancedManyToManyMapping_NotLazy_NotInverse()
        {
            var map = new ClassMap<MappedObject>();
            map.HasManyToMany(x => x.Children)
                .Not.LazyLoad()
                .Not.Inverse();

            document = map.CreateMapping(new MappingVisitor());

            var element =
                (XmlElement)document.DocumentElement.SelectSingleNode("class/bag[@name='Children']");

            element.AttributeShouldEqual("lazy", "false");
            element.AttributeShouldEqual("inverse", "false");
        }

        [Test]
        public void BuildTheHeaderXmlWithAssemblyAndNamespace()
        {
            var map = new ClassMap<MappedObject>();
            document = map.CreateMapping(new MappingVisitor());

            document.DocumentElement.GetAttribute("assembly").ShouldEqual(typeof (MappedObject).Assembly.GetName().Name);
            document.DocumentElement.GetAttribute("namespace").ShouldEqual(typeof (MappedObject).Namespace);
        }

        [Test]
        public void CascadeAll_with_many_to_many()
        {
            var map = new ClassMap<MappedObject>();
            map.HasManyToMany(x => x.Children).Cascade.All();

            document = map.CreateMapping(new MappingVisitor());
            var element = (XmlElement) document.DocumentElement.SelectSingleNode("class/bag[@name='Children']");

            element.AttributeShouldEqual("cascade", "all");
        }

        [Test]
        public void CascadeAll_with_one_to_many()
        {
        	new MappingTester<MappedObject>()
        		.ForMapping(map => map.HasMany(x => x.Children).Cascade.All())
        		.Element("class/bag[@name='Children']").HasAttribute("cascade", "all");
        }

        [Test]
        public void Create_a_component_mapping()
        {
            var map = new ClassMap<MappedObject>();
            map.Component(x => x.Component, c =>
                                                                         {
                                                                             c.Map(x => x.Name);
                                                                             c.Map(x => x.Age);
                                                                         });

            document = map.CreateMapping(new MappingVisitor());

            var componentElement =
                (XmlElement) document.DocumentElement.SelectSingleNode("class/component");

            componentElement.AttributeShouldEqual("name", "Component");
            componentElement.AttributeShouldEqual("insert", "true");
            componentElement.AttributeShouldEqual("update", "true");

            componentElement.ShouldHaveChild("property[@name='Name']");
            componentElement.ShouldHaveChild("property[@name='Age']");
        }

        [Test]
        public void DetermineTheTableName()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => { })
                .Element("class").HasAttribute("table", "`MappedObject`");
        }

        [Test]
        public void CanSetTableName()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.WithTable("myTableName"))
                .Element("class").HasAttribute("table", "myTableName");
        }

        [Test]
        public void CanOverrideLazyLoad()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.LazyLoad())
                .Element("class").HasAttribute("lazy", "true");
        }

        [Test]
        public void CanOverrideNotLazyLoad()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Not.LazyLoad())
                .Element("class").HasAttribute("lazy", "false");
        }

        [Test] // stupid, I know but somebody'll try it
        public void DoubleNotWorksCorrectly()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Not.Not.LazyLoad())
                .Element("class").HasAttribute("lazy", "true");
        }

        [Test]
        public void DomainClassMapAutomaticallyCreatesTheId()
        {
            var map = new ClassMap<MappedObject>();
            map.UseIdentityForKey(x => x.Id, "id");
            document = map.CreateMapping(new MappingVisitor());

            XmlElement idElement = document.DocumentElement["class"]["id"];
            idElement.ShouldNotBeNull();

            idElement.GetAttribute("name").ShouldEqual("Id");
            idElement.GetAttribute("column").ShouldEqual("id");
            idElement.GetAttribute("type").ShouldEqual("Int64");

            XmlElement generatorElement = idElement["generator"];
            generatorElement.ShouldNotBeNull();
            generatorElement.GetAttribute("class").ShouldEqual("identity");
        }

        [Test]
        public void Map_an_enumeration()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Map(x => x.Color))
                .Element("class/property[@name='Color']")
                    .Exists()
                    .HasAttribute("type", typeof(GenericEnumMapper<ColorEnum>).AssemblyQualifiedName)
                .Element("class/property[@name='Color']/column")
                    .Exists()
                    .HasAttribute("sql-type", "string");
        }

        [Test]
		public void Map_a_nullable_enumeration()
		{
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Map(x => x.NullableColor))
                .Element("class/property[@name='NullableColor']")
                    .Exists()
                    .HasAttribute("type", typeof(GenericEnumMapper<ColorEnum>).AssemblyQualifiedName)
                .Element("class/property[@name='NullableColor']/column")
                    .Exists()
                    .HasAttribute("sql-type", "string")
                    .HasAttribute("not-null", "false");
		}

        [Test]
        public void MapASimplePropertyWithNoOverrides()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.Map(x => x.Name))
                .Element("//property[@name='Name']")
                    .Exists()
                    .HasAttribute("type", "String")
                .Element("//property[@name='Name']/column")
                    .Exists()
                    .HasAttribute("name", "Name");
        }

        [Test]
        public void WriteTheClassNode()
        {
            var map = new ClassMap<MappedObject>();
            document = map.CreateMapping(new MappingVisitor());

            XmlElement classElement = document.DocumentElement["class"];
            classElement.ShouldNotBeNull();

            classElement.AttributeShouldEqual("name", typeof (MappedObject).Name);
            classElement.AttributeShouldEqual("table", map.TableName);
        }

		[Test]
		public void DomainClassMapWithId()
		{
			var map = new ClassMap<MappedObject>();
			map.Id(x => x.Id, "id");
			document = map.CreateMapping(new MappingVisitor());

			XmlElement idElement = document.DocumentElement["class"]["id"];
			idElement.ShouldNotBeNull();

			idElement.GetAttribute("name").ShouldEqual("Id");
			idElement.GetAttribute("column").ShouldEqual("id");
			idElement.GetAttribute("type").ShouldEqual("Int64");

			XmlElement generatorElement = idElement["generator"];
			generatorElement.ShouldNotBeNull();
			generatorElement.GetAttribute("class").ShouldEqual("identity");
		}

		[Test]
		public void DomainClassMapWithIdNoColumn()
		{
			var map = new ClassMap<MappedObject>();
			map.Id(x => x.Id);
			document = map.CreateMapping(new MappingVisitor());

			XmlElement idElement = document.DocumentElement["class"]["id"];
			idElement.ShouldNotBeNull();

			idElement.GetAttribute("name").ShouldEqual("Id");
			idElement.GetAttribute("column").ShouldEqual("Id");
			idElement.GetAttribute("type").ShouldEqual("Int64");

			XmlElement generatorElement = idElement["generator"];
			generatorElement.ShouldNotBeNull();
			generatorElement.GetAttribute("class").ShouldEqual("identity");
		}

        [Test]
        public void ClassMapHasCorrectHBMFileName()
        {
            var expectedFileName = "MappedObject.hbm.xml";
            var map = new ClassMap<MappedObject>();
            Assert.AreEqual(expectedFileName , map.FileName);
        }

        [Test]
		public void DomainClassMapWithIdNoColumnAndGenerator()
		{
			var map = new ClassMap<MappedObject>();
			map.Id(x => x.Id).GeneratedBy.Native();
			document = map.CreateMapping(new MappingVisitor());

			XmlElement generatorElement = document.DocumentElement["class"]["id"]["generator"];

			generatorElement.ShouldNotBeNull();
			generatorElement.GetAttribute("class").ShouldEqual("native");
		}

   		[Test]
		public void Creating_a_many_to_one_reference_with_column_specified()
		{
   		    new MappingTester<MappedObject>()
   		        .ForMapping(m => m.References(x => x.Parent, "MyParentId"))
   		        .Element("class/many-to-one").HasAttribute("column", "MyParentId");
		}

        [Test]
        public void Creating_a_many_to_one_reference_with_column_specified_through_TheColumnNameIs_method()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.References(x => x.Parent).TheColumnNameIs("MyParentId"))
                .Element("class/many-to-one").HasAttribute("column", "MyParentId");
        }

		[Test]
		public void Creating_a_many_to_one_reference_using_specified_foreign_key()
		{
		    new MappingTester<MappedObject>()
		        .ForMapping(m => m.References(x => x.Parent).WithForeignKey("FK_MyForeignKey"))
		        .Element("class/many-to-one").HasAttribute("foreign-key", "FK_MyForeignKey");
		}

		[Test]
		public void Creating_a_many_to_one_reference_with_cascade_specified_as_None()
		{
		    new MappingTester<MappedObject>()
		        .ForMapping(m => m.References(x => x.Parent).Cascade.None())
		        .Element("class/many-to-one").HasAttribute("cascade", "none");
		}

		[Test]
		public void Creating_a_many_to_one_reference_with_fetchtype_set()
		{
			new MappingTester<MappedObject>()
				.ForMapping(m => m.References(x => x.Parent).FetchType.Select())
				.Element("class/many-to-one").HasAttribute("fetch", "select");
		}

        [Test]
        public void CanSetSchema()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.SchemaIs("test"))
                .HasAttribute("schema", "test");
        }

        [Test]
        public void CanSetAsUnique()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Map(x => x.Name).Unique())
                .Element("class/property").HasAttribute("unique", "true");
        }

        [Test]
        public void CanSetAsNotUnique()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Map(x => x.Name).Not.Unique())
                .Element("class/property").HasAttribute("unique", "false");
        }

        [Test]
        public void SpanningClassAcrossTwoTables()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.WithTable("tableTwo", t => t.Map(x => x.Name)))
                .Element("class/join").Exists();
        }

        [Test]
        public void ShouldAddMultipleAttributesToClassUsingSetAttributes()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => x.SetAttributes(new Attributes
                {
                    {"first", "value"},
                    {"second", "secondValue"},
                }))
                .Element("class")
                    .HasAttribute("first", "value")
                    .HasAttribute("second", "secondValue");
        }

        [Test]
        public void ShouldAddImportElementsBeforeClass()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => x.ImportType<SecondMappedObject>())
                .Element("import")
                    .Exists()
                    .HasAttribute("class", typeof(SecondMappedObject).AssemblyQualifiedName);
        }

        [Test]
        public void ShouldntAddImportElementsInsideClass()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => x.ImportType<SecondMappedObject>())
                .Element("class/import").DoesntExist();
        }

        [Test]
        public void ShouldAddRenameAttributeWhenDifferentNameSpecified()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => x.ImportType<SecondMappedObject>().As("MappedObject"))
                .Element("import").HasAttribute("rename", "MappedObject");
        }

        [Test]
        public void DefaultLazyLoad_should_be_true_by_default_for_compatibility_with_NHibernate()
        {
            new MappingTester<MappedObject>()
                .ForMapping(c => { })
                .HasAttribute("default-lazy", "true");
        }

        [Test]
        public void Can_set_readonly()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.ReadOnly())
                .Element("class").HasAttribute("mutable", "false");
        }

        [Test]
        public void Can_set_non_readonly()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Not.ReadOnly())
                .Element("class").HasAttribute("mutable", "true");
        }
    }

    public class SecondMappedObject
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public int IdPart1 { get; set; }
        public int IdPart2 { get; set; }
        public int IdPart3 { get; set; }
    }

    public class ComponentOfMappedObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public enum ColorEnum
    {
        Blue,
        Green,
        Red
    }

    public class MappedObject
    {
        public ColorEnum Color { get; set; }

        public ColorEnum? NullableColor { get; set; }

        public ComponentOfMappedObject Component { get; set; }

        public SecondMappedObject Parent { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public IList<ChildObject> Children { get; set; }

        public long Id { get; set; }
    }

    public class ChildObject
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Position { get; set; }
    }
}