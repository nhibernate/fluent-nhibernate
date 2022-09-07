using System;
using System.Xml.Serialization;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlLinkedEnumBiDictionary<E> : EnumBiDictionary<E, string>
        where E : Enum
    {
        public XmlLinkedEnumBiDictionary() : base(e => e.GetCustomAttribute<XmlEnumAttribute>().Name)
        {
        }
    }
}
