using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel.Mapping;

namespace FluentNHibernate.Testing.Automapping
{
    public class AutoMappingTester<T> : MappingTester<T>
    {
        public AutoMappingTester(AutoPersistenceModel mapper)
            : base(mapper)
        {
            ForMapping((ClassMap<T>)null);
        }
    }
}