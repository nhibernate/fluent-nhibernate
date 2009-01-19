using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Xml
{
    public class MappingXmlSerializer
    {
        public XmlDocument Serialize(HibernateMapping mapping)
        {
            var hbm = BuildHbm(mapping);
            return PerformSerialize(hbm);
        }

        public XmlDocument Serialize(HbmMapping hbm)
        {
            return PerformSerialize(hbm);
        }

        public XmlDocument SerializeHbmFragment(object hbm)
        {
            return PerformSerialize(hbm);
        }

        private XmlDocument PerformSerialize(object hbm)
        {
            using (var stream = new MemoryStream())
            using (var writer = new XmlTextWriter(stream, System.Text.Encoding.Default))
            {
                var s = new XmlSerializer(hbm.GetType());
                s.Serialize(writer, hbm);
                stream.Position = 0;
                var doc = new XmlDocument();
                doc.Load(stream);

                using (XmlTextReader reader = new XmlTextReader("nhibernate-mapping.xsd"))
                    doc.Schemas.Add(XmlSchema.Read(reader, null));

                return doc;
            }
        }

        private HbmMapping BuildHbm(HibernateMapping rootMapping)
        {
            HbmHibernateMappingWriter rootWriter =
                new HbmHibernateMappingWriter(new HbmClassWriter(
                    new HbmIdentityWriter(
                        new HbmIdWriter(
                            new HbmColumnWriter(),
                            new HbmIdGeneratorWriter()
                            ),
                        new HbmCompositeIdWriter()
                        ),
                    new HbmCollectionWriter(
                        new HbmBagWriter(
                            new HbmCollectionContentsWriter(
                                new HbmOneToManyWriter()
                                ),
                            new HbmKeyWriter()
                            ),
                        new HbmSetWriter(
                            new HbmCollectionContentsWriter(
                                new HbmOneToManyWriter()),
                            new HbmKeyWriter()
                            )
                    ),
                    new HbmPropertyWriter(),
                    new HbmManyToOneWriter()
                    ));

            return rootWriter.Write(rootMapping);
        }
    }
}
