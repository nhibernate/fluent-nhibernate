using FluentNHibernate.AutoMap;
using FluentNHibernate.Testing.Xml;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    //[TestFixture]
    //public class IntegrationsTests
    //{
    //    [Test]
    //    public void ShouldMapIdenties()
    //    {
    //        var document = AutoPersistenceModel
    //            .MapEntitiesFromAssemblyOf<IdentityClass>()
    //            .Where(q => q == typeof (IdentityClass))
    //            .OutputXml();

    //        new MappingXmlTestHelper(document)
    //            .Element("class")
    //            .Element("id")
    //            .HasAttribute("name", "Id")
    //            .Element("column")
    //            .HasAttribute("name", "Id")
    //            .Exists();
    //    }

    //    [Test]
    //    public void ShouldMapProperties()
    //    {
    //        var document = AutoPersistenceModel
    //            .MapEntitiesFromAssemblyOf<IdentityClass>()
    //            .Where(q => q == typeof(IdentityClass))
    //            .OutputXml();

    //        new MappingXmlTestHelper(document)
    //            .Element("class")
    //            .Element("property")
    //            .HasAttribute("name", "Name")
    //            .Exists();
    //    }
        
    //    public class IdentityClass
    //    {
    //        public int Id { get; set; }
    //        public string Name { get; set; }
    //    }
    //}
}