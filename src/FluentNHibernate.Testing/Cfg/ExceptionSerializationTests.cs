using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentNHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg;

[TestFixture]
[Obsolete("BinaryFormatter serialization is obsolete and should not be used.")]
public class ExceptionSerializationTests
{
    [Test]
    public void ShouldSerializeFluentConfigurationExceptionCorrectly()
    {
        var original = new FluentConfigurationException("Test", new Exception());
        original.PotentialReasons.Add("reason 1");
        original.PotentialReasons.Add("reason 2");

        var formatter = new BinaryFormatter();

        using (var stream = new MemoryStream())
        {
            formatter.Serialize(stream, original);
            stream.Position = 0;

            var result = formatter.Deserialize(stream) as FluentConfigurationException;

            original.Message.ShouldEqual(result.Message);
            Assert.That(result.PotentialReasons, Is.EquivalentTo(original.PotentialReasons));
        }
    }

    [Test]
    public void ShouldSerializeUnknownPropertyExceptionCorrectly()
    {
        var original = new UnknownPropertyException(typeof(string), "Property1");
        UnknownPropertyException result;

        using (var stream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, original);
            stream.Position = 0;

            result = formatter.Deserialize(stream) as UnknownPropertyException;
        }

        result.Message.ShouldEqual(original.Message);
        result.Property.ShouldEqual(original.Property);
        result.Type.ShouldEqual(original.Type);
    }
}
