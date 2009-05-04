using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IJoinedSubclass : IClasslike, IMappingPart
    {
        void WithTableName(string tableName);
        JoinedSubclassMapping GetJoinedSubclassMapping();
    }
}