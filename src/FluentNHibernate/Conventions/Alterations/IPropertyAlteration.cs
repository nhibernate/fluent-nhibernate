using System;

namespace FluentNHibernate.Conventions.Alterations
{
    public interface IPropertyAlteration
        : IInsertAlteration, IUpdateAlteration,
          IReadOnlyAlteration, INullableAlteration,
          IAccessAlteration
    {
        void CustomTypeIs<T>();
        void CustomTypeIs(Type type);
        void CustomTypeIs(string type);
        void CustomSqlTypeIs(string sqlType);
        void Unique();
        void UniqueKey(string keyName);

        IPropertyAlteration Not { get; }
    }
}
