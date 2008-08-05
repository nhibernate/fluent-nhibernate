using System;

namespace FluentNHibernate.Metadata
{
    public interface IDeleter
    {
        void DeleteAll(Type type);
        void FollowDependencies(TypeDependency dependency);
    }
}