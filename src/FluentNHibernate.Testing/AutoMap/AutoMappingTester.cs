using System;
using FluentNHibernate;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NHibernate.Cfg;

public class AutoMappingTester<T> : MappingTester<T>
{
    public AutoMappingTester(AutoPersistenceModel mapper)
    {
        mapper.CompileMappings();
        document = mapper.FindMapping<T>().CreateMapping(new MappingVisitor(new Configuration()));
        currentElement = document.DocumentElement;
    }

}
