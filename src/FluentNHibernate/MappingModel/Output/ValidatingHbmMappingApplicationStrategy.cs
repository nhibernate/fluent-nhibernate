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

        private string ToJson(object mapping)
        {
            List<Type> knownTypes = new List<Type>();
            knownTypes.Add(typeof(NHibernate.Type.Int32Type));
            knownTypes.Add(typeof(Dictionary<String, LayeredValues>));
            knownTypes.Add(typeof(HbmClass));
            knownTypes.Add(typeof(HbmId));
            knownTypes.Add(typeof(HbmProperty));
            knownTypes.Add(typeof(HbmColumn));
            knownTypes.Add(typeof(HbmManyToOne));
            knownTypes.Add(typeof(HbmJoin));
            knownTypes.Add(typeof(HbmSet));
            knownTypes.Add(typeof(HbmOneToMany));
            knownTypes.Add(typeof(HbmManyToMany));
            knownTypes.Add(typeof(HbmSubclass));
            knownTypes.Add(typeof(HbmJoinedSubclass));
            knownTypes.Add(typeof(HbmUnionSubclass));
            knownTypes.Add(typeof(HbmArray));
            knownTypes.Add(typeof(HbmBag));
            knownTypes.Add(typeof(HbmList));
            knownTypes.Add(typeof(HbmMap));
            knownTypes.Add(typeof(HbmSet));
            knownTypes.Add(typeof(HbmOneToMany));
            knownTypes.Add(typeof(HbmManyToMany));
            knownTypes.Add(typeof(HbmIndex));
            knownTypes.Add(typeof(HbmIndexManyToMany));
            knownTypes.Add(typeof(HbmListIndex));

            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(mapping.GetType(), knownTypes);
            MemoryStream memStream = new MemoryStream();
            jsonSerializer.WriteObject(memStream, mapping);
            return Encoding.Default.GetString(memStream.ToArray());
        }
    }
}
