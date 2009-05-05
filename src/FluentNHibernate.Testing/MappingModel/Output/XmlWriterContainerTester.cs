using System.Linq;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlWriterContainerTester
    {
        private XmlWriterContainer container;

        [SetUp]
        public void CreateContainer()
        {
            container = new XmlWriterContainer();
        }

        [Test]
        public void ShouldResolveAllWriters()
        {
            var writers = from type in typeof(IXmlWriter<>).Assembly.GetTypes()
                          from interfaceType in type.GetInterfaces()
                          where interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IXmlWriter<>)
                          select interfaceType;

            foreach (var type in writers)
            {
                container.Resolve(type);
            }
        }
    }
}
