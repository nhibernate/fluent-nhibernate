using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IJoinedSubclassMappingProvider
    {
        JoinedSubclassMapping GetJoinedSubclassMapping();
    }
}