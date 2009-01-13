using System;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using FluentNHibernate.AutoMap.TestFixtures.SuperTypes;
using FluentNHibernate.Mapping;
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
			var id = new IdentityPart<IdentityTarget>(property);

			var document = new XmlDocument();
			var element = document.CreateElement("root");
			id.Write(element, new MappingVisitor());
			Debug.Write(element.InnerXml);
		}

		[Test]
		public void Defaults()
		{
		    new MappingTester<IdentityTarget>()
		        .ForMapping(mapping => mapping.Id(x => x.IntId))
		        .Element("class/id")
		            .Exists()
		            .HasAttribute("name", "IntId")
		            .HasAttribute("column", "IntId")
		            .HasAttribute("type", "Int32")
		        .Element("class/id/generator")
		            .Exists()
		            .HasAttribute("class", "identity");
		}

        [Test]
        public void IdIsAlwaysFirstElementInClass()
        {
            new MappingTester<IdentityTarget>()
                .ForMapping(m =>
                {
                    m.Map(x => x.StringId); // just a property in this case
                    m.Id(x => x.IntId);
                })
                .Element("class/*[1]").HasName("id");
        }

		[Test]
		public void ColumnName_SpecifyColumnName()
		{
		    new MappingTester<IdentityTarget>()
		        .ForMapping(mapping => mapping.Id(x => x.IntId, "Id"))
		        .Element("class/id").HasAttribute("column", "Id");
		}

		[Test]
		public void GeneratorClass_Long_DefaultsToIdentity()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping => mapping.Id(x => x.LongId))
                .Element("class/id").HasAttribute("type", "Int64")
                .Element("class/id/generator").HasAttribute("class", "identity");
		}

		[Test]
		public void GeneratorClass_Guid_DefaultsToGuidComb()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping => mapping.Id(x => x.GuidId))
                .Element("class/id").HasAttribute("type", "Guid")
                .Element("class/id/generator").HasAttribute("class", "guid.comb");
		}

		[Test]
		public void GeneratorClass_String_DefaultsToAssigned()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping => mapping.Id(x => x.StringId))
                .Element("class/id").HasAttribute("type", "String")
                .Element("class/id/generator").HasAttribute("class", "assigned");
		}

		[Test]
		public void GeneratorClass_CanSpecifyIncrement()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.IntId)
                        .GeneratedBy.Increment())
                .Element("class/id/generator").HasAttribute("class", "increment");
		}

		[Test]
		public void GeneratorClass_CanSpecifyIdentity()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.IntId)
                        .GeneratedBy.Identity())
                .Element("class/id/generator").HasAttribute("class", "identity");
		}

		[Test]
		public void GeneratorClass_CanSpecifySequence()
		{
		    new MappingTester<IdentityTarget>()
		        .ForMapping(mapping =>
                    mapping.Id(x => x.IntId)
                        .GeneratedBy.Sequence("uid_sequence"))
		        .Element("class/id/generator").HasAttribute("class", "sequence")
		        .Element("class/id/generator/param")
		            .Exists()
		            .HasAttribute("name", "sequence")
		            .ValueEquals("uid_sequence");
		}

		[Test]
		public void GeneratorClass_CanSpecifyHiLo()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.IntId)
                        .GeneratedBy.HiLo("hi_value", "next_value", "100"))
                .Element("class/id/generator").HasAttribute("class", "hilo")
                .Element("class/id/generator/param[1]")
                    .Exists()
                    .HasAttribute("name", "table")
                    .ValueEquals("hi_value")
                .Element("class/id/generator/param[2]")
                    .Exists()
                    .HasAttribute("name", "column")
                    .ValueEquals("next_value")
                .Element("class/id/generator/param[3]")
                    .Exists()
                    .HasAttribute("name", "max_lo")
                    .ValueEquals("100");
		}

	    [Test]
		public void GeneratorClass_CanSpecifySeqHiLo()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.IntId)
                        .GeneratedBy.SeqHiLo("hi_value", "100"))
                .Element("class/id/generator").HasAttribute("class", "seqhilo")
                .Element("class/id/generator/param[1]")
                    .Exists()
                    .HasAttribute("name", "sequence")
                    .ValueEquals("hi_value")
                .Element("class/id/generator/param[2]")
                    .Exists()
                    .HasAttribute("name", "max_lo")
                    .ValueEquals("100");
		}

		[Test]
		public void GeneratorClass_CanSpecifyUuidHex()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.StringId)
                        .GeneratedBy.UuidHex("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"))
                .Element("class/id/generator").HasAttribute("class", "uuid.hex")
                .Element("class/id/generator/param")
                    .Exists()
                    .HasAttribute("name", "format")
                    .ValueEquals("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
		}

		[Test]
		public void GeneratorClass_CanSpecifyUuidString()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.StringId)
                        .GeneratedBy.UuidString())
                .Element("class/id/generator").HasAttribute("class", "uuid.string");
		}

		[Test]
		public void GeneratorClass_CanSpecifyGuid()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.GuidId)
                        .GeneratedBy.Guid())
                .Element("class/id/generator").HasAttribute("class", "guid");
		}

		[Test]
		public void GeneratorClass_CanSpecifyGuidComb()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.GuidId)
                        .GeneratedBy.GuidComb())
                .Element("class/id/generator").HasAttribute("class", "guid.comb");
		}

		[Test]
		public void GeneratorClass_CanSpecifyNative()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.IntId)
                        .GeneratedBy.Native())
                .Element("class/id/generator").HasAttribute("class", "native");
		}

		[Test]
		public void GeneratorClass_CanSpecifyAssigned()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.GuidId)
                        .GeneratedBy.Assigned())
                .Element("class/id/generator").HasAttribute("class", "assigned");
		}

		[Test]
		public void GeneratorClass_CanSpecifyForeign()
		{
            new MappingTester<IdentityTarget>()
                .ForMapping(mapping =>
                    mapping.Id(x => x.IntId)
                        .GeneratedBy
                        .Foreign("Parent"))
                .Element("class/id/generator").HasAttribute("class", "foreign")
                .Element("class/id/generator/param")
                    .Exists()
                    .HasAttribute("name", "property")
                    .ValueEquals("Parent");
		}

		[Test]
		[ExpectedException(typeof (InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForIncrement()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
            new IdentityPart<IdentityTarget>(property).GeneratedBy.Increment();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForIdentity()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
            new IdentityPart<IdentityTarget>(property).GeneratedBy.Identity();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForSequence()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
            new IdentityPart<IdentityTarget>(property).GeneratedBy.Sequence("no");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForHiLo()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
            new IdentityPart<IdentityTarget>(property).GeneratedBy.HiLo("no", "no", "no");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForSeqHiLo()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
            new IdentityPart<IdentityTarget>(property).GeneratedBy.SeqHiLo("no", "no");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeString_ForUuidHex()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);
            new IdentityPart<IdentityTarget>(property).GeneratedBy.UuidHex("format");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeString_ForUuidString()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);
            new IdentityPart<IdentityTarget>(property).GeneratedBy.UuidString();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeGuid_ForGuid()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);
            new IdentityPart<IdentityTarget>(property).GeneratedBy.Guid();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeGuid_ForGuidComb()
		{

			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId);
            new IdentityPart<IdentityTarget>(property).GeneratedBy.GuidComb();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityType_MustBeIntegral_ForNative()
		{
			PropertyInfo property = ReflectionHelper.GetProperty<IdentityTarget>(x => x.GuidId);
            new IdentityPart<IdentityTarget>(property).GeneratedBy.Native();
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
        public void Each_visitor_should_be_asked_to_specify_the_primary_key_name_convention()
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

        [Test]
        public void Key_convention_receives_superclass_type()
        {
            var visitor = new MappingVisitor();

            visitor.Conventions.GetPrimaryKeyNameFromType =
                type => type.Name + "Id";

            new MappingTester<ExampleClass>()
                .UsingVisitor(visitor)
                    .ForMapping(mapping => mapping.Id(x => x.Id))
                    .Element("class/id").HasAttribute("column", "ExampleClassId");
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
