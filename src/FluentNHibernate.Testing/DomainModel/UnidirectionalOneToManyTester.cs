using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel;

public class UnidirectionalOneToManyTester
{
    [SetUp]
    public void SetUp()
    {
        var properties = SQLiteFrameworkConfigurationFactory
            .CreateStandardConfiguration()
            .UseOuterJoin()
            .ShowSql()
            .InMemory()
            .ToProperties();

        var model = new PersistenceModel();
        model.Add(typeof(OrderMap));
        model.Add(typeof(LineItemMap));
        _source = new SingleConnectionSessionSourceForSQLiteInMemoryTesting(properties, model);
        _source.BuildSchema();
    }

    ISessionSource _source;

    [Test]
    public void ShouldHandleUnidirectionalCollections()
    {
        var order = new Order() { LineItems = new List<LineItem>() };
        order.LineItems.Add(new LineItem());
        new PersistenceSpecification<Order>(_source)
            /* NHibernate.PropertyValueException: not-null property references
             * a null or transient value LineItem._Order.LineItemsBackref */
            .CheckInverseList(o => o.LineItems, order.LineItems, i => i.Id)
            .VerifyTheMappings();
    }
}

public class Order
{
    public virtual Guid Id { get; set; }
    public virtual ICollection<LineItem> LineItems { get; set; }
}

public class LineItem
{
    public virtual Guid Id { get; set; }
}

public class OrderMap : ClassMap<Order>
{
    public OrderMap()
    {
        Id(x => x.Id).GeneratedBy.GuidComb();
        HasMany(x => x.LineItems)
            .Not.Inverse()
            .Not.KeyNullable()
            .Not.KeyUpdate()
            .Cascade.AllDeleteOrphan();
    }
}

public class LineItemMap : ClassMap<LineItem>
{
    public LineItemMap()
    {
        Id(x => x.Id).GeneratedBy.GuidComb();
    }
}

