using System.Reflection;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionMapping : IMappingBase, INameable
    {
        bool IsInverse { get; }
        bool IsLazy { get; }
        KeyMapping Key { get; set; }
        ICollectionContentsMapping Contents { get; set; }
        AttributeStore<ICollectionMapping> Attributes { get; }
        PropertyInfo PropertyInfo { get; set;  }
    }
}