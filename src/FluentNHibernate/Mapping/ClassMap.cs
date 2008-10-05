using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

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
            TableName = String.Format("[{0}]", typeof (T).Name);
        }

        public string TableName { get; private set; }

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

            AddPart(part);
        }

		public CompositeIdentityPart<T> UseCompositeId()
		{
			var part = new CompositeIdentityPart<T>();
			
            AddPart(part);

			return part;
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

        public virtual IdentityPart Id(Expression<Func<T, object>> expression)
		{
			return Id(expression, null);
		}

        public virtual IdentityPart Id(Expression<Func<T, object>> expression, string column)
    	{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);
    		var id = column == null ? new IdentityPart(property) : new IdentityPart(property, column);
    		AddPart(id);
    		return id;
    	}

        public virtual JoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(string keyColumn, Action<JoinedSubClassPart<TSubclass>> action) where TSubclass : T
        {
            var subclass = new JoinedSubClassPart<TSubclass>(keyColumn);

            action(subclass);
            AddPart(subclass);

            return subclass;
        }

        public void SchemaIs(string schema)
        {
            SetHibernateMappingAttribute("schema", schema);
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

        /// <summary>
        /// Sets the table for the class.
        /// </summary>
        /// <param name="tableName">Table name</param>
        public void WithTable(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// Sets additional tables for the class via the NH 2.0 Join element.
        /// </summary>
        /// <param name="tableName">Joined table name</param>
        /// <param name="action">Joined table mapping</param>
        public void WithTable(string tableName, Action<JoinPart<T>> action)
        {
            var join = new JoinPart<T>(tableName);

            action(join);
            AddPart(join);
        }
    }
}
