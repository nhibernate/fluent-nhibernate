using System;
using System.Linq;
using System.Xml;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    public static class AutomappingSpecExtensions
    {
        public static ClassMapping BuildMappingFor<T>(this AutoPersistenceModel model)
        {
            return model.BuildMappings()
                .SelectMany(x => x.Classes)
                .FirstOrDefault(x => x.Type == typeof(T));
        }

        public static XmlDocument ToXml(this ClassMapping mapping)
        {
            var hbm = new HibernateMapping();

            hbm.AddClass(mapping);

            return new MappingXmlSerializer()
                .Serialize(hbm);
        }
    }

    public static class XmlTestExtensions
    {
        public static XmlElementTester Element(this XmlDocument doc, string path)
        {
            return new XmlElementTester(doc, path);
        }

        public class XmlElementTester
        {
            readonly XmlDocument doc;
            string currentPath;
            XmlElement currentElement;

            public XmlElementTester(XmlDocument doc, string path)
            {
                currentElement = (XmlElement)doc.DocumentElement.SelectSingleNode(path);
                this.doc = doc;
                currentPath = path;
            }

            public XmlElementTester ShouldExist()
            {
                if (currentElement == null)
                    throw new SpecificationException(string.Format("Should exist at {0} but does not.", currentPath));

                return this;
            }

            public XmlElementTester ShouldNotExist()
            {
                if (currentElement != null)
                    throw new SpecificationException(string.Format("Should not exist at {0} but does.", currentPath));

                return this;
            }

            public XmlElementTester HasAttribute(string name)
            {
                if (!currentElement.HasAttribute(name))
                    throw new SpecificationException(string.Format("Should have attribute named {0} at {1} but does not.", name, currentPath));

                return this;
            }

            public XmlElementTester HasAttribute(string name, string value)
            {
                ShouldExist();
                HasAttribute(name);

                var actual = currentElement.GetAttribute(name);

                if (!actual.Equals(value))
                    throw new SpecificationException(string.Format("Should have attribute named {0} at {1} with value of {2} but was {3}", name, currentPath, value, actual));

                return this;
            }

            public XmlElementTester HasAttribute(string name, Func<string, bool> predicate)
            {
                ShouldExist();
                HasAttribute(name);

                var actual = currentElement.GetAttribute(name);

                if (!predicate(actual))
                    throw new SpecificationException(string.Format("Should have attribute named {0} at {1} with value matching predicate but does not.", name, currentPath));

                return this;
            }

            public XmlElementTester DoesntHaveAttribute(string name)
            {
                ShouldExist();

                if (currentElement.HasAttribute(name))
                    throw new SpecificationException(string.Format("Should not have attribute named {0} at {1} but does.", name, currentPath));

                return this;
            }
        }
    }
}