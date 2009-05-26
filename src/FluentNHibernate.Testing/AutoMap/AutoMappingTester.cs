using System;
using FluentNHibernate;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NHibernate.Cfg;

public class AutoMappingTester<T> : MappingTester<T>
{
    public AutoMappingTester(AutoPersistenceModel mapper)
    {
        mapper.CompileMappings();
        
        var mapping = mapper.FindMapping<T>();
        
        ForMapping((IClassMap)mapping);
    }
}