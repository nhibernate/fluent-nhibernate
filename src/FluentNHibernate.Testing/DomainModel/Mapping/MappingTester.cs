using System;
using System.Xml;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    public class MappingTester<T>
    {
        private XmlElement currentElement;
        private XmlDocument document;

        public MappingTester<T> RootElement
        {
            get
            {
                currentElement = document.DocumentElement;
                return this;
            }
        }

        public MappingTester<T> ForMapping(Action<ClassMap<T>> mapping)
        {
            var classMap = new ClassMap<T>();

            mapping(classMap);

            document = classMap.CreateMapping(new MappingVisitor());
            currentElement = document.DocumentElement;

            return this;
        }

        public MappingTester<T> Element(string elementPath)
        {
            currentElement = (XmlElement)currentElement.SelectSingleNode(elementPath);

            return this;
        }

        public MappingTester<T> HasAttribute(string name, string value)
        {
            currentElement.AttributeShouldEqual(name, value);

            return this;
        }

        public MappingTester<T> DoesntHaveAttribute(string name)
        {
            Assert.IsFalse(currentElement.HasAttribute(name), "Found attribute '" + name + "' on element.");

            return this;
        }
    }
}