using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class HibernateMapping(AttributeStore attributes) : MappingBase, IEquatable<HibernateMapping>
{
    readonly IList<ClassMapping> classes = new List<ClassMapping>();
    readonly IList<FilterDefinitionMapping> filters = new List<FilterDefinitionMapping>();
    readonly IList<ImportMapping> imports = new List<ImportMapping>();
    readonly AttributeStore attributes = attributes;

    public HibernateMapping(): this(new AttributeStore())
    {}

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessHibernateMapping(this);

        foreach (var import in Imports)
            visitor.Visit(import);

        foreach (var classMapping in Classes)
            visitor.Visit(classMapping);

        foreach (var filterMapping in Filters)
            visitor.Visit(filterMapping);
    }

    public IEnumerable<ClassMapping> Classes => classes;

    public IEnumerable<FilterDefinitionMapping> Filters => filters;

    public IEnumerable<ImportMapping> Imports => imports;

    public void AddClass(ClassMapping classMapping)
    {
        classes.Add(classMapping);            
    }

    public void AddFilter(FilterDefinitionMapping filterMapping)
    {
        filters.Add(filterMapping);
    }

    public void AddImport(ImportMapping importMapping)
    {
        imports.Add(importMapping);
    }

    public string Catalog => attributes.GetOrDefault<string>();

    public string DefaultAccess => attributes.GetOrDefault<string>();

    public string DefaultCascade => attributes.GetOrDefault<string>();

    public bool AutoImport => attributes.GetOrDefault<bool>();

    public string Schema => attributes.GetOrDefault<string>();

    public bool DefaultLazy => attributes.GetOrDefault<bool>();

    public string Namespace => attributes.GetOrDefault<string>();

    public string Assembly => attributes.GetOrDefault<string>();

    public bool Equals(HibernateMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return other.classes.ContentEquals(classes) &&
               other.filters.ContentEquals(filters) &&
               other.imports.ContentEquals(imports) &&
               Equals(other.attributes, attributes);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(HibernateMapping)) return false;
        return Equals((HibernateMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = (classes is not null ? classes.GetHashCode() : 0);
            result = (result * 397) ^ (filters is not null ? filters.GetHashCode() : 0);
            result = (result * 397) ^ (imports is not null ? imports.GetHashCode() : 0);
            result = (result * 397) ^ (attributes is not null ? attributes.GetHashCode() : 0);
            return result;
        }
    }

    public void Set<T>(Expression<Func<HibernateMapping, T>> expression, int layer, T value)
    {
        Set(expression.ToMember().Name, layer, value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
        attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute)
    {
        return attributes.IsSpecified(attribute);
    }
}
