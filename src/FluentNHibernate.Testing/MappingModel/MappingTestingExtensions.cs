using System;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Xml;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Xml;
using System.Linq;

namespace FluentNHibernate.Testing.MappingModel
{
    internal static class MappingTestingExtensions
    {
        public static MappingXmlTestHelper VerifyXml<T>(this IXmlWriter<T> writer, T model)
        {
            object hbm = writer.Write(model);
            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.SerializeHbmFragment(hbm);        
            document.OutputXmlToConsole();
            return new MappingXmlTestHelper(document);
        }
        
        public static void ShouldGenerateValidOutput<T>(this IXmlWriter<T> writer, T model)
        {
            object hbm = writer.Write(model);
            hbm.ShouldNotBeNull();  
            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.SerializeHbmFragment(hbm);
            Validate(document);
        }

        public static void ShouldBeValidAgainstSchema(this HibernateMapping mapping)
        {
            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(mapping);
            Validate(document);
        }

        private static void Validate(XmlDocument document)
        {
            MappingXmlValidator validator = new MappingXmlValidator();
            var result = validator.Validate(document);

            document.OutputXmlToConsole();
            Assert.IsTrue(result.Success, result.FullMessageLog);
        }


        public static void OutputXmlToConsole(this XmlDocument document)
        {
            var stringWriter = new System.IO.StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.Formatting = Formatting.Indented;
            document.WriteContentTo(xmlWriter);

            Console.WriteLine(string.Empty);
            Console.WriteLine(stringWriter.ToString());
            Console.WriteLine(string.Empty);
        }
    }
}