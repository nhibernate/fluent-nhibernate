using FluentNHibernate;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Metadata;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NHibernate.Cfg;

public class AutoMappingTester<T> : MappingTester<T>
{
    public AutoMappingTester(AutoPersistenceModel mapper)
    {
        document = mapper.FindMapping<T>().CreateMapping(new MappingVisitor(mapper.Conventions, new Configuration(), new DependencyChain()));
        currentElement = document.DocumentElement;
    }

}