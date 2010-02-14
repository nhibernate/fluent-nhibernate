using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlFilterWriterTester
    {
        [Test]
        public void ShouldWriteTheFilter()
        {
            var filter = new FilterMapping();
            filter.Name = "sid";
            filter.Condition = "Fred = :george";
            XmlFilterWriter writer = new XmlFilterWriter();
            writer.VerifyXml(filter)
                .RootElement.HasName("filter")
                .HasAttribute("name", "sid")
                .HasAttribute("condition", "Fred = :george");
        }
    }
}
