using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping;

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
    readonly AttributeStore attributes = new AttributeStore();

    public FilterPart(string name) : this(name, null) { }

    public FilterPart(string name, string condition)
    {
        Name = name;
        this.Condition = condition;
    }

    public string Condition { get; }

    public string Name { get; }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(FilterPart)) return false;
        return Equals((FilterPart)obj);
    }

    FilterMapping IFilterMappingProvider.GetFilterMapping()
    {
        var mapping = new FilterMapping();
        mapping.Set(x => x.Name, Layer.Defaults, Name);
        mapping.Set(x => x.Condition, Layer.Defaults, Condition);
        return mapping;
    }

    public bool Equals(FilterPart other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.Name, Name) && Equals(other.Condition, Condition) && Equals(other.attributes, attributes);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = (Name is not null ? Name.GetHashCode() : 0);
            result = (result * 397) ^ (Condition is not null ? Condition.GetHashCode() : 0);
            result = (result * 397) ^ (attributes is not null ? attributes.GetHashCode() : 0);
            return result;
        }
    }
}
