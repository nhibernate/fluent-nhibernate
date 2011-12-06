using System;
using System.IO;
using System.Linq;
using System.Xml;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    /// <summary>
    /// Provides testing facilities for mapped entities within a <see cref="PersistenceModel"/>.
    /// </summary>
    /// <typeparam name="T">The entity which mapping you want to test.</typeparam>
    public class MappingTester<T>
    {
        readonly PersistenceModel model;
        XmlElement currentElement;
        string currentPath;
        XmlDocument document;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingTester&lt;T&gt;"/> class.
        /// </summary>
        public MappingTester()
            : this(new PersistenceModel())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingTester&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public MappingTester(PersistenceModel model)
        {
            this.model = model;
            this.model.ValidationEnabled = false;
        }

        /// <summary>
        /// Gets the root element.
        /// </summary>
        public virtual MappingTester<T> RootElement
        {
            get
            {
                currentElement = document.DocumentElement;
                return this;
            }
        }

        /// <summary>
        /// Applies the specified conventions.
        /// </summary>
        /// <param name="conventionFinderAction">The convention finder action.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> Conventions(Action<IConventionFinder> conventionFinderAction)
        {
            conventionFinderAction(model.Conventions);
            return this;
        }

        /// <summary>
        /// Adds the subclass mapping.
        /// </summary>
        /// <typeparam name="TSubClass">The type of the subclass.</typeparam>
        /// <param name="action">The action that returns the subclass mapping.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> SubClassMapping<TSubClass>(Action<SubclassMap<TSubClass>> action)
            where TSubClass : T
        {
            var map = new SubclassMap<TSubClass>();
            action(map);

            model.Add(map);

            return this;
        }

        /// <summary>
        /// Sets up the <see cref="MappingTester{T}"/> to provide mapping for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="mappingAction">The mapping action.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> ForMapping(Action<ClassMap<T>> mappingAction)
        {
            var classMap = new ClassMap<T>();
            mappingAction(classMap);

            return ForMapping(classMap);
        }

        /// <summary>
        /// Sets up the <see cref="MappingTester{T}"/> to provide mapping for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="classMap">The class map to set up.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> ForMapping(ClassMap<T> classMap)
        {
            if (classMap != null)
                model.Add(classMap);

            var mappings = model.BuildMappings();
            var foundMapping = mappings
                .Where(x => x.Classes.FirstOrDefault(c => c.Type == typeof(T)) != null)
                .FirstOrDefault();

            if (foundMapping == null)
                throw new InvalidOperationException("Could not find mapping for class '" + typeof(T).Name + "'");

            document = new MappingXmlSerializer()
                .Serialize(foundMapping);
            currentElement = document.DocumentElement;

            return this;
        }

        /// <summary>
        /// Selects the element found at the specified XPath expression.
        /// </summary>
        /// <param name="elementPath">The XPath expression for the requested element.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> Element(string elementPath)
        {
            currentElement = (XmlElement)document.DocumentElement.SelectSingleNode(elementPath);
            currentPath = elementPath;

            return this;
        }

        /// <summary>
        /// Asserts that the currently selected element (as selected with <see cref="Element"/>)
        /// has the specified number of child nodes.
        /// </summary>
        /// <param name="expected">The expected number of child nodes.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> HasThisManyChildNodes(int expected)
        {
            currentElement.ChildNodeCountShouldEqual(expected);

            return this;
        }

        /// <summary>
        /// Asserts that the currently selected element (as selected with <see cref="Element"/>)
        /// has an attribute with the specified <paramref name="name"/> and given <paramref name="value"/>.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> HasAttribute(string name, string value)
        {
            Assert.IsNotNull(currentElement, "Couldn't find element matching '" + currentPath + "'");

            var actual = currentElement.GetAttribute(name);

            Assert.AreEqual(value, actual,
                "Attribute '" + name + "' of '" + currentPath + "' didn't match.");

            return this;
        }

        /// <summary>
        /// Asserts that the currently selected element (as selected with <see cref="Element"/>)
        /// has an attribute with the specified <paramref name="name"/> and fulfills the given
        /// <paramref name="predicate"/>.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="predicate">The predicate the attribute needs to fullfill.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> HasAttribute(string name, Func<string, bool> predicate)
        {
            Assert.IsNotNull(currentElement, "Couldn't find element matching '" + currentPath + "'");

            currentElement.HasAttribute(name).ShouldBeTrue();

            predicate(currentElement.Attributes[name].Value).ShouldBeTrue();

            return this;
        }

        /// <summary>
        /// Asserts that the currently selected element (as selected with <see cref="Element"/>)
        /// does not have an attribute with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> DoesntHaveAttribute(string name)
        {
            Assert.IsFalse(currentElement.HasAttribute(name), "Found attribute '" + name + "' on element.");

            return this;
        }

        /// <summary>
        /// Asserts that the currently selected element (as selected with <see cref="Element"/>) exists.
        /// </summary>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> Exists()
        {
            Assert.IsNotNull(currentElement, "Couldn't find element matching '" + currentPath + "'");

            return this;
        }

        /// <summary>
        /// Asserts that the currently selected element (as selected with <see cref="Element"/>) doesn't exists.
        /// </summary>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> DoesntExist()
        {
            Assert.IsNull(currentElement);

            return this;
        }

        /// <summary>
        /// Asserts that the currently selected element (as selected with <see cref="Element"/>)
        /// has the specified <paramref name="name"/> or not.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> HasName(string name)
        {
            Assert.AreEqual(name, currentElement.Name, "Expected current element to have the name '" + name + "' but found '" + currentElement.Name + "'.");

            return this;
        }

        /// <summary>
        /// Outputs the return from <see cref="ToString"/> to <see cref="System.Console.Out"/>.
        /// </summary>
        public virtual void OutputToConsole()
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine(ToString());
            Console.WriteLine(string.Empty);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented})
            {
                document.WriteContentTo(xmlWriter);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Asserts that the children of the currently selected element (as selected with <see cref="Element"/>)
        /// don't contain an attribute with the specified <paramref name="name"/> and given <paramref name="value"/>.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public MappingTester<T> ChildrenDontContainAttribute(string name, string value)
        {
            Assert.IsNotNull(currentElement, "Couldn't find element matching '" + currentPath + "'");

            foreach (XmlElement node in currentElement.ChildNodes)
            {
                if (node.HasAttribute(name))
                    Assert.AreNotEqual(node.Attributes[name].Value, value);
            }
            return this;
        }

        /// <summary>
        /// Asserts that the currently selected element's (as selected with <see cref="Element"/>) content
        /// is equal to the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public MappingTester<T> ValueEquals(string value)
        {
            currentElement.InnerXml.ShouldEqual(value);

            return this;
        }

        /// <summary>
        /// Asserts that the currently selected element (as selected with <see cref="Element"/>) is located
        /// at a specified element position in its parent.
        /// </summary>
        /// <param name="elementPosition">Zero based index of elements on the parent.</param>
        /// <returns>
        /// The current instance of <see cref="MappingTester{T}"/>.
        /// </returns>
        public virtual MappingTester<T> ShouldBeInParentAtPosition(int elementPosition)
        {
            XmlElement parentElement = (XmlElement)currentElement.ParentNode;
            if (parentElement == null)
            {
                Assert.Fail("Current element has no parent element.");
            }
            else
            {
                XmlElement elementAtPosition = (XmlElement)currentElement.ParentNode.ChildNodes.Item(elementPosition);
                Assert.IsTrue(elementAtPosition == currentElement, "Expected '" + currentElement.Name + "' but was '" + elementAtPosition.Name + "'");
            }

            return this;
        }
    }
}