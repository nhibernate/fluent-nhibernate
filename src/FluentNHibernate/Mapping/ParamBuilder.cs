using System;
using System.Collections.Generic;

namespace FluentNHibernate.Mapping
{
    public class ParamBuilder
    {
        private readonly IDictionary<string, string> parameters;

        public ParamBuilder(IDictionary<string, string> parameters)
        {
            this.parameters = parameters;
        }

        public ParamBuilder AddParam(string name, string value)
        {
            parameters.Add(name, value);
            return this;
        }
    }
}