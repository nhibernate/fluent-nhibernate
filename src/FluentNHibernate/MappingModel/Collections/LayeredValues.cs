using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class LayeredValues : Dictionary<int, object>
    {
        public LayeredValues()
        {}

        protected LayeredValues(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {}
    }
}