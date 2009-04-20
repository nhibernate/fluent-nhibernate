using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IClassMap : IClasslike, IHasAttributes
    {
        void ApplyMappings(IMappingVisitor visitor);
        XmlDocument CreateMapping(IMappingVisitor visitor);
        Type EntityType { get; }
        string TableName { get; }
        ICache Cache { get; }
        Cache<string, string> Attributes { get; }
        Cache<string, string> HibernateMappingAttributes { get; }
        /// <summary>
        /// Sets the optimistic locking strategy
        /// </summary>
        OptimisticLockBuilder OptimisticLock { get; }

        /// <summary>
        /// Sets the table for the class.
        /// </summary>
        /// <param name="tableName">Table name</param>
        void WithTable(string tableName);

        void SetHibernateMappingAttribute(string name, string value);
        void SetHibernateMappingAttribute(string name, bool value);

        /// <summary>
        /// Sets the hibernate-mapping schema for this class.
        /// </summary>
        /// <param name="schema">Schema name</param>
        void SchemaIs(string schema);

        /// <summary>
        /// Sets the hibernate-mapping auto-import for this class.
        /// </summary>
        void AutoImport();

        /// <summary>
        /// Override the inferred assembly for this class
        /// </summary>
        /// <param name="assembly">Assembly to use</param>
        void OverrideAssembly(Assembly assembly);

        /// <summary>
        /// Override the inferred assembly for this class
        /// </summary>
        /// <param name="assembly">Assembly to use</param>
        void OverrideAssembly(string assembly);

        /// <summary>
        /// Override the inferred namespace for this class
        /// </summary>
        /// <param name="namespace">Namespace to use</param>
        void OverrideNamespace(string @namespace);

        /// <summary>
        /// Sets this entity to be lazy-loaded (overrides the default lazy load configuration).
        /// </summary>
        void LazyLoad();

        /// <summary>
        /// Imports an existing type for use in the mapping.
        /// </summary>
        /// <typeparam name="TImport">Type to import.</typeparam>
        ImportPart ImportType<TImport>();

        /// <summary>
        /// Set the mutability of this class, sets the mutable attribute.
        /// </summary>
        void ReadOnly();

        /// <summary>
        /// Sets this entity to be dynamic update
        /// </summary>
        void DynamicUpdate();

        /// <summary>
        /// Sets this entity to be dynamic insert
        /// </summary>
        void DynamicInsert();

        IClassMap BatchSize(int size);

        /// <summary>
        /// Inverse next boolean
        /// </summary>
        IClassMap Not { get; }
    }

    public class ClassMap<T> : ClasslikeMapBase<T>, IClassMap
    {
        public Cache<string, string> Attributes { get; private set; }
        public Cache<string, string> HibernateMappingAttributes { get; private set; }
        private readonly AccessStrategyBuilder<ClassMap<T>> defaultAccess;

        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        public ICache Cache
        {
            get
            {
                return m_Parts.Where(x => x.GetType() == typeof(CachePart)).FirstOrDefault() as ICache;
            }
            private set
            {
                if (Cache != null)
                { m_Parts.Remove(Cache); }

                AddPart(value);
            }
        }
        
        private readonly IList<ImportPart> imports = new List<ImportPart>();
        private string assemblyName;
        private string namespaceName;
        private bool nextBool = true;
        private int batchSize;

        public ClassMap()
        {
            Attributes = new Cache<string, string>();
            HibernateMappingAttributes = new Cache<string, string>();
            defaultAccess = new DefaultAccessStrategyBuilder<T>(this);
            Cache = new CachePart();
        }

        public string TableName { get; private set; }

        public XmlDocument CreateMapping(IMappingVisitor visitor)
        {
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

        public Type EntityType
        {
            get { return typeof(T); }
        }

        public void UseIdentityForKey(Expression<Func<T, object>> expression, string columnName)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            var part = new IdentityPart(EntityType, property, columnName);

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
            var type = typeof(T);
            var typeName = type.IsGenericType ? type.FullName : type.Name;

            var classElement = parentNode.AddElement("class");

            classElement.WithAtt("name", typeName)
                .WithAtt("table", TableName)
                .WithAtt("xmlns", "urn:nhibernate-mapping-2.2");

            if (batchSize > 0)
                classElement.WithAtt("batch-size", batchSize.ToString());

            classElement.WithProperties(Attributes);

            return classElement;
        }

        private void setHeaderValues(IMappingVisitor visitor, XmlDocument document)
        {
            var documentElement = document.DocumentElement;

            documentElement.SetAttribute("assembly", assemblyName ?? typeof(T).Assembly.GetName().FullName);
            documentElement.SetAttribute("namespace", namespaceName ?? typeof (T).Namespace);

            HibernateMappingAttributes.ForEachPair(documentElement.SetAttribute);
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
            Attributes.Store(name, value);
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
            HibernateMappingAttributes.Store(name, value);
        }

        public void SetHibernateMappingAttribute(string name, bool value)
        {
            HibernateMappingAttributes.Store(name, value.ToString().ToLowerInvariant());
        }

        public virtual IIdentityPart Id(Expression<Func<T, object>> expression)
		{
			return Id(expression, null);
		}

        public virtual IIdentityPart Id(Expression<Func<T, object>> expression, string column)
    	{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);
            var id = column == null ? new IdentityPart(EntityType, property) : new IdentityPart(EntityType, property, column);
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

        IClassMap IClassMap.Not
        {
            get { return Not; }
        }

        /// <summary>
        /// Sets this entity to be lazy-loaded (overrides the default lazy load configuration).
        /// </summary>
        public void LazyLoad()
        {
            Attributes.Store("lazy", nextBool.ToString().ToLowerInvariant());
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
            Attributes.Store("mutable", (!nextBool).ToString().ToLowerInvariant());
            nextBool = true;
        }

        /// <summary>
        /// Sets this entity to be dynamic update
        /// </summary>
        public void DynamicUpdate()
        {
            Attributes.Store("dynamic-update", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
        }

        /// <summary>
        /// Sets this entity to be dynamic insert
        /// </summary>
        public void DynamicInsert()
        {
            Attributes.Store("dynamic-insert", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
        }

        public IClassMap BatchSize(int size)
        {
            batchSize = size;
            return this;
        }

        /// <summary>
        /// Sets the optimistic locking strategy
        /// </summary>
        public OptimisticLockBuilder OptimisticLock
        {
            get { return new OptimisticLockBuilder(Attributes); }
        }
    }
}
