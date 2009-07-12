using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface ICacheInstance : ICacheInspector
    {
        void ReadWrite();
        void NonStrictReadWrite();
        void ReadOnly();
        void Custom(string custom);
        void Region(string name);
    }
}