using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    public interface IHasMappedMembers
    {
        IEnumerable<PropertyMapping> Properties { get; }
        IEnumerable<ICollectionMapping> Collections { get; }
        IEnumerable<ManyToOneMapping> References { get; }
        IEnumerable<ComponentMapping> Components { get; }
        void AddProperty(PropertyMapping property);
        void AddCollection(ICollectionMapping collection);
        void AddReference(ManyToOneMapping manyToOne);
        void AddComponent(ComponentMapping component);
    }
}