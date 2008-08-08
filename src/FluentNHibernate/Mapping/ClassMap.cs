using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using ShadeTree.Core;

namespace FluentNHibernate.Mapping
{
    public class ClassMap<T> : ClassMapBase<T>, IMapping, IHasAttributes
    {
        private readonly Cache<string, string> attributes = new Cache<string, string>();
        private readonly Cache<string, string> hibernateMappingAttributes = new Cache<string, string>();
        private readonly AccessStrategyBuilder<ClassMap<T>> defaultAccess;

        public ClassMap()
        {
            defaultAccess = new DefaultAccessStrategyBuilder<T>(this);
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
                .WithAtt("xmlns", "urn:nhibernate-mapping-2.2")
                .WithProperties(attributes);
        }

        private void setHeaderValues(XmlDocument document)
        {
            document.DocumentElement.SetAttribute("assembly", typeof (T).Assembly.GetName().Name);
            document.DocumentElement.SetAttribute("namespace", typeof (T).Namespace);

            hibernateMappingAttributes.ForEachPair((name,value) => document.DocumentElement.SetAttribute(name, value));
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

        /// <summary>
        /// Set an attribute on the xml element produced by this class mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            attributes.Store(name, value);
        }

        public void SetHibernateMappingAttribute(string name, string value)
        {
            hibernateMappingAttributes.Store(name, value);
        }

        public IdentityPart Id(Expression<Func<T, object>> expression)
		{
			return Id(expression, null);
		}

        public IdentityPart Id(Expression<Func<T, object>> expression, string column)
    	{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);
    		var id = column == null ? new IdentityPart(property) : new IdentityPart(property, column);
    		_properties.Insert(0, id);
    		return id;
    	}

        public string FileName
        {
            get { return string.Format("{0}.hbm.xml", typeof (T).Name); }
        }

        /// <summary>
        /// Set the default access and naming strategies for this entire mapping.
        /// </summary>
        public AccessStrategyBuilder<ClassMap<T>> DefaultAccess
        {
            get { return defaultAccess; }
        }
    }
}
