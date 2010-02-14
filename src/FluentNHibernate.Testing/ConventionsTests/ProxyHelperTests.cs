using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;
using FluentNHibernate.Conventions.Helpers;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class ProxyHelperTests
    {
        [Test]
        public void ProxyUsedForTypeWillReturnAProxyConvention()
        {
            new MappingTester<ProxiedObject>()
                .Conventions(x => x.Add(Proxy<IProxiedObject>.UsedForType<ProxiedObject>()))
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.HasMany(x => x.Children);
                })

                // The mapping for ProxiedObject should specify the proxy type using the proxy attribute
                .Element("class").HasAttribute("proxy", typeof(IProxiedObject).AssemblyQualifiedName)

                // The mapping for the one-to-many should specify the PERSISTENT TYPE.
                .Element("class/bag/one-to-many").HasAttribute("class", typeof(ProxiedObject).AssemblyQualifiedName);                
        }

    }
}