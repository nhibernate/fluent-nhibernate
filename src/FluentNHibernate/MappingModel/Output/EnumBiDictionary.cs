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

        public EnumBiDictionary(IReadOnlyDictionary<E, V> enumToValueDict)
        {
            valueToMember = enumToValueDict.ToDictionary(pair => pair.Value, pair => pair.Key); // Obtain this by inverting the input dictionary
            memberToValue = valueToMember.ToDictionary(pair => pair.Value, pair => pair.Key); // Invert the inversion (we could also just copy it, but this is more consistent)
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
