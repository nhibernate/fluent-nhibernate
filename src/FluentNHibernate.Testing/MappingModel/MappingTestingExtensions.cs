using System;
using System.Xml;
using FluentNHibernate.Testing.Xml;
using FluentNHibernate.MappingModel.Output;

namespace FluentNHibernate.Testing.MappingModel
{
    internal static class MappingTestingExtensions
    {
        public static MappingXmlTestHelper VerifyXml<T>(this IXmlWriter<T> writer, T model)
        {
            var document = writer.Write(model);

            return new MappingXmlTestHelper(document);
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