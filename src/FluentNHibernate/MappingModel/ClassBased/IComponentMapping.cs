using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public interface IComponentMapping : IMapping
    {
        bool HasColumnPrefix { get; }
        string ColumnPrefix { get; }
        ParentMapping Parent { get; }
        bool Insert { get; }
        bool Update { get; }
        string Access { get; }
        Type ContainingEntityType { get; }
        string Name { get; }
        Member Member { get; }
        Type Type { get; }
        bool OptimisticLock { get; }
        bool Unique { get; }
        IEnumerable<ManyToOneMapping> References { get; }
        IEnumerable<CollectionMapping> Collections { get; }
        IEnumerable<PropertyMapping> Properties { get; }
        IEnumerable<IComponentMapping> Components { get; }
        IEnumerable<OneToOneMapping> OneToOnes { get; }
        IEnumerable<AnyMapping> Anys { get; }
        ComponentType ComponentType { get; }
        TypeReference Class { get; }
        bool Lazy { get; }
        void AddProperty(PropertyMapping mapping);
        void AddComponent(IComponentMapping mapping);
        void AddOneToOne(OneToOneMapping mapping);
        void AddCollection(CollectionMapping mapping);
        void AddReference(ManyToOneMapping mapping);
        void AddAny(AnyMapping mapping);
    }
}