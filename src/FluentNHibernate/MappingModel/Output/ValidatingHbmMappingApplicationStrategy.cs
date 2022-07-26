using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    /// <summary>
    /// A mapping application strategy which utilizes both <see cref="MappingHbmConverter"/> and <see cref="MappingXmlSerializer"/>
    /// for translation, checking that the results align before using the HBM variant.
    /// </summary>
    public class ValidatingHbmMappingApplicationStrategy : MappingApplicationStrategyBase<Tuple<XmlDocument, HbmMapping>>
    {
        public ValidatingHbmMappingApplicationStrategy()
        {
        }

        protected override Tuple<XmlDocument, HbmMapping> ToIntermediateForm(HibernateMapping mapping)
        {
            var document = new MappingXmlSerializer().Serialize(mapping);
            var hbm = new MappingHbmConverter().Convert(mapping);

            return new Tuple<XmlDocument, HbmMapping>(document, hbm);
        }

        protected override void ApplyIntermediateFormToConfiguration(Tuple<XmlDocument, HbmMapping> intermediateForm, Configuration cfg)
        {
            var document = intermediateForm.Item1;
            // Note that cfg.LoadMappingDocument isn't necessarily thread safe, but we can't guarantee thread safety for this
            // method anyway, so it shouldn't be an issue.
            var loadedHbmMapping = cfg.LoadMappingDocument(new XmlTextReader(document.InnerXml, XmlNodeType.Document, null), "(SerializedXmlDocument)").Document;
            var directHbmMapping = intermediateForm.Item2;

            string loadedHbmJson = ToJson(loadedHbmMapping);
            string directHbmJson = ToJson(directHbmMapping);
            if (loadedHbmJson != directHbmJson)
                throw new Exception("Loaded and direct HBM mappings did not match\nLoaded:\n" + loadedHbmJson + "\n!=\nDirect:\n" + directHbmJson);

            cfg.AddDeserializedMapping(directHbmMapping, "(XmlDocument)");
        }

        private static readonly IReadOnlyList<Type> KnownTypes = new List<Type>() {
                typeof(NHibernate.Type.Int32Type),
                typeof(Dictionary<String, LayeredValues>),
                typeof(HbmClass),
                typeof(HbmId),
                typeof(HbmProperty),
                typeof(HbmColumn),
                typeof(HbmManyToOne),
                typeof(HbmJoin),
                typeof(HbmSet),
                typeof(HbmOneToMany),
                typeof(HbmManyToMany),
                typeof(HbmSubclass),
                typeof(HbmJoinedSubclass),
                typeof(HbmUnionSubclass),
                typeof(HbmArray),
                typeof(HbmBag),
                typeof(HbmList),
                typeof(HbmMap),
                typeof(HbmSet),
                typeof(HbmOneToMany),
                typeof(HbmManyToMany),
                typeof(HbmIndex),
                typeof(HbmIndexManyToMany),
                typeof(HbmListIndex),
            };

        private string ToJson(object mapping)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(mapping.GetType(), KnownTypes);
            MemoryStream memStream = new MemoryStream();
            jsonSerializer.WriteObject(memStream, mapping);
            return Encoding.Default.GetString(memStream.ToArray());
        }
    }
}
