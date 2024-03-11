using System;
using System.Collections.Generic;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased;

[Serializable]
public abstract class ClassMappingBase(AttributeStore attributes) : MappingBase, IHasMappedMembers
{
    readonly AttributeStore attributes = attributes;
    readonly MappedMembers mappedMembers = new();
    readonly IList<SubclassMapping> subclasses = new List<SubclassMapping>();

    public abstract string Name { get; }
    public abstract Type Type { get; }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        mappedMembers.AcceptVisitor(visitor);

        foreach (var subclass in Subclasses)
            visitor.Visit(subclass);
    }

    #region IHasMappedMembers

    public IEnumerable<ManyToOneMapping> References => mappedMembers.References;

    public IEnumerable<Collections.CollectionMapping> Collections => mappedMembers.Collections;

    public IEnumerable<PropertyMapping> Properties => mappedMembers.Properties;

    public IEnumerable<IComponentMapping> Components => mappedMembers.Components;

    public IEnumerable<OneToOneMapping> OneToOnes => mappedMembers.OneToOnes;

    public IEnumerable<AnyMapping> Anys => mappedMembers.Anys;

    public IEnumerable<JoinMapping> Joins => mappedMembers.Joins;

    public IEnumerable<FilterMapping> Filters => mappedMembers.Filters;

    public IEnumerable<SubclassMapping> Subclasses => subclasses;

    public IEnumerable<StoredProcedureMapping> StoredProcedures => mappedMembers.StoredProcedures;

    public void AddProperty(PropertyMapping property)
    {
        mappedMembers.AddProperty(property);
    }

    public void AddOrReplaceProperty(PropertyMapping mapping)
    {
        mappedMembers.AddOrReplaceProperty(mapping);
    }

    public void AddCollection(Collections.CollectionMapping collection)
    {
        mappedMembers.AddCollection(collection);
    }

    public void AddOrReplaceCollection(Collections.CollectionMapping mapping)
    {
        mappedMembers.AddOrReplaceCollection(mapping);
    }

    public void AddReference(ManyToOneMapping manyToOne)
    {
        mappedMembers.AddReference(manyToOne);
    }

    public void AddOrReplaceReference(ManyToOneMapping manyToOne)
    {
        mappedMembers.AddOrReplaceReference(manyToOne);
    }

    public void AddComponent(IComponentMapping componentMapping)
    {
        mappedMembers.AddComponent(componentMapping);
    }

    public void AddOrReplaceComponent(IComponentMapping mapping)
    {
        mappedMembers.AddOrReplaceComponent(mapping);
    }

    public void AddOneToOne(OneToOneMapping mapping)
    {
        mappedMembers.AddOneToOne(mapping);
    }

    public void AddOrReplaceOneToOne(OneToOneMapping mapping)
    {
        mappedMembers.AddOrReplaceOneToOne(mapping);
    }

    public void AddAny(AnyMapping mapping)
    {
        mappedMembers.AddAny(mapping);
    }

    public void AddOrReplaceAny(AnyMapping mapping)
    {
        mappedMembers.AddOrReplaceAny(mapping);
    }

    public void AddJoin(JoinMapping mapping)
    {
        mappedMembers.AddJoin(mapping);
    }

    public void AddOrReplaceJoin(JoinMapping mapping)
    {
        mappedMembers.AddOrReplaceJoin(mapping);
    }

    public void AddFilter(FilterMapping mapping)
    {
        mappedMembers.AddFilter(mapping);
    }

    public void AddOrReplaceFilter(FilterMapping mapping)
    {
        mappedMembers.AddOrReplaceFilter(mapping);
    }

    public void AddSubclass(SubclassMapping subclass)
    {
        subclasses.Add(subclass);
    }

    public void AddStoredProcedure(StoredProcedureMapping mapping)
    {
        mappedMembers.AddStoredProcedure(mapping);
    }
    #endregion

    public override string ToString()
    {
        return string.Format("ClassMapping({0})", Type.Name);
    }

    public bool Equals(ClassMappingBase other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.mappedMembers, mappedMembers) &&
               other.subclasses.ContentEquals(subclasses);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(ClassMappingBase)) return false;
        return Equals((ClassMappingBase)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((mappedMembers is not null ? mappedMembers.GetHashCode() : 0) * 397) ^ (subclasses is not null ? subclasses.GetHashCode() : 0);
        }
    }

    public void MergeAttributes(AttributeStore clone)
    {
        clone.CopyTo(attributes);
    }
}
