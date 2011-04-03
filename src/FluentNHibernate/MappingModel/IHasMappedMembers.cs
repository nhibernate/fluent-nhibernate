using System.Collections.Generic;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    public interface IHasMappedMembers
    {
        IEnumerable<PropertyMapping> Properties { get; }
        IEnumerable<CollectionMapping> Collections { get; }
        IEnumerable<ManyToOneMapping> References { get; }
        IEnumerable<IComponentMapping> Components { get; }
        IEnumerable<OneToOneMapping> OneToOnes { get; }
        IEnumerable<AnyMapping> Anys { get; }
        IEnumerable<FilterMapping> Filters { get; }
        void AddProperty(PropertyMapping property);
        void AddCollection(CollectionMapping collection);
        void AddReference(ManyToOneMapping manyToOne);
        void AddComponent(IComponentMapping component);
        void AddOneToOne(OneToOneMapping mapping);
        void AddAny(AnyMapping mapping);
        void AddFilter(FilterMapping mapping);
    }
}