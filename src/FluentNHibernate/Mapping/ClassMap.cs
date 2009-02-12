using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IClassMap : IMapping
    {
        XmlDocument CreateMapping(IMappingVisitor visitor);
    }

    public class ClassMap<T> : ClassMapBase<T>, IClassMap, IHasAttributes
    {
        public const string DefaultLazyAttributeKey = "default-lazy";
        private readonly Cache<string, string> attributes = new Cache<string, string>();
        private readonly Cache<string, string> hibernateMappingAttributes = new Cache<string, string>();
        private readonly AccessStrategyBuilder<ClassMap<T>> defaultAccess;
        private CachePart cache;
        private readonly IList<ImportPart> imports = new List<ImportPart>();
        private string assemblyName;
        private string namespaceName;
        private bool nextBool = true;

        public ClassMap()
        {
            defaultAccess = new DefaultAccessStrategyBuilder<T>(this);
        }

        public string TableName { get; private set; }

        public XmlDocument CreateMapping(IMappingVisitor visitor)
        {
            PrepareBeforeCreateMapping(visitor);

            visitor.CurrentType = typeof(T);
            XmlDocument document = getBaseDocument();
            setHeaderValues(visitor, document);

            foreach (var import in imports)
            {
                import.Write(document.DocumentElement, visitor);
            }

            XmlElement classElement = createClassValues(document, document.DocumentElement);

            writeTheParts(classElement, visitor);

            return document;
        }

        private void PrepareBeforeCreateMapping(IMappingVisitor visitor)
        {
            if (cache == null)
                cache = visitor.Conventions.DefaultCache(new CachePart());

            if (cache != null)
                AddPart(cache);

            if (String.IsNullOrEmpty(TableName))
                TableName = visitor.Conventions.GetTableName.Invoke(typeof(T));

            PrepareDynamicUpdate(visitor);
            PrepareDynamicInsert(visitor);
            PrepareOptimisticLock(visitor);
        }

        private void PrepareDynamicUpdate(IMappingVisitor visitor)
        {
            if (attributes.Has("dynamic-update")) return;

            var value = visitor.Conventions.DynamicUpdate(typeof(T));

            if (value != null)
                attributes.Store("dynamic-update", value.ToString().ToLowerInvariant());
        }

        private void PrepareDynamicInsert(IMappingVisitor visitor)
        {
            if (attributes.Has("dynamic-insert")) return;

            var value = visitor.Conventions.DynamicInsert(typeof(T));

            if (value != null)
                attributes.Store("dynamic-insert", value.ToString().ToLowerInvariant());
        }

        private void PrepareOptimisticLock(IMappingVisitor visitor)
        {
            if (attributes.Has("optimistic-lock")) return;

            visitor.Conventions.OptimisticLock(typeof(T), OptimisticLock);
        }

        public void UseIdentityForKey(Expression<Func<T, object>> expression, string columnName)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            var part = new IdentityPart<T>(property, columnName);

            AddPart(part);
        }

		public CompositeIdentityPart<T> UseCompositeId()
		{
			var part = new CompositeIdentityPart<T>();
			
            AddPart(part);

			return part;
		}

        public virtual DiscriminatorPart<TDiscriminator, T> DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName, TDiscriminator baseClassDiscriminator)
        {
            var part = new DiscriminatorPart<TDiscriminator, T>(columnName, baseClassDiscriminator, this);
            
            AddPart(part);

            return part;
        }

        public virtual DiscriminatorPart<TDiscriminator, T> DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName)
        {
            var part = new DiscriminatorPart<TDiscriminator, T>(columnName, this);
            
            AddPart(part);

            return part;
        }

        public virtual DiscriminatorPart<string, T> DiscriminateSubClassesOnColumn(string columnName)
        {
            return DiscriminateSubClassesOnColumn<string>(columnName);
        }

        protected virtual XmlElement createClassValues(XmlDocument document, XmlNode parentNode)
        {
            return parentNode.AddElement("class")
                .WithAtt("name", typeof (T).Name)
                .WithAtt("table", TableName)
                .WithAtt("xmlns", "urn:nhibernate-mapping-2.2")
                .WithProperties(attributes);
        }

        private void setHeaderValues(IMappingVisitor visitor, XmlDocument document)
        {
            var documentElement = document.DocumentElement;

            //TODO: Law of Demeter violation here. The convention stuff smells, perhaps some double-dispatch is in order?
            var defaultLazyValue = visitor.Conventions.DefaultLazyLoad.ToString().ToLowerInvariant();
            
            if (!hibernateMappingAttributes.Has(DefaultLazyAttributeKey))
            {
                documentElement.SetAttribute(DefaultLazyAttributeKey, defaultLazyValue);
            }

            documentElement.SetAttribute("assembly", assemblyName ?? typeof(T).Assembly.GetName().Name);
            documentElement.SetAttribute("namespace", namespaceName ?? typeof (T).Namespace);

            hibernateMappingAttributes.ForEachPair(documentElement.SetAttribute);
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
        public virtual void SetAttribute(string name, string value)
        {
            attributes.Store(name, value);
        }

        public virtual void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public void SetHibernateMappingAttribute(string name, string value)
        {
            hibernateMappingAttributes.Store(name, value);
        }

        public void SetHibernateMappingAttribute(string name, bool value)
        {
            hibernateMappingAttributes.Store(name, value.ToString().ToLowerInvariant());
        }

        public virtual IIdentityPart Id(Expression<Func<T, object>> expression)
		{
			return Id(expression, null);
		}

        public virtual IIdentityPart Id(Expression<Func<T, object>> expression, string column)
    	{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);
            var id = column == null ? new IdentityPart<T>(property) : new IdentityPart<T>(property, column);
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

        /// <summary>
        /// Sets the hibernate-mapping schema for this class.
        /// </summary>
        /// <param name="schema">Schema name</param>
        public void SchemaIs(string schema)
        {
            SetHibernateMappingAttribute("schema", schema);
        }

        /// <summary>
        /// Sets the hibernate-mapping auto-import for this class.
        /// </summary>
        public void AutoImport()
        {
            SetHibernateMappingAttribute("auto-import", nextBool);
            nextBool = true;
        }

        /// <summary>
        /// Override the inferred assembly for this class
        /// </summary>
        /// <param name="assembly">Assembly to use</param>
        public void OverrideAssembly(Assembly assembly)
        {
            assemblyName = assembly.GetName().Name;
        }

        /// <summary>
        /// Override the inferred assembly for this class
        /// </summary>
        /// <param name="assembly">Assembly to use</param>
        public void OverrideAssembly(string assembly)
        {
            assemblyName = assembly;
        }

        /// <summary>
        /// Override the inferred namespace for this class
        /// </summary>
        /// <param name="namespace">Namespace to use</param>
        public void OverrideNamespace(string @namespace)
        {
            namespaceName = @namespace;
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
        /// Specify caching for this entity.
        /// </summary>
        public CachePart Cache
        {
            get
            {
                cache = new CachePart();

                return cache;
            }
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
        /// Inverse next boolean
        /// </summary>
        public ClassMap<T> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        /// <summary>
        /// Sets this entity to be lazy-loaded (overrides the default lazy load configuration).
        /// </summary>
        public void LazyLoad()
        {
            attributes.Store("lazy", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
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

        /// <summary>
        /// Imports an existing type for use in the mapping.
        /// </summary>
        /// <typeparam name="TImport">Type to import.</typeparam>
        public ImportPart ImportType<TImport>()
        {
            var part = new ImportPart(typeof(TImport));
            
            imports.Add(part);

            return part;
        }

        /// <summary>
        /// Set the mutability of this class, sets the mutable attribute.
        /// </summary>
        public void ReadOnly()
        {
            attributes.Store("mutable", (!nextBool).ToString().ToLowerInvariant());
            nextBool = true;
        }

        /// <summary>
        /// Sets this entity to be dynamic update
        /// </summary>
        public void DynamicUpdate()
        {
            attributes.Store("dynamic-update", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
        }

        /// <summary>
        /// Sets this entity to be dynamic insert
        /// </summary>
        public void DynamicInsert()
        {
            attributes.Store("dynamic-insert", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
        }

        /// <summary>
        /// Sets the optimistic locking strategy
        /// </summary>
        public OptimisticLock OptimisticLock
        {
            get { return new OptimisticLock(attributes); }
        }
    }
}
