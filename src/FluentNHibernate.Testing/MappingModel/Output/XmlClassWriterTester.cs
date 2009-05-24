using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlClassWriterTester
    {
        [Test]
        public void ShouldWriteTheJoins()
        {
            var classMapping = new ClassMapping();
            classMapping.AddJoin(new JoinMapping());

            var joinDocument = new XmlDocument();
            joinDocument.AppendChild(joinDocument.CreateElement("join"));

            var joinWriter = MockRepository.GenerateMock<IXmlWriter<JoinMapping>>();
            joinWriter
                .Expect(x => x.Write(classMapping.Joins.First()))
                .Return(joinDocument);                

            XmlClassWriter writer = new XmlClassWriter(null, null, null, null, null, joinWriter, null, null, null);

            writer.VerifyXml(classMapping)
                .Element("join").Exists();
        }
    }
}
