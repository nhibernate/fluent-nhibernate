using FluentNHibernate.Testing.DomainModel;

namespace FluentNHibernate.Testing.Fixtures
{
    public class TestPersistenceModel : PersistenceModel
    {
        public TestPersistenceModel()
        {
            AddMappingsFromSource(new StubTypeSource(typeof(RecordMap), typeof(BinaryRecordMap), typeof(RecordWithNullablePropertyMap), typeof(RecordFilter), typeof(NestedSubClassMap)));
        }
    }
}