using System.Collections.Generic;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

[TestFixture]
public class ComponentElementPartTester
{
    [Test]
    public void CanIncludeParentReference()
    {
        new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.HasMany(x => x.Components)
                    .Component(c =>
                    {
                        c.Map(x => x.Name);
                        c.ParentReference(x => x.MyParent);
                    }))
            .Element("class/bag/composite-element/parent").Exists()
            .HasAttribute("name", "MyParent");
    }

    [Test]
    public void CanSetParentReferenceAccess()
    {
        new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.HasMany(x => x.Components)
                    .Component(c =>
                    {
                        c.Map(x => x.Name);
                        c.ParentReference(x => x.MyParent, x => x.Access.BackingField());
                    }))
            .Element("class/bag/composite-element/parent").Exists()
            .HasAttribute("name", "MyParent")
            .HasAttribute("access", "backfield");
    }

    [Test]
    public void CanSetNestedCompositeElementAccessAccess()
    {
        var tester = new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.HasMany(x => x.Components)
                    .Component(c =>
                    {
                        c.Map(x => x.Name);
                        c.Component(target => target.MyParent, nested => nested.Access.BackingField());
                    }));
        tester
            .Element("class/bag/composite-element/nested-composite-element").Exists()
            .HasAttribute("name", "MyParent")
            .HasAttribute("access", "backfield");
    }

    [Test]
    public void CanCreateNestedCompositeElement()
    {
        new MappingTester<TopLevel>()
            .ForMapping(
                a => a.HasMany(x => x.Items)
                    .AsList()
                    .Component(item =>
                    {
                        item.Component(i => i.Target, n => n.Map(z => z.Name));
                        item.Map(x => x.SomeString);
                    })                        
            )                
            .Element("class/list/composite-element/nested-composite-element").Exists()
            .HasAttribute("name", "Target");
    }

    private class TopLevel
    {
        public IList<Item> Items { get; set; }
    }

    private class Item
    {
        public PropertyTarget Target { get; set; }
        public string SomeString { get; set; }
    }
}
