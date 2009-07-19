using System.Collections.Generic;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    public interface IHasMappedMembers
    {
        IEnumerable<PropertyMapping> Properties { get; }
        IEnumerable<ICollectionMapping> Collections { get; }
        IEnumerable<ManyToOneMapping> References { get; }
        IEnumerable<IComponentMapping> Components { get; }
        IEnumerable<OneToOneMapping> OneToOnes { get; }
        IEnumerable<AnyMapping> Anys { get; }
        void AddProperty(PropertyMapping property);
        void AddCollection(ICollectionMapping collection);
        void AddReference(ManyToOneMapping manyToOne);
        void AddComponent(IComponentMapping component);
        void AddOneToOne(OneToOneMapping mapping);
        void AddAny(AnyMapping mapping);
    }
}