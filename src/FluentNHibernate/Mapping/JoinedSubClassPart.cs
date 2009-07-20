using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class JoinedSubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, IJoinedSubclass
    {
        private string keyColumn;
        private readonly Cache<string, string> unmigratedAttributes = new Cache<string, string>();
        private readonly JoinedSubclassMapping mapping;
        private bool nextBool = true;

        public JoinedSubClassPart(string keyColumn)
            : this(new JoinedSubclassMapping())
        {
            this.keyColumn = keyColumn;
        }

        public JoinedSubClassPart(JoinedSubclassMapping mapping)
        {
            this.mapping = mapping;
        }

        public JoinedSubClassPart<TSubclass> KeyColumnName(string columnName)
        {
            keyColumn = columnName;
            return this;
        }

        protected override PropertyMap Map(PropertyInfo property, string columnName)
        {
            var propertyMapping = new PropertyMapping
            {
                Name = property.Name,
                PropertyInfo = property
            };

            var propertyMap = new PropertyMap(propertyMapping);

            if (!string.IsNullOrEmpty(columnName))
                propertyMap.ColumnName(columnName);

            properties.Add(propertyMap); // new

            return propertyMap;
        }

        public override DynamicComponentPart<IDictionary> DynamicComponent(PropertyInfo property, Action<DynamicComponentPart<IDictionary>> action)
        {
            var part = new DynamicComponentPart<IDictionary>(property);
            components.Add(part);
            action(part);

            return part;
        }

        protected override ComponentPart<TComponent> Component<TComponent>(PropertyInfo property, Action<ComponentPart<TComponent>> action)
        {
            var part = new ComponentPart<TComponent>(property);
            action(part);
            components.Add(part);

            return part;
        }

        public virtual void JoinedSubClass<TNextSubclass>(string keyColumn, Action<JoinedSubClassPart<TNextSubclass>> action)
        {
            var subclass = new JoinedSubClassPart<TNextSubclass>(keyColumn);

            action(subclass);

            joinedSubclasses.Add(subclass);

            mapping.AddSubclass(subclass.GetJoinedSubclassMapping());
        }

        public virtual void SetAttribute(string name, string value)
        {
            unmigratedAttributes.Store(name, value);
        }

        public virtual void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public JoinedSubClassPart<TSubclass> WithTableName(string tableName)
        {
            mapping.TableName = tableName;
            return this;
        }

        public JoinedSubClassPart<TSubclass> SchemaIs(string schema)
        {
            mapping.Schema = schema;
            return this;
        }

        public JoinedSubClassPart<TSubclass> CheckConstraint(string constraintName)
        {
            mapping.Check = constraintName;
            return this;
        }

        public JoinedSubClassPart<TSubclass> Proxy(Type type)
        {
            mapping.Proxy = type;
            return this;
        }

        public JoinedSubClassPart<TSubclass> Proxy<T>()
        {
            mapping.Proxy = typeof(T);
            return this;
        }

        public JoinedSubClassPart<TSubclass> LazyLoad()
        {
            mapping.Lazy = nextBool;
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> DynamicUpdate()
        {
            mapping.DynamicUpdate = nextBool;
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> DynamicInsert()
        {
            mapping.DynamicInsert = nextBool;
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> SelectBeforeUpdate()
        {
            mapping.SelectBeforeUpdate = nextBool;
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> Abstract()
        {
            mapping.Abstract = nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        public JoinedSubClassPart<TSubclass> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public JoinedSubclassMapping GetJoinedSubclassMapping()
        {
            mapping.Key = new KeyMapping
            {
                Column = keyColumn
            };
            mapping.Name = typeof(TSubclass).AssemblyQualifiedName;

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in components)
                mapping.AddComponent(component.GetComponentMapping());

            foreach (var part in Parts)
                mapping.AddUnmigratedPart(part);

            unmigratedAttributes.ForEachPair(mapping.AddUnmigratedAttribute);

            return mapping;
        }

        void IJoinedSubclass.KeyColumnName(string columnName)
        {
            KeyColumnName(columnName);
        }

        void IJoinedSubclass.WithTableName(string tableName)
        {
            WithTableName(tableName);
        }

        void IJoinedSubclass.SchemaIs(string schema)
        {
            SchemaIs(schema);
        }

        void IJoinedSubclass.CheckConstraint(string checkConstraint)
        {
            CheckConstraint(checkConstraint);
        }

        void IJoinedSubclass.Proxy(Type type)
        {
            Proxy(type);
        }

        void IJoinedSubclass.Proxy<T>()
        {
            Proxy<T>();
        }

        void IJoinedSubclass.LazyLoad()
        {
            LazyLoad();
        }

        void IJoinedSubclass.DynamicUpdate()
        {
            DynamicUpdate();
        }

        void IJoinedSubclass.DynamicInsert()
        {
            DynamicInsert();
        }

        void IJoinedSubclass.SelectBeforeUpdate()
        {
            SelectBeforeUpdate();
        }

        void IJoinedSubclass.Abstract()
        {
            Abstract();
        }

        IJoinedSubclass IJoinedSubclass.Not
        {
            get { return Not; }
        }

        void IMappingPart.Write(XmlElement classElement, IMappingVisitor visitor)
        {
            throw new NotSupportedException("Obsolete");
        }

        int IMappingPart.LevelWithinPosition
        {
            get { throw new NotSupportedException("Obsolete"); }
        }

        PartPosition IMappingPart.PositionOnDocument
        {
            get { throw new NotSupportedException("Obsolete"); }
        }
    }
}