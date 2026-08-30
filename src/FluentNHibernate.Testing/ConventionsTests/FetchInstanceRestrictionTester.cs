using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests;

[TestFixture]
public class FetchInstanceRestrictionTester
{
    [Test]
    public void join_convention_fetch_select_is_allowed()
    {
        var mapping = new JoinMapping();

        new JoinInstance(mapping).Fetch.Select();

        mapping.Fetch.ShouldEqual("select");
    }

    [Test]
    public void join_convention_fetch_subselect_throws()
    {
        var instance = new JoinInstance(new JoinMapping());

        Assert.Throws<NotSupportedException>(() => instance.Fetch.Subselect());
    }

    [Test]
    public void many_to_one_convention_fetch_subselect_throws()
    {
        var instance = new ManyToOneInstance(new ManyToOneMapping());

        Assert.Throws<NotSupportedException>(() => instance.Fetch.Subselect());
    }

    [Test]
    public void one_to_one_convention_fetch_subselect_throws()
    {
        var instance = new OneToOneInstance(new OneToOneMapping());

        Assert.Throws<NotSupportedException>(() => instance.Fetch.Subselect());
    }
}
