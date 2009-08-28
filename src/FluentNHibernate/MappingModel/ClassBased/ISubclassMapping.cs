using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public interface ISubclassMapping : IMappingBase  
    {
        string Name { get; set; }
        Type Type { get; }
        IEnumerable<ISubclassMapping> Subclasses { get; }
        IEnumerable<IComponentMapping> Components { get; }
        IEnumerable<PropertyMapping> Properties { get; }
        IEnumerable<ICollectionMapping> Collections { get; }
        void OverrideAttributes(AttributeStore store);
        void AddProperty(PropertyMapping mapping);
        void AddComponent(IComponentMapping mapping);
        void AddOneToOne(OneToOneMapping mapping);
        void AddCollection(ICollectionMapping mapping);
        void AddReference(ManyToOneMapping mapping);
        void AddSubclass(ISubclassMapping mapping);
        void AddAny(AnyMapping mapping);
    }
}