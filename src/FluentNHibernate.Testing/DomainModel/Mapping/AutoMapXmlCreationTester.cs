using System;
using System.Xml;
using FluentNHibernate.Mapping;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
	[TestFixture]
	public class AutoMapXmlCreationTester
	{
		[Test]
		public void SimpleMappedObject()
		{
			var autoMap = new ClassMap<SimpleAutoMappedObject>();
			autoMap.GenerateAutoMappings();

			var document = autoMap.CreateMapping(new MappingVisitor());
			var classElement = (XmlElement)document.DocumentElement.SelectSingleNode("class");

			classElement.ShouldHaveChild("id");
			classElement.ShouldHaveChild("id/generator");

			classElement.ShouldHaveChild("property[@name='FirstName']");
			classElement.ShouldHaveChild("property[@name='LastName']");
			classElement.ShouldHaveChild("property[@name='Age']");
		}

		[Test]
		public void SimpleAutoMappedObjectWithByteArray()
		{
			var autoMap = new ClassMap<SimpleAutoMappedObjectWithByteArray>();
			autoMap.GenerateAutoMappings();

			var document = autoMap.CreateMapping(new MappingVisitor());
			var classElement = (XmlElement)document.DocumentElement.SelectSingleNode("class");

			classElement.ShouldHaveChild("property[@name='Password']");
		}

		[Test]
		public void SimpleAutoMappedObjectWithNullableClrProperty()
		{
			var autoMap = new ClassMap<SimpleAutoMappedObjectWithNullableClrProperty>();
			autoMap.GenerateAutoMappings();

			var document = autoMap.CreateMapping(new MappingVisitor());
			var classElement = (XmlElement)document.DocumentElement.SelectSingleNode("class");

			classElement.ShouldHaveChild("property[@name='UpdatedOn']");
		}

		[Test]
		[ExpectedException(typeof(MappingException))]
		public void SimpleMappedObject_WithoutID_ShouldThrowMappingException()
		{
			var autoMap = new ClassMap<SimpleAutoMappedObject_WithoutID>();

			autoMap.GenerateAutoMappings();
		}

		[Test]
		public void AutoMappedObjectWithManyToOne_MapsAsManyToOne()
		{
			var autoMap = new ClassMap<AutoMappedObjectWithManyToOne>();
			autoMap.GenerateAutoMappings();

			var document = autoMap.CreateMapping(new MappingVisitor());
			var classElement = (XmlElement)document.DocumentElement.SelectSingleNode("class");

			classElement.ShouldHaveChild("many-to-one[@name='ManyToOne']");
		}
	}

	public class AutoMappedObjectWithManyToOne : SimpleAutoMappedObject
	{
		public ManyToOneObject ManyToOne { get; set; }
	}

	public class ManyToOneObject
	{
	}

	public class SimpleAutoMappedObjectWithByteArray : SimpleAutoMappedObject
	{
		public byte[] Password { get; set; }
	}

	public class SimpleAutoMappedObjectWithNullableClrProperty : SimpleAutoMappedObject
	{
		public DateTime? UpdatedOn { get; set; }
	}

	public class SimpleAutoMappedObject : SimpleAutoMappedObject_WithoutID
	{
		public int ID { get; set; }
	}

	public class SimpleAutoMappedObject_WithoutID
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
	}
}
