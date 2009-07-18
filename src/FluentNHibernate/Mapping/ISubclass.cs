using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public interface ISubclassMappingProvider
    {
        SubclassMapping GetSubclassMapping();
    }
}