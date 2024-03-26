﻿using FluentNHibernate.Data;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.Fixtures;
using NHibernate;
using NUnit.Framework;
using static FluentNHibernate.Testing.Cfg.SQLiteFrameworkConfigurationFactory;

namespace FluentNHibernate.Testing.DomainModel;

[TestFixture]
public class ConnectedTester
{
    ISessionSource source;

    [SetUp]
    public void SetUp()
    {
        var properties = CreateStandardConfiguration()
            .UseOuterJoin()
            .InMemory()
            .ToProperties();

        source = new SingleConnectionSessionSourceForSQLiteInMemoryTesting(properties, new TestPersistenceModel());
        source.BuildSchema();
    }

    [Test]
    public void Mapping_simple_properties()
    {
        new PersistenceSpecification<Record>(source)
            .CheckProperty(r => r.Age, 22)
            .CheckProperty(r => r.Name, "somebody")
            .CheckProperty(r => r.Location, "somewhere")
            .VerifyTheMappings();
    }

#if NETFRAMEWORK
        [Test]
        public void Mapping_test_with_arrays()
        {
            new PersistenceSpecification<BinaryRecord>(source)
                .CheckProperty(r => r.BinaryValue, new byte[] { 1, 2, 3 })
                .VerifyTheMappings();
        }
#else
    [Test, Ignore("Currently not supported by Msqlite with NETStandard")]
    public void Mapping_test_with_arrays()
    {
        new PersistenceSpecification<BinaryRecord>(source)
            .CheckProperty(r => r.BinaryValue, new byte[] { 1, 2, 3 })
            .VerifyTheMappings();
    }
#endif

    [Test]
    public void CanWorkWithNestedSubClasses()
    {
        new PersistenceSpecification<Child2Record>(source)
            .CheckProperty(r => r.Name, "Foxy")
            .CheckProperty(r => r.Another, "Lady")
            .CheckProperty(r => r.Third, "Yeah")
            .VerifyTheMappings();
    }

    [Test]
    public void MappingTest2_NullableProperty()
    {
        new PersistenceSpecification<RecordWithNullableProperty>(source)
            .CheckProperty(x => x.Age, null)
            .CheckProperty(x => x.Name, "somebody")
            .CheckProperty(x => x.Location, "somewhere")
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
        ApplyFilter<RecordFilter>();
    }
}

// ignored warning for obsolete SubClass
#pragma warning disable 612, 618

public sealed class NestedSubClassMap : ClassMap<SuperRecord>
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

#pragma warning restore 612, 618

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

public class CachedRecordMap : ClassMap<CachedRecord>
{
    CachedRecordMap()
    {
        Cache.ReadWrite();
        Id(x => x.Id, "id");
    }
}
public class CachedRecord : Entity
{ }

public class RecordFilter : FilterDefinition
{
    public RecordFilter()
    {
        WithName("ageHighEnough").WithCondition("Age > :age").AddParameter("age", NHibernateUtil.Int32);
    }
}

public sealed class RecordWithNullablePropertyMap : ClassMap<RecordWithNullableProperty>
{
    public RecordWithNullablePropertyMap()
    {
        Id(x => x.Id);
        Map(x => x.Name);
        Map(x => x.Age);
        Map(x => x.Location);
    }
}

public class RecordWithNullableProperty : Entity
{
    public virtual string Name { get; set; }
    public virtual int? Age { get; set; }
    public virtual string Location { get; set; }
}
