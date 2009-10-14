using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentNHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg
{
    [TestFixture]
    public class FluentConfigurationExceptionTests
    {
        [Test]
        public void ShouldSerializeCorrectly()
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

                Assert.AreEqual(original.Message, result.Message);

                Assert.AreEqual(original.PotentialReasons.Count, result.PotentialReasons.Count);
                Assert.AreEqual(original.PotentialReasons[0], result.PotentialReasons[0]);
                Assert.AreEqual(original.PotentialReasons[1], result.PotentialReasons[1]);
            }



        }
    }
}