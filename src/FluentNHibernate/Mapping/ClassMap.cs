using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using ShadeTree.Core;

namespace ShadeTree.DomainModel.Mapping
{
    public class ClassMap<T> : ClassMapBase<T>, IMapping
    {
        public ClassMap()
        {
            TableName = typeof (T).Name;
        }

        public string TableName { get; set; }

        public XmlDocument CreateMapping(IMappingVisitor visitor)
        {
            visitor.CurrentType = typeof(T);
            XmlDocument document = getBaseDocument();
            setHeaderValues(document);
            XmlElement classElement = createClassValues(document, document.DocumentElement);

            writeTheParts(classElement, visitor);

            return document;
        }

        public void UseIdentityForKey(Expression<Func<T, object>> expression, string columnName)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            var part = new IdentityPart(property, columnName);

            _properties.Insert(0, part);
        }

        protected virtual XmlElement createClassValues(XmlDocument document, XmlNode parentNode)
        {
            return parentNode.AddElement("class")
                .WithAtt("name", typeof (T).Name)
                .WithAtt("table", TableName)
                .WithAtt("xmlns", "urn:nhibernate-mapping-2.2");
        }

        private void setHeaderValues(XmlDocument document)
        {
            document.DocumentElement.SetAttribute("assembly", typeof (T).Assembly.GetName().Name);
            document.DocumentElement.SetAttribute("namespace", typeof (T).Namespace);
        }

        private static XmlDocument getBaseDocument()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Stream stream = executingAssembly.GetManifestResourceStream(executingAssembly.GetName().Name + ".Mapping.Template.xml");
            var document = new XmlDocument();
            document.Load(stream);
            return document;
        }

        public void ApplyMappings(IMappingVisitor visitor)
        {
            XmlDocument mapping = null;

            try
            {
                visitor.CurrentType = typeof (T);
                mapping = CreateMapping(visitor);
                visitor.AddMappingDocument(mapping, typeof (T));
            }
            catch (Exception e)
            {
                if (mapping != null)
                {
                    var writer = new StringWriter();
                    var xmlWriter = new XmlTextWriter(writer);
                    xmlWriter.Formatting = Formatting.Indented;

                    mapping.WriteContentTo(xmlWriter);
                    Debug.WriteLine(writer.ToString());
                }

                string message = string.Format("Error while trying to build the Mapping Document for '{0}'",
                                               typeof (T).FullName);
                throw new ApplicationException(message, e);

                
            }
        }
    }
}