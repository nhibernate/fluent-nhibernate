using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public interface IDefaultableEnumerable<T> : IEnumerable<T>
    {
        IEnumerable<T> Defaults { get; }
        IEnumerable<T> UserDefined { get; }
    }
}