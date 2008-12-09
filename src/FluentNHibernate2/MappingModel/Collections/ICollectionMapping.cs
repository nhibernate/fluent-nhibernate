using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionMapping : IMappingBase
    {
        CollectionAttributes Attributes { get; }
        string Name { get; set; }
        KeyMapping Key { get; set; }
        ICollectionContentsMapping Contents { get; set; }
    }
}