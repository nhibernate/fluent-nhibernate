using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlLinkedEnumBiDictionary<E>
        where E : Enum
    {
        private readonly IReadOnlyDictionary<string, E> stringToMember;
        private readonly IReadOnlyDictionary<E, string> memberToString;

        public XmlLinkedEnumBiDictionary()
        {
            stringToMember = Enum.GetValues(typeof(E))
                .Cast<E>()
                .ToDictionary(e => e.GetCustomAttribute<XmlEnumAttribute>().Name);
            memberToString = stringToMember.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        public string this [E enumValue]
        {
            get { return memberToString[enumValue]; }
        }

        public E this [string xmlString]
        {
            get { return stringToMember[xmlString]; }
        }
    }
}
