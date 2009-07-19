using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class JoinedSubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, IJoinedSubclassMappingProvider
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

            joinedSubclasses[typeof(TNextSubclass)] = subclass;

            mapping.AddSubclass(((IJoinedSubclassMappingProvider)subclass).GetJoinedSubclassMapping());
        }

        public JoinedSubClassPart<TSubclass> Table(string tableName)
        {
            mapping.TableName = tableName;
            return this;
        }

        public JoinedSubClassPart<TSubclass> Schema(string schema)
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
            mapping.Lazy = nextBool ? Laziness.True : Laziness.False;
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

        JoinedSubclassMapping IJoinedSubclassMappingProvider.GetJoinedSubclassMapping()
        {
            mapping.Key = new KeyMapping { ContainingEntityType = typeof(TSubclass) };
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
    }
}