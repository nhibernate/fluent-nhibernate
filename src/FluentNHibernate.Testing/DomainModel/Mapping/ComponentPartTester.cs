﻿using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

[TestFixture]
public class ComponentPartTester
{
    [Test]
    public void ComponentCanIncludeParentReference()
    {
        new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.Component(x => x.Component, c =>
                {
                    c.Map(x => x.Name);
                    c.ParentReference(x => x.MyParent);
                }))
            .Element("class/component/parent").ShouldBeInParentAtPosition(0)
            .HasAttribute("name", "MyParent");
    }

    [Test]
    public void ComponentCanSetParentReferenceAccessStrategy()
    {
        new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.Component(x => x.Component, c =>
                {
                    c.Map(x => x.Name);
                    c.ParentReference(x => x.MyParent, x => x.Access.BackingField());
                }))
            .Element("class/component/parent").ShouldBeInParentAtPosition(0)
            .HasAttribute("name", "MyParent")
            .HasAttribute("access", "backfield");
    }

    [Test]
    public void ComponentDoesntHaveUniqueAttributeByDefault()
    {
        new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.Component(x => x.Component, c =>
                {
                    c.Map(x => x.Name);
                    c.ParentReference(x => x.MyParent);
                }))
            .Element("class/component").DoesntHaveAttribute("unique");
    }

    [Test]
    public void ComponentIsGeneratedWithOnlyOnePropertyReference()
    {
        //Regression test for issue 223
        new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.Component(x => x.Component, c =>
                {
                    c.Map(x => x.Name);
                }))
            .Element("//class/component").HasThisManyChildNodes(1);
    }

    [Test]
    public void ComponentInComponent()
    {
        new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.Component(x => x.Component, c =>
                {
                    c.Component(x => x.MyParent, c2 => { });
                }))
            .Element("class/component/component").Exists();
    }

    [Test]
    public void ComponentCanSetInsert()
    {
        new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.Component(x => x.Component, c =>
                {
                    c.Insert();
                }))
            .Element("//class/component").HasAttribute("insert", "true");
    }

    [Test]
    public void ComponentCanSetUpdate()
    {
        new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.Component(x => x.Component, c =>
                {
                    c.Update();
                }))
            .Element("//class/component").HasAttribute("update", "true");
    }

    [Test]
    public void ComponentSetsClass()
    {
        new MappingTester<PropertyTarget>()
            .ForMapping(m =>
                m.Component(x => x.Component, c => c.Map(x => x.Name)))
            .Element("class/component").HasAttribute("class", typeof(ComponentTarget).AssemblyQualifiedName);                
    }        

}
