using System;
using System.Linq;
using System.Xml;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

public class MappingTester<T>
{
    protected XmlElement currentElement;
    protected XmlDocument document;
    private readonly PersistenceModel model;
    private string currentPath;

    public MappingTester(): this(new PersistenceModel())
    {}

    public MappingTester(PersistenceModel model)
    {
        this.model = model;
        this.model.ValidationEnabled = false;
    }

    public virtual MappingTester<T> RootElement
    {
        get
        {
            currentElement = document.DocumentElement;
            return this;
        }
    }

    public virtual MappingTester<T> Conventions(Action<IConventionFinder> conventionFinderAction)
    {
        conventionFinderAction(model.Conventions);
        return this;
    }

    public virtual MappingTester<T> SubClassMapping<TSubClass>(Action<SubclassMap<TSubClass>> action) where TSubClass : T
    {
        var map = new SubclassMap<TSubClass>();
        action(map);

        this.model.Add(map);

        return this;
    }

    public virtual MappingTester<T> ForMapping(Action<ClassMap<T>> mappingAction)
    {
        var classMap = new ClassMap<T>();
        mappingAction(classMap);

        return ForMapping(classMap);
    }

    public virtual MappingTester<T> ForMapping(ClassMap<T> classMap)
    {
        if (classMap  is not null)
            model.Add(classMap);

        var mappings = model.BuildMappings();
        var foundMapping = mappings.FirstOrDefault(x => x.Classes.FirstOrDefault(c => c.Type == typeof(T)) is not null);

        if (foundMapping is null)
            throw new InvalidOperationException("Could not find mapping for class '" + typeof(T).Name + "'");

        document = new MappingXmlSerializer()
            .Serialize(foundMapping);
        currentElement = document.DocumentElement;

        return this;
    }

    public virtual MappingTester<T> Element(string elementPath)
    {
        currentElement = (XmlElement)document.DocumentElement.SelectSingleNode(elementPath);
        currentPath = elementPath;

        return this;
    }

    public virtual MappingTester<T> HasThisManyChildNodes(int expected)
    {
        currentElement.ChildNodeCountShouldEqual(expected);

        return this;
    }

    public virtual MappingTester<T> HasAttribute(string name, string value)
    {
        Assert.That(currentElement, Is.Not.Null, $"Couldn't find element matching '{currentPath}'");

        var actual = currentElement.GetAttribute(name);

        Assert.That(actual, Is.EqualTo(value), $"Attribute '{name}' of '{currentPath}' didn't match.");

        return this;
    }

    public virtual MappingTester<T> HasAttribute(string name, Func<string, bool> predicate)
    {
        Assert.That(currentElement, Is.Not.Null, $"Couldn't find element matching '{currentPath}'");

        currentElement.HasAttribute(name).ShouldBeTrue();

        predicate(currentElement.Attributes[name].Value).ShouldBeTrue();

        return this;
    }

    public virtual MappingTester<T> DoesntHaveAttribute(string name)
    {
        Assert.That(currentElement.HasAttribute(name), Is.False, $"Found attribute '{name}' on element.");

        return this;
    }

    public virtual MappingTester<T> Exists()
    {
        Assert.That(currentElement, Is.Not.Null, $"Couldn't find element matching '{currentPath}'");

        return this;
    }

    public virtual MappingTester<T> DoesntExist()
    {
        Assert.That(currentElement, Is.Null);

        return this;
    }

    public virtual MappingTester<T> HasName(string name)
    {
        Assert.That(currentElement.Name, Is.EqualTo(name), $"Expected current element to have the name '{name}' but found '{currentElement.Name}'.");

        return this;
    }

    public virtual void OutputToConsole()
    {
        Console.WriteLine(string.Empty);
        Console.WriteLine(this.ToString());
        Console.WriteLine(string.Empty);
    }

    public override string ToString()
    {
        var stringWriter = new System.IO.StringWriter();
        var xmlWriter = new XmlTextWriter(stringWriter);
        xmlWriter.Formatting = Formatting.Indented;
        this.document.WriteContentTo(xmlWriter);
        return stringWriter.ToString();
    }

    public MappingTester<T> ChildrenDontContainAttribute(string key, string value)
    {
        Assert.That(currentElement, Is.Not.Null, $"Couldn't find element matching '{currentPath}'");

        foreach (XmlElement node in currentElement.ChildNodes)
        {
            if (node.HasAttribute(key))
                Assert.That(value, Is.Not.EqualTo(node.Attributes[key].Value));
        }
        return this;
    }

    public MappingTester<T> ValueEquals(string value)
    {
        currentElement.InnerXml.ShouldEqual(value);

        return this;
    }

    /// <summary>
    /// Determines if the CurrentElement is located at a specified element position in it's parent
    /// </summary>
    /// <param name="elementPosition">Zero based index of elements on the parent</param>
    public virtual MappingTester<T> ShouldBeInParentAtPosition(int elementPosition)
    {
        XmlElement parentElement = (XmlElement)currentElement.ParentNode;
        if (parentElement is null)
        {
            Assert.Fail("Current element has no parent element.");
        }
        else
        {
            XmlElement elementAtPosition = (XmlElement)currentElement.ParentNode.ChildNodes.Item(elementPosition);
            Assert.That(elementAtPosition, Is.EqualTo(currentElement), $"Expected '{currentElement.Name}' but was '{elementAtPosition.Name}'");
        }

        return this;
    }
}
