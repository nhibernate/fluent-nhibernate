using System;
using System.Collections.Generic;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class NaturalIdMapping : MappingBase
{
    private readonly AttributeStore attributes;
    private readonly IList<PropertyMapping> properties = new List<PropertyMapping>();
    private readonly IList<ManyToOneMapping> manyToOnes = new List<ManyToOneMapping>();

    public NaturalIdMapping()
        : this(new AttributeStore()) { }

    public NaturalIdMapping(AttributeStore attributes)
    {
        this.attributes = attributes;
    }

    public bool Mutable => attributes.GetOrDefault<bool>("Mutable");

    public IEnumerable<PropertyMapping> Properties => properties;

    public IEnumerable<ManyToOneMapping> ManyToOnes => manyToOnes;

    public void AddProperty(PropertyMapping mapping)
    {
        properties.Add(mapping);
    }

    public void AddReference(ManyToOneMapping mapping)
    {
        manyToOnes.Add(mapping);
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessNaturalId(this);

        foreach (var key in properties)
            visitor.Visit(key);

        foreach (var key in manyToOnes)
            visitor.Visit(key);
    }

    public override bool IsSpecified(string attribute)
    {
        return attributes.IsSpecified(attribute);
    }

    protected override void Set(string attribute, int layer, object value)
    {
        attributes.Set(attribute, layer, value);
    }
}
