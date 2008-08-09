using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using NUnit.Framework;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using ShadeTree.Validation;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassMapXmlCreationTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }

        #endregion

        private XmlDocument document;

        private XmlElement elementForProperty(string propertyName)
        {
            string xpath = string.Format("class/property[@name='{0}']", propertyName);
            return (XmlElement) document.DocumentElement.SelectSingleNode(xpath);
        }

        [Test, Ignore("Needs to be rewritten at some point")]
        public void BasicManyToManyMapping()
        {
            var map = new ClassMap<MappedObject>();
            map.HasManyToMany<ChildObject>(x => x.Children);

            document = map.CreateMapping(new MappingVisitor());
            var element = (XmlElement) document.DocumentElement.SelectSingleNode("class/bag[@name='Children']");

            element.AttributeShouldEqual("name", "Children");
            element.AttributeShouldEqual("cascade", "none");

            element["key"].AttributeShouldEqual("column", "MappedObject_Fk");
            element["many-to-many"].AttributeShouldEqual("class", typeof (ChildObject).AssemblyQualifiedName);
            element["many-to-many"].AttributeShouldEqual("table", typeof (ChildObject).Name);
            element["many-to-many"].AttributeShouldEqual("column", "ChildObject_Fk");
        }

        [Test]
        public void BasicOneToManyMapping()
        {
            var map = new ClassMap<MappedObject>();
            map.HasMany<ChildObject>(x => x.Children);

            document = map.CreateMapping(new MappingVisitor());
            
            var element =
                (XmlElement) document.DocumentElement.SelectSingleNode("class/bag[@name='Children']");

            element.AttributeShouldEqual("name", "Children");
            element.AttributeShouldEqual("cascade", "none");

            element["key"].AttributeShouldEqual("column", "MappedObject_id");
            element["one-to-many"].AttributeShouldEqual("class", typeof (ChildObject).AssemblyQualifiedName);
        }

        [Test]
        public void AdvancedOneToManyMapping()
        {
            var map = new ClassMap<MappedObject>();
            map.HasMany<ChildObject>(x => x.Children).LazyLoad().IsInverse();

            document = map.CreateMapping(new MappingVisitor());

            var element =
                (XmlElement)document.DocumentElement.SelectSingleNode("class/bag[@name='Children']");

            element.AttributeShouldEqual("lazy", "true");
            element.AttributeShouldEqual("inverse", "true");
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
            map.HasManyToMany<ChildObject>(x => x.Children).CascadeAll();

            document = map.CreateMapping(new MappingVisitor());
            var element = (XmlElement) document.DocumentElement.SelectSingleNode("class/bag[@name='Children']");

            element.AttributeShouldEqual("cascade", "all");
        }

        [Test]
        public void CascadeAll_with_one_to_many()
        {
            var map = new ClassMap<MappedObject>();
            map.HasMany<ChildObject>(x => x.Children).CascadeAll();

            document = map.CreateMapping(new MappingVisitor());
            var element =
                (XmlElement) document.DocumentElement.SelectSingleNode("class/bag[@name='Children']");

            element.AttributeShouldEqual("cascade", "all");
        }

        [Test]
        public void Create_a_component_mapping()
        {
            var map = new ClassMap<MappedObject>();
            map.Component<ComponentOfMappedObject>(x => x.Component, c =>
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
        public void
            Create_a_component_mapping_for_a_component_that_is_not_required_and_the_fields_for_the_component_should_not_be_non_null
            ()
        {
            var map = new ClassMap<MappedObject>();
            map.Component<SecondMappedObject>(x => x.Parent, c => { c.Map(x => x.Name); });

            document = map.CreateMapping(new MappingVisitor());

            var element =
                (XmlElement) document.DocumentElement.SelectSingleNode("class/component/property[@name='Name']");

            Debug.WriteLine(element.OuterXml);

            element.HasAttribute("not-null").ShouldBeFalse();
        }

        [Test]
        public void Create_a_component_mapping_for_a_required_child_and_set_the_required_fields_of_component_to_non_null
            ()
        {
            var map = new ClassMap<MappedObject>();
            map.Component<ComponentOfMappedObject>(x => x.Component, c =>
                                                                         {
                                                                             c.Map(x => x.Name);
                                                                             c.Map(x => x.Age);
                                                                         });

            document = map.CreateMapping(new MappingVisitor());

            var componentElement =
                (XmlElement) document.DocumentElement.SelectSingleNode("class/component");

            var propertyElement = (XmlElement) componentElement.SelectSingleNode("property[@name = 'Age']");
            propertyElement.AttributeShouldEqual("not-null", "true");
        }

        [Test]
        public void CreateDiscriminator()
        {
            var map = new ClassMap<SecondMappedObject>();
            map.DiscriminateSubClassesOnColumn<string>("Type");

            document = map.CreateMapping(new MappingVisitor());
            var element = (XmlElement) document.DocumentElement.SelectSingleNode("class/discriminator");
            element.AttributeShouldEqual("column", "Type");
            element.AttributeShouldEqual("type", "String");
        }

        [Test]
        public void CreateTheSubClassMappings()
        {
            var map = new ClassMap<MappedObject>();

            map.DiscriminateSubClassesOnColumn<string>("Type")
                .SubClass<SecondMappedObject>().IsIdentifiedBy("red")
                .MapSubClassColumns(m => { m.Map(x => x.Name); });

            document = map.CreateMapping(new MappingVisitor());

            Debug.WriteLine(document.OuterXml);

            var element = (XmlElement) document.DocumentElement.SelectSingleNode("//subclass");
            element.AttributeShouldEqual("name", "SecondMappedObject");
            element.AttributeShouldEqual("discriminator-value", "red");

            XmlElement propertyElement = element["property"];
            propertyElement.AttributeShouldEqual("column", "Name");
        }

    	[Test]
    	public void CreateDiscriminatorValueAtClassLevel()
		{
			var map = new ClassMap<MappedObject>();

			map.DiscriminateSubClassesOnColumn<string>("Type", "Foo")
				.SubClass<SecondMappedObject>().IsIdentifiedBy("Bar")
				.MapSubClassColumns(m => m.Map(x => x.Name));

			document = map.CreateMapping(new MappingVisitor());

			var element = (XmlElement)document.DocumentElement.SelectSingleNode("class");
			element.AttributeShouldEqual("discriminator-value", "Foo");
    	}

        [Test]
        public void Creating_a_many_to_one_reference()
        {
            var map = new ClassMap<MappedObject>();
            map.References(x => x.Parent);

            document = map.CreateMapping(new MappingVisitor());
            var element = (XmlElement) document.DocumentElement.SelectSingleNode("class/many-to-one");

            element.AttributeShouldEqual("name", "Parent");
            element.AttributeShouldEqual("cascade", "all");
            element.AttributeShouldEqual("column", "Parent_id");
        }

        [Test]
        public void Creating_a_many_to_one_reference_sets_the_column_overrides()
        {
            var map = new ClassMap<MappedObject>();
            map.References(x => x.Parent);

            document = map.CreateMapping(new MappingVisitor());

            Debug.WriteLine(document.DocumentElement.OuterXml);

            var element = (XmlElement)document.DocumentElement.SelectSingleNode("class/many-to-one");

            element.AttributeShouldEqual("foreign-key", "FK_MappedObjectToParent");
        }

        [Test]
        public void DetermineTheTableName()
        {
            var map = new ClassMap<MappedObject>();
            map.TableName.ShouldEqual("MappedObject");

            map.TableName = "Different";
            map.TableName.ShouldEqual("Different");
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
            var map = new ClassMap<MappedObject>();
            map.Map(x => x.Color);

            document = map.CreateMapping(new MappingVisitor());
            XmlElement element = elementForProperty("Color");

            Debug.WriteLine(element.OuterXml);

            element.AttributeShouldEqual("type", typeof (GenericEnumMapper<ColorEnum>).AssemblyQualifiedName);
            element["column"].AttributeShouldEqual("name", "Color");
            element["column"].AttributeShouldEqual("sql-type", "string");
            element["column"].AttributeShouldEqual("length", "50");
        }

        [Test]
        public void MapASimplePropertyWithNoOverrides()
        {
            var map = new ClassMap<MappedObject>();
            map.Map(x => x.Name);

            document = map.CreateMapping(new MappingVisitor());
            XmlElement element = elementForProperty("Name");

            element.AttributeShouldEqual("name", "Name");
            element.AttributeShouldEqual("column", "Name");
            element.AttributeShouldEqual("type", "String");
        }

        [Test]
        public void SimpleProperty_picks_up_maximum_length_for_string_fields()
        {
            var map = new ClassMap<MappedObject>();
            map.Map(x => x.Name);
            map.Map(x => x.NickName);

            document = map.CreateMapping(new MappingVisitor());

            Debug.WriteLine(document.DocumentElement.OuterXml);

            elementForProperty("Name").AttributeShouldEqual("length", "100");
            elementForProperty("NickName").AttributeShouldEqual("length", "10");
        }

        [Test]
        public void SimpleProperty_picks_up_not_null_for_required()
        {
            var map = new ClassMap<MappedObject>();
            map.Map(x => x.Name);
            map.Map(x => x.NickName);

            document = map.CreateMapping(new MappingVisitor());
            elementForProperty("Name").AttributeShouldEqual("not-null", "true");
            elementForProperty("NickName").DoesNotHaveAttribute("not-null");
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
    }

    public class SecondMappedObject
    {
        [Required]
        public string Name { get; set; }
        public long Id { get; set; }
    }

    public class ComponentOfMappedObject
    {
        public string Name { get; set; }

        [Required]
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

        [Required]
        public ComponentOfMappedObject Component { get; set; }

        public SecondMappedObject Parent { get; set; }

        [Required]
        public string Name { get; set; }

        [MaximumStringLength(10)]
        public string NickName { get; set; }


        public IList<ChildObject> Children { get; set; }

        public long Id { get; set; }
    }

    public class ChildObject
    {
        public int Id { get; set; }
    }
}
