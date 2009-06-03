using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class JoinedSubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, IJoinedSubclass
    {
        private readonly JoinedSubclassMapping mapping;
        private readonly IList<string> columns = new List<string>();
        private bool nextBool = true;

        public JoinedSubClassPart(string keyColumn)
            : this(new JoinedSubclassMapping())
        {
            columns.Add(keyColumn);
        }

        public JoinedSubClassPart(JoinedSubclassMapping mapping)
        {
            this.mapping = mapping;
        }

        public virtual void JoinedSubClass<TNextSubclass>(string keyColumn, Action<JoinedSubClassPart<TNextSubclass>> action)
        {
            var subclass = new JoinedSubClassPart<TNextSubclass>(keyColumn);

            action(subclass);

            joinedSubclasses.Add(subclass);

            mapping.AddSubclass(subclass.GetJoinedSubclassMapping());
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
            mapping.Proxy = type.AssemblyQualifiedName;
            return this;
        }

        public JoinedSubClassPart<TSubclass> Proxy<T>()
        {
            return Proxy(typeof(T));
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
            mapping.Key = new KeyMapping();
            mapping.Name = typeof(TSubclass).AssemblyQualifiedName;

            foreach (var column in columns)
                mapping.Key.AddColumn(new ColumnMapping { Name = column });

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in components)
                mapping.AddComponent(component.GetComponentMapping());

            foreach (var oneToOne in oneToOnes)
                mapping.AddOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var collection in collections)
                mapping.AddCollection(collection.GetCollectionMapping());

            foreach (var reference in references)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var any in anys)
                mapping.AddAny(any.GetAnyMapping());

            return mapping;
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
    }
}