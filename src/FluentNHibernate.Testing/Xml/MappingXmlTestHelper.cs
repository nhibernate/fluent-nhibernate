using System;
using System.Xml;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Xml;

public class MappingXmlTestHelper
{
    protected XmlElement _currentElement;
    protected XmlDocument _document;

    public MappingXmlTestHelper(XmlDocument document)
    {
        _document = document;
        _currentElement = _document.DocumentElement;
    }

    public virtual MappingXmlTestHelper RootElement
    {
        get
        {
            _currentElement = _document.DocumentElement;
            return this;
        }
    }

    public virtual MappingXmlTestHelper Element(string element)
    {
        _currentElement = (XmlElement)_document.DocumentElement.SelectSingleNode(element);

        return this;
    }

    public virtual MappingXmlTestHelper HasAttribute(string name, string value)
    {
        _currentElement.AttributeShouldEqual(name, value);

        return this;
    }

    public virtual MappingXmlTestHelper HasInnerXml(string content)
    {
        _currentElement.InnerXml.ShouldEqual(content);

        return this;
    }

    public virtual MappingXmlTestHelper DoesntHaveAttribute(string name)
    {
        Assert.That(_currentElement.HasAttribute(name), Is.False, $"Found attribute '{name}' on element.");

        return this;
    }

    public virtual MappingXmlTestHelper Exists()
    {
        Assert.That(_currentElement, Is.Not.Null);

        return this;
    }

    public virtual MappingXmlTestHelper DoesntExist()
    {
        Assert.That(_currentElement, Is.Null);

        return this;
    }

    public virtual MappingXmlTestHelper HasName(string name)
    {
        Assert.That(_currentElement.Name, Is.EqualTo(name), $"Expected current element to have the name '{name}' but found '{_currentElement.Name}'.");

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
        this._document.WriteContentTo(xmlWriter);
        return stringWriter.ToString();
    }

    public MappingXmlTestHelper ChildrenDontContainAttribute(string key, string value)
    {
        foreach (XmlElement node in _currentElement.ChildNodes)
        {
            if (node.HasAttribute(key))
                Assert.That(value, Is.Not.EqualTo(node.Attributes[key].Value));
        }
        return this;
    }

    public MappingXmlTestHelper ValueEquals(string value)
    {
        Assert.That(_currentElement.InnerXml, Is.EqualTo(value));

        return this;
    }

    public MappingXmlTestHelper IsFirst()
    {
        Assert.That(_currentElement.ParentNode.FirstChild.OuterXml, Is.EqualTo(_currentElement.OuterXml));
        return this;
    }
}
