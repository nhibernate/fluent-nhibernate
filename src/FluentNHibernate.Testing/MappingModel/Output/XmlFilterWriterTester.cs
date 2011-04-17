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
            filter.Set(x => x.Name, Layer.Defaults, "sid");
            filter.Set(x => x.Condition, Layer.Defaults, "Fred = :george");
            
            var writer = new XmlFilterWriter();
            writer.VerifyXml(filter)
                .RootElement.HasName("filter")
                .HasAttribute("name", "sid")
                .HasAttribute("condition", "Fred = :george");
        }
    }
}
