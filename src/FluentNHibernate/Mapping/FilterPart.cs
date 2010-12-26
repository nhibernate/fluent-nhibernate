using System;
using System.Xml;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

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

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(FilterPart)) return false;
            return Equals((FilterPart)obj);
        }

        public FilterMapping GetFilterMapping()
        {
            var mapping = new FilterMapping();
            mapping.Name = Name;
            mapping.Condition = Condition;
            return mapping;
        }

        public bool Equals(FilterPart other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.filterName, filterName) && Equals(other.condition, condition) && Equals(other.attributes, attributes);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (filterName != null ? filterName.GetHashCode() : 0);
                result = (result * 397) ^ (condition != null ? condition.GetHashCode() : 0);
                result = (result * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
                return result;
            }
        }
    }
}
