using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public interface IComponentMapping
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
        ParentMapping Parent { get; set; }
        bool Insert { get; set; }
        bool Update { get; set; }
        string Access { get; set; }
        Type ContainingEntityType { get; }
        string Name { get; set; }
        PropertyInfo PropertyInfo { get; }
        Type Type { get; }
        bool Lazy { get; }
        bool OptimisticLock { get; }
        bool Unique { get; }
        IEnumerable<ManyToOneMapping> References { get; }
        IEnumerable<ICollectionMapping> Collections { get; }
        IEnumerable<PropertyMapping> Properties { get; }
        IEnumerable<IComponentMapping> Components { get; }
        IEnumerable<OneToOneMapping> OneToOnes { get; }
        IEnumerable<AnyMapping> Anys { get; }
        void AddProperty(PropertyMapping mapping);
        void AddComponent(IComponentMapping mapping);
        void AddOneToOne(OneToOneMapping mapping);
        void AddCollection(ICollectionMapping mapping);
        void AddReference(ManyToOneMapping mapping);
        void AddAny(AnyMapping mapping);
    }
}