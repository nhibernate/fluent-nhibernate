using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FluentNHibernate.MappingModel;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Xml
{
    public class MappingXmlSerializer
    {
        public XmlDocument Serialize<T>(MappingBase<T> mapping) where T : class, new()
        {
            using (var stream = new MemoryStream())
            using (var writer = new XmlTextWriter(stream, System.Text.Encoding.Default))
            {
                var s = new XmlSerializer(typeof(T));
                s.Serialize(writer, mapping.Hbm);
                stream.Position = 0;
                var doc = new XmlDocument();
                doc.Load(stream);

                using (XmlTextReader reader = new XmlTextReader("nhibernate-mapping.xsd"))
                    doc.Schemas.Add(XmlSchema.Read(reader, null));

                return doc;
            }
        }
    }
}
