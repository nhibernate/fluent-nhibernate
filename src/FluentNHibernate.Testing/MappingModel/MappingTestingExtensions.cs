using System;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Xml;
using NUnit.Framework;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Xml;

namespace FluentNHibernate.Testing.MappingModel
{
    internal static class MappingTestingExtensions
    {
        public static MappingXmlTestHelper VerifyXml<T>(this IXmlWriter<T> writer, T model)
        {
            var document = writer.Write(model);

            return new MappingXmlTestHelper(document);
        }
        
        public static void ShouldGenerateValidOutput<T>(this IXmlWriter<T> writer, T model)
        {
            var document = writer.Write(model);
            ShouldBeValidXml(document);
        }

        public static void ShouldBeValidAgainstSchema(this HibernateMapping mapping)
        {
            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(mapping);
            ShouldBeValidXml(document);
        }

        public static void ShouldBeValidXml(this XmlDocument document)
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