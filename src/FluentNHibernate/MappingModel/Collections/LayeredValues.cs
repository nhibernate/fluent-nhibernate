using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FluentNHibernate.MappingModel.Collections;

[Serializable]
public class LayeredValues : Dictionary<int, object>
{
    public LayeredValues()
    { }

    [Obsolete("This API supports obsolete formatter-based serialization and will be removed in a future version")]
    protected LayeredValues(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
