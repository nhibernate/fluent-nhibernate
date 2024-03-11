using System.Linq;
using FluentNHibernate.Cfg;
using NHibernate.Type;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Serialization;

[TestFixture]
public class SerializationFixture
{
    [Test]
    public void all_itypes_should_be_serializables()
    {
        var userTypes = typeof(FluentConfiguration)
            .Assembly.GetTypes()
            .Where(t => typeof(IType).IsAssignableFrom(t));

        foreach (var userTypeType in userTypes)
        {
            Assert.That(userTypeType.IsSerializable, $"{userTypeType.Name} should be serializable.");
        }
    }
}
