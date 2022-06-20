using System.Linq;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmConverterContainerTester
    {
        private HbmConverterContainer container;

        [SetUp]
        public void CreateContainer()
        {
            container = new HbmConverterContainer();
        }

        [Test]
        public void ShouldResolveAllWriters()
        {
            var writers = from type in typeof(IHbmConverter<,>).Assembly.GetTypes()
                          from interfaceType in type.GetInterfaces()
                          where interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IHbmConverter<,>)
                          select interfaceType;

            foreach (var type in writers)
            {
                container.Resolve(type);
            }
        }
    }
}
