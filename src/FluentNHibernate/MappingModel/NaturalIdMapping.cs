using System;
using System.Collections.Generic;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class NaturalIdMapping(AttributeStore attributes) : MappingBase
{
    readonly AttributeStore attributes = attributes;
    readonly List<PropertyMapping> properties = [];
    readonly List<ManyToOneMapping> manyToOnes = [];

    public NaturalIdMapping()
        : this(new AttributeStore()) { }

    public bool Mutable => attributes.GetOrDefault<bool>();

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
