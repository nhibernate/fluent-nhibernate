using System;
using System.Xml;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    public class MappingTester<T>
    {
        protected XmlElement currentElement;
        protected XmlDocument document;
        protected IMappingVisitor _visitor = new MappingVisitor();

        public MappingTester<T> RootElement
        {
            get
            {
                currentElement = document.DocumentElement;
                return this;
            }
        }

        public MappingTester<T> UsingVisitor(IMappingVisitor visitor)
        {
            _visitor = visitor;
            return this;
        }

        public MappingTester<T> ForMapping(Action<ClassMap<T>> mappingAction)
        {
            var classMap = new ClassMap<T>();
            mappingAction(classMap);

            return ForMapping(classMap);
        }

        public MappingTester<T> ForMapping(ClassMap<T> classMap)
        {
            document = classMap.CreateMapping(_visitor);
            currentElement = document.DocumentElement;

            return this;
        }

        public MappingTester<T> Element(string elementPath)
        {
            currentElement = (XmlElement)document.DocumentElement.SelectSingleNode(elementPath);

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

        public MappingTester<T> Exists()
        {
            Assert.IsNotNull(currentElement);

            return this;
        }

        public MappingTester<T> DoesntExist()
        {
            Assert.IsNull(currentElement);

            return this;
        }

        public MappingTester<T> HasName(string name)
        {
            Assert.AreEqual(name, currentElement.Name, "Expected current element to have the name '" + name + "' but found '" + currentElement.Name + "'.");

            return this;
        }
        
        public void OutputToConsole()
        {
        	var stringWriter = new System.IO.StringWriter();
        	var xmlWriter = new XmlTextWriter(stringWriter);
        	xmlWriter.Formatting = Formatting.Indented;        	
        	this.document.WriteContentTo(xmlWriter);       
        	
        	Console.WriteLine(string.Empty);
        	Console.WriteLine(stringWriter.ToString());
        	Console.WriteLine(string.Empty);
        }
    }
}
