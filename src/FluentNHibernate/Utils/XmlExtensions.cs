using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FluentNHibernate.Utils
{
    public static class XmlExtensions
    {
        public static XmlElement AddElement(this XmlDocument document, string name)
        {
            var child = document.CreateElement(name);

            document.AppendChild(child);

            return child;
        }

        public static XmlElement AddElement(this XmlNode element, string name)
        {
            XmlElement child = element.OwnerDocument.CreateElement(name);
            element.AppendChild(child);

            return child;
        }

        public static XmlElement WithAtt(this XmlElement element, string key, bool value)
        {
            return WithAtt(element, key, value.ToString().ToLowerInvariant());
        }

        public static XmlElement WithAtt(this XmlElement element, string key, int value)
        {
            return WithAtt(element, key, value.ToString());
        }

        public static XmlElement WithAtt(this XmlElement element, string key, string attValue)
        {
            element.SetAttribute(key, attValue);
            return element;
        }

        public static void SetAttributeOnChild(this XmlElement element, string childName, string attName, string attValue)
        {
            XmlElement childElement = element[childName];
            if (childElement == null)
            {
                childElement = element.AddElement(childName);
            }

            childElement.SetAttribute(attName, attValue);
        }

        public static XmlElement WithProperties(this XmlElement element, Dictionary<string, string> properties)
        {
            foreach (var pair in properties)
            {
                element.SetAttribute(pair.Key, pair.Value);
            }

            return element;
        }

        public static XmlElement WithProperties(this XmlElement element, Cache<string, string> properties)
        {
            properties.ForEachPair((k, v) => element.SetAttribute(k, v));

            return element;
        }

        public static XmlElement SetColumnProperty(this XmlElement element, string name, string value)
        {
            XmlElement columnElement = element["column"] ?? element.AddElement("column");
            columnElement.WithAtt(name, value);

            return element;
        }

        public static void ImportAndAppendChild(this XmlDocument document, XmlDocument toImport)
        {
            var importedNode = document.ImportNode(toImport.DocumentElement, true);

            document.DocumentElement.AppendChild(importedNode);
        }
    }
}