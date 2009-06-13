using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface ICollectionInspector : IOneToManyCollectionInspector, IManyToManyCollectionInspector
    {
        new IKeyInspector Key { get; }
        new string TableName { get; }
    }

    public interface IOneToManyCollectionInspector : IInspector
    {
        IKeyInspector Key { get; }
        IOneToManyInspector OneToMany { get; }
        string TableName { get; }
    }

    public interface IManyToManyCollectionInspector : IInspector
    {
        IKeyInspector Key { get; }
        IManyToManyInspector ManyToMany { get; }
        Type ChildType { get; }
        string TableName { get; }
    }
}