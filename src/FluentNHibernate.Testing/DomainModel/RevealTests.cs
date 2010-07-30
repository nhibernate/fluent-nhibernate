using System;
using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel
{
    [TestFixture]
    public class RevealTests
    {
        [Test]
        public void Can_use_manytomany_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map =>
                {
                    map.Id(x => x.Id);
                    map.HasManyToMany<ExampleClass>(Reveal.Member<StringTarget>("PrivateObject"));
                })
                .Element("class/bag").HasAttribute("name", "PrivateObject");
        }

        [Test]
        public void Can_use_id_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map =>
                {
                    map.Id(x => x.Id);
                    map.Id(Reveal.Member<StringTarget>("PrivateObject"));
                })
                .Element("class/id").HasAttribute("name", "PrivateObject");
        }

        [Test]
        public void Can_use_references_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map =>
                {
                    map.Id(x => x.Id);
                    map.References(Reveal.Member<StringTarget>("PrivateObject"));
                })
                .Element("class/many-to-one").HasAttribute("name", "PrivateObject");
        }
    }

    public class StringTarget
    {
        public int Id { get; set; }
        private Double DoubleProperty { get; set; }
        private int IntProperty { get; set; }
        private string PrivateProperty { get; set; }
        protected string ProtectedProperty { get; set; }
        protected string PublicProperty { get; set; }
        private IList<ExampleClass> PrivateCollection { get; set; }
        private IDictionary PrivateDictionary { get; set; }
        private ExampleClass PrivateObject { get; set; }
    }
}