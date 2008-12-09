using System;
using System.Xml;
using System.Xml.Schema;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Xml;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Xml
{
    [TestFixture]
    public class MappingXmlValidatorTester
    {

        [Test]
        public void CanValidateXmlAgainstSchema()
        {
            var mapping = new HibernateMapping();

            // Invalid, cannot use a default meta. Schema validation should fail.
            mapping.Hbm.meta = new HbmMeta[] { new HbmMeta() };

            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(mapping);

            MappingXmlValidator validator = new MappingXmlValidator();
            var result = validator.Validate(document);

            result.Success.ShouldBeFalse();
            result.Messages.ShouldContain("Element 'meta': The required attribute 'attribute' is missing.");
                        
        }
    }
}