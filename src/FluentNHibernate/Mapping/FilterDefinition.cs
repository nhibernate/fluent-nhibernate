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

        public FilterDefinitionMapping GetFilterMapping()
        {
            var mapping = new FilterDefinitionMapping();
            mapping.Name = filterName;
            mapping.Condition = filterCondition;
            foreach (var pair in Parameters)
            {
                mapping.Parameters.Add(pair);
            }
            return mapping;
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        public HibernateMapping GetHibernateMapping()
        {
            return new HibernateMapping();
        }
    }
}
