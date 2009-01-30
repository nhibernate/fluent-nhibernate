using System.Reflection;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionMapping : IMappingBase, INameable
    {
        bool IsInverse { get; }
        bool IsLazy { get; }
        KeyMapping Key { get; set; }
        ICollectionContentsMapping Contents { get; set; }
        CollectionAttributes Attributes { get; }
        PropertyInfo PropertyInfo { get; set;  }
    }
}