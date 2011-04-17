using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using NHibernate.Type;

namespace FluentNHibernate.Mapping
{
    public abstract class FilterDefinition : IFilterDefinition
    {
        private string filterName;
        private string filterCondition;
        private readonly IDictionary<string, IType> parameters;

        protected FilterDefinition()
        {
            parameters = new Dictionary<string, IType>();
        }

        public string Name
        {
            get { return filterName; }
        }

        public IEnumerable<KeyValuePair<string, IType>> Parameters
        {
            get { return parameters; }
        }

        public FilterDefinition WithName(string name)
        {
            filterName = name;
            return this;
        }

        public FilterDefinition WithCondition(string condition)
        {
            filterCondition = condition;
            return this;
        }

        public FilterDefinition AddParameter(string name, IType type)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("The name is mandatory", "name");
            if (type == null) throw new ArgumentNullException("type");
            parameters.Add(name, type);
            return this;
        }

        FilterDefinitionMapping IFilterDefinition.GetFilterMapping()
        {
            var mapping = new FilterDefinitionMapping();
            mapping.Set(x => x.Name, Layer.Defaults, filterName);
            mapping.Set(x => x.Condition, Layer.Defaults, filterCondition);
            foreach (var pair in Parameters)
            {
                mapping.Parameters.Add(pair);
            }
            return mapping;
        }

        HibernateMapping IFilterDefinition.GetHibernateMapping()
        {
            return new HibernateMapping();
        }
    }
}
