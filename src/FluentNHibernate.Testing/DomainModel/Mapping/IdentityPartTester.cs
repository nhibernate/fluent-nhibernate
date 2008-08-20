using System;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Mapping;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Util;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
	[TestFixture]
	public class IdentityPartTester
	{
		[Test, Explicit]
		public void Scratch()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);
			var id = new IdentityPart(property);

			var document = new XmlDocument();
			var element = document.CreateElement("root");
			id.Write(element, new MappingVisitor());
			Debug.Write(element.InnerXml);
		}


		[Test]
		public void Defaults()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);

			var id = new IdentityPart(property);

			var document = new XmlDocument();
			var element = document.CreateElement("root");
			id.Write(element, new MappingVisitor());
			var idElement = (XmlElement)element.SelectSingleNode("id");
			var generatorElemenent = (XmlElement) idElement.SelectSingleNode("generator");
			
			idElement.AttributeShouldEqual("name", "IntId");
			idElement.AttributeShouldEqual("column", "IntId");
			idElement.AttributeShouldEqual("type", "Int32");
			generatorElemenent.AttributeShouldEqual("class", "identity");
		}

		[Test]
		public void ColumnName_SpecifyColumnName()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);

			var id = new IdentityPart(property, "Id");

			var document = new XmlDocument();
			var element = document.CreateElement("root");
			id.Write(element, new MappingVisitor());
			var idElement = (XmlElement)element.SelectSingleNode("id");

			idElement.AttributeShouldEqual("column", "Id");
		}

		[Test]
		public void GeneratorClass_Long_DefaultsToIdentity()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.LongId);
			
			var id = new IdentityPart(property);
			
			var document = new XmlDocument();
			var element = document.CreateElement("root");
			id.Write(element, new MappingVisitor());
			var idElement = (XmlElement)element.SelectSingleNode("id");
			var generatorElemenent = (XmlElement)idElement.SelectSingleNode("generator");
			
			idElement.AttributeShouldEqual("type", "Int64");
			generatorElemenent.AttributeShouldEqual("class", "identity");
		}

		[Test]
		public void GeneratorClass_Guid_DefaultsToGuidComb()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);

			var id = new IdentityPart(property);

			var document = new XmlDocument();
			var element = document.CreateElement("root");
			id.Write(element, new MappingVisitor());
			var idElement = (XmlElement)element.SelectSingleNode("id");
			var generatorElemenent = (XmlElement)idElement.SelectSingleNode("generator");

			idElement.AttributeShouldEqual("type", "Guid");
			generatorElemenent.AttributeShouldEqual("class", "guid.comb");
		}

		[Test]
		public void GeneratorClass_String_DefaultsToAssigned()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.StringId);

			var id = new IdentityPart(property);

			var document = new XmlDocument();
			var element = document.CreateElement("root");
			id.Write(element, new MappingVisitor());
			var idElement = (XmlElement)element.SelectSingleNode("id");
			var generatorElemenent = (XmlElement)idElement.SelectSingleNode("generator");

			idElement.AttributeShouldEqual("type", "String");
			generatorElemenent.AttributeShouldEqual("class", "assigned");
		}

		[Test]
		public void GeneratorClass_CanSpecifyIncrement()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);

			var id = new IdentityPart(property).GeneratedBy.Increment();
			
			XmlNodeList ignored;
			XmlElement generatorElement = getGeneratorElement(id, out ignored);

			generatorElement.AttributeShouldEqual("class", "increment");
		}

		[Test]
		public void GeneratorClass_CanSpecifyIdentity()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);

			var id = new IdentityPart(property).GeneratedBy.Identity();

			XmlNodeList ignored;
			XmlElement generatorElement = getGeneratorElement(id, out ignored);

			generatorElement.AttributeShouldEqual("class", "identity");
		}

		[Test]
		public void GeneratorClass_CanSpecifySequence()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);

			var id = new IdentityPart(property).GeneratedBy.Sequence("uid_sequence");

			XmlNodeList paramElements;
			XmlElement generatorElement = getGeneratorElement(id, out paramElements);

			generatorElement.AttributeShouldEqual("class", "sequence");
			paramElements.Count.ShouldEqual(1);
			paramElements[0].Name.ShouldEqual("param");
			paramElements[0].Attributes["name"].Value.ShouldEqual("sequence");
			paramElements[0].InnerXml.ShouldEqual("uid_sequence");
		}

		[Test]
		public void GeneratorClass_CanSpecifyHiLo()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);

			var id = new IdentityPart(property).GeneratedBy.HiLo("hi_value", "next_value", "100");

			XmlNodeList paramElements;
			XmlElement generatorElement = getGeneratorElement(id, out paramElements);

			generatorElement.AttributeShouldEqual("class", "hilo");
			paramElements.Count.ShouldEqual(3);
			paramElements[0].Name.ShouldEqual("param");
			paramElements[0].Attributes["name"].Value.ShouldEqual("table");
			paramElements[0].InnerXml.ShouldEqual("hi_value");

			paramElements[1].Name.ShouldEqual("param");
			paramElements[1].Attributes["name"].Value.ShouldEqual("column");
			paramElements[1].InnerXml.ShouldEqual("next_value");

			paramElements[2].Name.ShouldEqual("param");
			paramElements[2].Attributes["name"].Value.ShouldEqual("max_lo");
			paramElements[2].InnerXml.ShouldEqual("100");

			var id2 = new IdentityPart(property).GeneratedBy.HiLo("hi_value", "next_value", "100");
		}

		private XmlElement getGeneratorElement(IdentityPart id, out XmlNodeList paramElements)
		{
			var document = new XmlDocument();
			var element = document.CreateElement("root");
			id.Write(element, new MappingVisitor());
			var generatorElement = (XmlElement)element.SelectSingleNode("id/generator");
			paramElements = generatorElement.SelectNodes("param");
			return generatorElement;
		}

		[Test]
		public void GeneratorClass_CanSpecifySeqHiLo()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);

			var id = new IdentityPart(property).GeneratedBy.SeqHiLo("hi_value","100");

			XmlNodeList paramElements;
			XmlElement generatorElement = getGeneratorElement(id, out paramElements);

			generatorElement.AttributeShouldEqual("class", "seqhilo");
			paramElements.Count.ShouldEqual(2);
			paramElements[0].Name.ShouldEqual("param");
			paramElements[0].Attributes["name"].Value.ShouldEqual("sequence");
			paramElements[0].InnerXml.ShouldEqual("hi_value");

			paramElements[1].Name.ShouldEqual("param");
			paramElements[1].Attributes["name"].Value.ShouldEqual("max_lo");
			paramElements[1].InnerXml.ShouldEqual("100");
		}

		[Test]
		public void GeneratorClass_CanSpecifyUuidHex()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.StringId);

			var id = new IdentityPart(property).GeneratedBy.UuidHex("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

			XmlNodeList paramElements;
			XmlElement generatorElement = getGeneratorElement(id, out paramElements);

			generatorElement.AttributeShouldEqual("class", "uuid.hex");
			paramElements.Count.ShouldEqual(1);
			paramElements[0].Name.ShouldEqual("param");
			paramElements[0].Attributes["name"].Value.ShouldEqual("format");
			paramElements[0].InnerXml.ShouldEqual("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
		}

		[Test]
		public void GeneratorClass_CanSpecifyUuidString()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.StringId);

			var id = new IdentityPart(property).GeneratedBy.UuidString();

			XmlNodeList ignored;
			XmlElement generatorElement = getGeneratorElement(id, out ignored);

			generatorElement.AttributeShouldEqual("class", "uuid.string");
		}

		[Test]
		public void GeneratorClass_CanSpecifyGuid()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);

			var id = new IdentityPart(property).GeneratedBy.Guid();

			XmlNodeList ignored;
			XmlElement generatorElement = getGeneratorElement(id, out ignored);

			generatorElement.AttributeShouldEqual("class", "guid");
		}

		[Test]
		public void GeneratorClass_CanSpecifyGuidComb()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);

			var id = new IdentityPart(property).GeneratedBy.GuidComb();

			XmlNodeList ignored;
			XmlElement generatorElement = getGeneratorElement(id, out ignored);

			generatorElement.AttributeShouldEqual("class", "guid.comb");
		}

		[Test]
		public void GeneratorClass_CanSpecifyNative()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);

			var id = new IdentityPart(property).GeneratedBy.Native();

			XmlNodeList ignored;
			XmlElement generatorElement = getGeneratorElement(id, out ignored);

			generatorElement.AttributeShouldEqual("class", "native");
		}

		[Test]
		public void GeneratorClass_CanSpecifyAssigned()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);

			var id = new IdentityPart(property).GeneratedBy.Assigned();

			XmlNodeList ignored;
			XmlElement generatorElement = getGeneratorElement(id, out ignored);

			generatorElement.AttributeShouldEqual("class", "assigned");	
		}

		[Test]
		public void GeneratorClass_CanSpecifyForeign()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);

			var id = new IdentityPart(property).GeneratedBy.Foreign("Parent");

			XmlNodeList paramElements;
			XmlElement generatorElement = getGeneratorElement(id, out paramElements);

			generatorElement.AttributeShouldEqual("class", "foreign");
			paramElements.Count.ShouldEqual(1);
			paramElements[0].Name.ShouldEqual("param");
			paramElements[0].Attributes["name"].Value.ShouldEqual("property");
			paramElements[0].InnerXml.ShouldEqual("Parent");
		}

		[Test]
		[ExpectedException(typeof (InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForIncrement()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
			new IdentityPart(property).GeneratedBy.Increment();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForIdentity()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
			new IdentityPart(property).GeneratedBy.Identity();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForSequence()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
			new IdentityPart(property).GeneratedBy.Sequence("no");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForHiLo()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
			new IdentityPart(property).GeneratedBy.HiLo("no","no","no");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForSeqHiLo()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
			new IdentityPart(property).GeneratedBy.SeqHiLo("no", "no");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeString_ForUuidHex()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);
			new IdentityPart(property).GeneratedBy.UuidHex("format");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeString_ForUuidString()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);
			new IdentityPart(property).GeneratedBy.UuidString();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeGuid_ForGuid()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);
			new IdentityPart(property).GeneratedBy.Guid();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeGuid_ForGuidComb()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);
			new IdentityPart(property).GeneratedBy.GuidComb();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForNative()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
			new IdentityPart(property).GeneratedBy.Native();
		}

        [Test]
        public void WithUnsavedValue_SetsUnsavedValueAttributeOnId()
        {
            new MappingTester<IdentityTarget>()
                .ForMapping(c => c.Id(x => x.IntId).WithUnsavedValue(-1))
                .Element("class/id").HasAttribute("unsaved-value", "-1");
        }

        [Test]
        public void UnsavedValueAttributeIsntSetIfThereHasntBeenAValueSpecified()
        {
            new MappingTester<IdentityTarget>()
                .ForMapping(c => c.Id(x => x.IntId))
                .Element("class/id").DoesntHaveAttribute("unsaved-value");
        }

        [Test]
        public void TypeIsSetToTypeName()
        {
            new MappingTester<IdentityTarget>()
                .ForMapping(c => c.Id(x => x.IntId).WithUnsavedValue(-1))
                .Element("class/id").HasAttribute("type", "Int32");
        }

        [Test]
        public void TypeIsSetToFullTypeNameIfTypeGeneric()
        {
            new MappingTester<IdentityTarget>()
                .ForMapping(c => c.Id(x => x.NullableGuidId).WithUnsavedValue(-1))
                .Element("class/id").HasAttribute("type", typeof(Guid?).FullName);
        }

        [Test]
        public void BUGFIX_each_visitor_should_be_asked_to_specify_the_primary_key_name_convention()
        {
            var visitor1 = new MappingVisitor();
            var visitor2 = new MappingVisitor();
            var classMap = new ClassMap<IdentityTarget>();
            classMap.Id(x => x.LongId);

            visitor1.Conventions.GetPrimaryKeyName = prop => "foo";
            visitor2.Conventions.GetPrimaryKeyName = prop => "bar";

            new MappingTester<IdentityTarget>()
                .UsingVisitor(visitor1)
                    .ForMapping(classMap)
                    .Element("class/id").HasAttribute("column", "foo")
                .UsingVisitor(visitor2)
                    .ForMapping(classMap)
                    .Element("class/id").HasAttribute("column", "bar");
            
        }
	}

	public class IdentityTarget
	{
		public virtual int IntId { get; set; }
		public virtual long LongId { get; set; }
		public virtual Guid GuidId { get; set; }
		public virtual Guid? NullableGuidId { get; set; }
		public virtual string StringId { get; set; }
	}
}
