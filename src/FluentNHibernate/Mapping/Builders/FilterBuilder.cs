using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Builders
{
    /// <summary>
    /// Maps to the Filter element in NH 2.0
    /// </summary>
    public class FilterBuilder
    {
        readonly FilterMapping mapping;

        public FilterBuilder(FilterMapping mapping)
        {
            this.mapping = mapping;
        }

        public void Condition(string condition)
        {
            mapping.Condition = condition;
        }

        public void Name(string name)
        {
            mapping.Name = name;
        }
    }
}
