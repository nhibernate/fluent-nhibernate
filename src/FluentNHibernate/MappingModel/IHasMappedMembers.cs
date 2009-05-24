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
        IEnumerable<ComponentMappingBase> Components { get; }
        IEnumerable<OneToOneMapping> OneToOnes { get; }
        void AddProperty(PropertyMapping property);
        void AddCollection(ICollectionMapping collection);
        void AddReference(ManyToOneMapping manyToOne);
        void AddComponent(ComponentMappingBase component);
        void AddOneToOne(OneToOneMapping mapping);
    }
}