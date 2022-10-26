using System.Linq;
using FluentNHibernate.Infrastructure;
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
        public void ShouldResolveAllConverters()
        {
            var converters = from type in typeof(IHbmConverter<,>).Assembly.GetTypes()
                          from interfaceType in type.GetInterfaces()
                          where !type.IsAbstract && interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IHbmConverter<,>)
                          select interfaceType;

            foreach (var type in converters)
            {
                try
                {
                    container.Resolve(type);
                }
                catch (ResolveException resolveEx)
                {
                    throw new AssertionException(string.Format("Unable to resolve converter {0}", type), resolveEx);
                }
            }
        }
    }
}
