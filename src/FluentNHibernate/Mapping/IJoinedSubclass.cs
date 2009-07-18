using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public interface IJoinedSubclassMappingProvider
    {
        JoinedSubclassMapping GetJoinedSubclassMapping();
    }
}