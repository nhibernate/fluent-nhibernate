using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface ICacheInstance : ICacheInspector
    {
        void ReadWrite();
        void NonStrictReadWrite();
        void ReadOnly();
        void Transactional();
        void IncludeAll();
        void IncludeNonLazy();
        void CustomInclude(string include);
        void CustomUsage(string custom);
        void Region(string name);
    }
}