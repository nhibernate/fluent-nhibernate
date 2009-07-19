using System;
using FluentNHibernate;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NHibernate.Cfg;

namespace FluentNHibernate.Testing.AutoMap
{
    public class AutoMappingTester<T> : MappingTester<T>
    {
        public AutoMappingTester(AutoPersistenceModel mapper)
            : base(mapper)
        {
            mapper.CompileMappings();
        
            ForMapping((ClassMap<T>)null);
        }
    }
}