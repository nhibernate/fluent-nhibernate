using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Data;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.Fixtures;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel
{
    [TestFixture]
    public class ConnectedTester
    {
        private ISessionSource _source;

        [SetUp]
        public void SetUp()
        {
            var properties = new SQLiteConfiguration()
                .UseOuterJoin()
                //.ShowSql()
                .InMemory()
                .ToProperties();

            //var properties = MsSqlConfiguration
            //    .MsSql2005
            //    .ConnectionString
            //    .Server(".")
            //    .Database("FluentNHibernate")
            //    .TrustedConnection
            //    .CreateProperties
            //    .ToProperties();

            _source = new SingleConnectionSessionSourceForSQLiteInMemoryTesting(properties, new TestPersistenceModel());
            _source.BuildSchema();
        }

        [Test]
        public void MappingTest1()
        {
            new PersistenceSpecification<Record>(_source)
                .CheckProperty(r => r.Age, 22)
                .CheckProperty(r => r.Name, "somebody")
                .CheckProperty(r => r.Location, "somewhere")
                .VerifyTheMappings();
        }

        [Test]
        public void Mapping_test_with_arrays()
        {
            new PersistenceSpecification<BinaryRecord>(_source)
                .CheckProperty(r => r.BinaryValue, new byte[] { 1, 2, 3 })
                .VerifyTheMappings();
        }
        [Test]
        public void CanWorkWithNestedSubClasses()
        {
            new PersistenceSpecification<Child2Record>(_source)
                .CheckProperty(r => r.Name, "Foxy")
                .CheckProperty(r => r.Another, "Lady")
                .CheckProperty(r => r.Third, "Yeah")
                .VerifyTheMappings();
        }
    }

    public class RecordMap : ClassMap<Record>
    {
        public RecordMap()
        {
            Id(x => x.Id, "id");
            Map(x => x.Name);
            Map(x => x.Age);
            Map(x => x.Location);
        }
    }

    public class NestedSubClassMap : ClassMap<SuperRecord>
    {
        public NestedSubClassMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            DiscriminateSubClassesOnColumn<string>("Type")
                .SubClass<ChildRecord>(sc =>
                {
                    sc.Map(x => x.Another);
                    sc.SubClass<Child2Record>(sc2 =>
                        sc2.Map(x => x.Third));
                });
        }
    }

    public class SuperRecord  : Entity
    {
        public virtual string Name { get; set; }
    }

    public class ChildRecord : SuperRecord
    {
        public virtual string Another { get; set; }
    }

    public class Child2Record : ChildRecord
    {
        public virtual string Third { get; set; }
    }

    public class Record : Entity
    {
        public virtual string Name { get; set; }
        public virtual int Age { get; set; }
        public virtual string Location { get; set; }
    }

    public class BinaryRecordMap : ClassMap<BinaryRecord>
    {
        public BinaryRecordMap()
        {
            Id(x => x.Id, "id");
            Map(x => x.BinaryValue).Not.Nullable();
        }
    }
    public class BinaryRecord : Entity
    {
        public virtual byte[] BinaryValue { get; set; }
    }

    public class CachedRecordMap 
    {
        private readonly ClassMap<CachedRecord> classMap = new ClassMap<CachedRecord>();
        public ClassMap<CachedRecord> ClassMap
        {
            get { return classMap; }
        }

        public CachedRecordMap()
        {
            classMap.Cache.ReadWrite();
            classMap.Id(x => x.Id, "id");
        }
    }
    public class CachedRecord : Entity
    { }
}
