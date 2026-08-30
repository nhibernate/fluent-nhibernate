using System;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

[TestFixture]
public class FetchTypeExpressionTester
{
    #region Test Setup
    public FetchTypeExpression<object> _fetchType;
    string fetchValue;

    [SetUp]
    public virtual void SetUp()
    {
        fetchValue = "";
        _fetchType = new FetchTypeExpression<object>(null, value => fetchValue = value);
    }

    protected FetchTypeExpressionTester A_call_to(Func<object> fetchAction)
    {
        fetchAction();
        return this;
    }

    void should_set_the_fetch_value_to(string expected)
    {
        fetchValue.ShouldEqual(expected);
    }

    #endregion

    [Test]
    public void Join_should_add_the_correct_fetch_attribute_to_the_parent_part()
    {
        A_call_to(_fetchType.Join).should_set_the_fetch_value_to("join");
    }

    [Test]
    public void Select_should_add_the_correct_fetch_attribute_to_the_parent_part()
    {
        A_call_to(_fetchType.Select).should_set_the_fetch_value_to("select");
    }

    [Test]
    public void Subselect_should_throw_because_it_is_not_supported_for_associations()
    {
        Assert.Throws<NotSupportedException>(() => _fetchType.Subselect());
    }

    [Test]
    public void Subselect_on_a_collection_fetch_should_add_the_correct_fetch_attribute()
    {
        var collectionFetchType = new CollectionFetchTypeExpression<object>(null, value => fetchValue = value);

        collectionFetchType.Subselect();

        fetchValue.ShouldEqual("subselect");
    }
}
