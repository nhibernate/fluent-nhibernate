using System;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Xml;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel
{
    internal static class MappingTestingExtensions
    {
        public static void ShouldBeValidAgainstSchema<T>(this MappingBase<T> mapping) where T : class, new()
        {
            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(mapping);

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