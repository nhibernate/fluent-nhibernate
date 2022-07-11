using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.MappingModel.Output
{
    public class EnumBiDictionary<E, V>
        where E : Enum
    {
        private readonly IReadOnlyDictionary<V, E> valueToMember;
        private readonly IReadOnlyDictionary<E, V> memberToValue;

        public EnumBiDictionary(Func<E, V> enumToValueMapper)
        {
            valueToMember = Enum.GetValues(typeof(E))
                .Cast<E>()
                .ToDictionary(enumToValueMapper);
            memberToValue = valueToMember.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        public EnumBiDictionary(IReadOnlyDictionary<E, V> enumToValueDict) : this(e => enumToValueDict[e])
        {
        }

        public V this [E enumMember]
        {
            get { return memberToValue[enumMember]; }
        }

        public E this [V value]
        {
            get { return valueToMember[value]; }
        }
    }
}
