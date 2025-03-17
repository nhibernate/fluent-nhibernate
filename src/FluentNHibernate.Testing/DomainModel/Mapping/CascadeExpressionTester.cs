using System;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

[TestFixture]
public class CascadeExpressionTester
{
    #region Test Setup

    protected CascadeExpression<object> _cascade;
    protected string cascadeValue;
    Func<object> _currentCascadeAction;

    [SetUp]
    public virtual void SetUp()
    {
        cascadeValue = "";
        _cascade = new CascadeExpression<object>(null, value => cascadeValue = value);
    }

    protected CascadeExpressionTester A_call_to(Func<object> cascadeAction)
    {
        _currentCascadeAction = cascadeAction;
        return this;
    }

    public void should_set_the_cascade_value_to(string expected)
    {
        _currentCascadeAction();
        cascadeValue.ShouldEqual(expected);
    }

    #endregion

    [Test]
    public void All_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.All).should_set_the_cascade_value_to("all");
    }

    [Test]
    public void None_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.None).should_set_the_cascade_value_to("none");
    }

    [Test]
    public void SaveUpdate_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.SaveUpdate).should_set_the_cascade_value_to("save-update");
    }

    [Test]
    public void Delete_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.Delete).should_set_the_cascade_value_to("delete");
    }

    [Test]
    public void Merge_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.Merge).should_set_the_cascade_value_to("merge");
    }

    [Test]
    public void Refresh_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.Refresh).should_set_the_cascade_value_to("refresh");
    }

    [Test]
    public void DeleteOrphans_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.DeleteOrphan).should_set_the_cascade_value_to("delete-orphan");
    }

    [Test]
    public void Evict_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.Evict).should_set_the_cascade_value_to("evict");
    }

    [Test]
    public void Lock_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.Lock).should_set_the_cascade_value_to("lock");
    }

    [Test]
    public void Persist_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.Persist).should_set_the_cascade_value_to("save-update, persist");
    }

    [Test]
    public void AllDeleteOrphan_should_add_the_correct_cascade_attribute_to_the_parent_part()
    {
        A_call_to(_cascade.AllDeleteOrphan).should_set_the_cascade_value_to("all-delete-orphan");
    }
}
