using System;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

[TestFixture]
public class FetchSubselectRestrictionTester
{
    [Test]
    public void join_fetch_subselect_throws()
    {
        var join = new JoinPart<Target>("secondary");

        Assert.Throws<NotSupportedException>(() => join.Fetch.Subselect());
    }

    [Test]
    public void join_fetch_join_is_allowed()
    {
        var join = new JoinPart<Target>("secondary");

        Assert.DoesNotThrow(() => join.Fetch.Join());
    }

    [Test]
    public void join_fetch_select_is_allowed()
    {
        var join = new JoinPart<Target>("secondary");

        Assert.DoesNotThrow(() => join.Fetch.Select());
    }

    [Test]
    public void many_to_one_fetch_subselect_throws()
    {
        Assert.Throws<NotSupportedException>(() => new ManyToOneSubselectMap());
    }

    [Test]
    public void one_to_one_fetch_subselect_throws()
    {
        Assert.Throws<NotSupportedException>(() => new OneToOneSubselectMap());
    }

    [Test]
    public void fetch_property_return_types_stay_the_base_type_for_binary_compatibility()
    {
        // These must remain FetchTypeExpression<T> (not a derived type). Widening a
        // return type is a binary breaking change: an assembly compiled against a
        // previous FluentNHibernate release would fail with MissingMethodException
        // when this build is dropped in as a replacement.
        Assert.That(typeof(JoinPart<Target>).GetProperty("Fetch").PropertyType,
            Is.EqualTo(typeof(FetchTypeExpression<JoinPart<Target>>)));
        Assert.That(typeof(ManyToOnePart<Other>).GetProperty("Fetch").PropertyType,
            Is.EqualTo(typeof(FetchTypeExpression<ManyToOnePart<Other>>)));
        Assert.That(typeof(OneToOnePart<Target>).GetProperty("Fetch").PropertyType,
            Is.EqualTo(typeof(FetchTypeExpression<OneToOnePart<Target>>)));
    }

    class Target
    {
        public int Id { get; set; }
        public Other Other { get; set; }
    }

    class Other
    {
        public int Id { get; set; }
        public Target Target { get; set; }
    }

    class ManyToOneSubselectMap : ClassMap<Target>
    {
        public ManyToOneSubselectMap()
        {
            Id(x => x.Id);
            References(x => x.Other).Fetch.Subselect();
        }
    }

    class OneToOneSubselectMap : ClassMap<Other>
    {
        public OneToOneSubselectMap()
        {
            Id(x => x.Id);
            HasOne(x => x.Target).Fetch.Subselect();
        }
    }
}
