using System;
using System.Xml;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IFilter : IFilterMappingProvider
    {
        string Condition { get; }
        string Name { get; }
    }

    /// <summary>
    /// Maps to the Filter element in NH 2.0
    /// </summary>
    public class FilterPart : IFilter
    {
        private readonly string filterName;
        private readonly string condition;
        private readonly AttributeStore<FilterMapping> attributes = new AttributeStore<FilterMapping>();

        public FilterPart(string name) : this(name, null) { }

        public FilterPart(string name, string condition)
        {
            filterName = name;
            this.condition = condition;
        }

        public string Condition
        {
            get { return condition; }
        }
        public string Name
        {
            get { return filterName; }
        }

        public void Write(XmlElement classElement, IMappingModelVisitor visitor)
        {
            XmlElement filterElement = classElement.AddElement("filter");
            filterElement.SetAttribute("name", Name);
            if (!string.IsNullOrEmpty(Condition))
            {
                filterElement.SetAttribute("condition", Condition);
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as FilterPart;
            if (other == null) return false;
            return Name == other.Name && Condition == other.Condition;
        }

        public FilterMapping GetFilterMapping()
        {
            var mapping = new FilterMapping();
            mapping.Name = Name;
            mapping.Condition = Condition;
            return mapping;
        }
    }
}
