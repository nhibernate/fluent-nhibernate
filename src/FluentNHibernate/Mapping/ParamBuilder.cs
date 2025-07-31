using System.Collections.Generic;

namespace FluentNHibernate.Mapping;

public class ParamBuilder(IDictionary<string, string> parameters)
{
    public ParamBuilder AddParam(string name, string value)
    {
        parameters.Add(name, value);
        return this;
    }
}
