using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Mapping.Builders;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class JoinedSubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, ISubclassMappingProvider
    {
        private readonly List<SubclassMapping> subclassMappings = new List<SubclassMapping>();
        private readonly AttributeStore<SubclassMapping> attributes;
        private bool nextBool = true;
        KeyMapping keyMapping;

        public JoinedSubClassPart()
            : this(new AttributeStore())
        {}

        public JoinedSubClassPart(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<SubclassMapping>(underlyingStore);
            keyMapping = new KeyMapping { ContainingEntityType = typeof(TSubclass) };
        }

        public virtual void JoinedSubClass<TNextSubclass>(string keyColumn, Action<JoinedSubClassPart<TNextSubclass>> action)
        {
            var subclass = new JoinedSubClassPart<TNextSubclass>();

            subclass.KeyColumn(keyColumn);

            action(subclass);

            subclasses[typeof(TNextSubclass)] = subclass;

            subclassMappings.Add(((ISubclassMappingProvider)subclass).GetSubclassMapping());
        }

        /// <summary>
        /// Specify how the foreign key is configured.
        /// </summary>
        /// <param name="keyConfiguration">Configuration <see cref="Action"/></param>
        /// <returns>Builder</returns>
        public void Key(Action<KeyBuilder> keyConfiguration)
        {
            keyConfiguration(new KeyBuilder(keyMapping));
        }


        /// <summary>
        /// Specify the foreign key column name
        /// </summary>
        /// <param name="keyColumn">Key column name</param>
        public void KeyColumn(string keyColumn)
        {
            Key(ke => ke.Column(keyColumn));
        }

        /// <summary>
        /// Modify the key columns collection
        /// </summary>
        [Obsolete("Deprecated in favour of Key(ke => ke.Columns...)")]
        public ColumnMappingCollection<JoinedSubClassPart<TSubclass>> KeyColumns
        {
            get { return new ColumnMappingCollection<JoinedSubClassPart<TSubclass>>(this, new KeyBuilder(keyMapping).Columns); }
        }

        public JoinedSubClassPart<TSubclass> Table(string tableName)
        {
            attributes.Set(x => x.TableName, tableName);
            return this;
        }

        public JoinedSubClassPart<TSubclass> Schema(string schema)
        {
            attributes.Set(x => x.Schema, schema);
            return this;
        }

        public JoinedSubClassPart<TSubclass> CheckConstraint(string constraintName)
        {
            attributes.Set(x => x.Check, constraintName);
            return this;
        }

        public JoinedSubClassPart<TSubclass> Proxy(Type type)
        {
            attributes.Set(x => x.Proxy, type.AssemblyQualifiedName);
            return this;
        }

        public JoinedSubClassPart<TSubclass> Proxy<T>()
        {
            return Proxy(typeof(T));
        }

        public JoinedSubClassPart<TSubclass> LazyLoad()
        {
            attributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> DynamicUpdate()
        {
            attributes.Set(x => x.DynamicUpdate, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> DynamicInsert()
        {
            attributes.Set(x => x.DynamicInsert, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> SelectBeforeUpdate()
        {
            attributes.Set(x => x.SelectBeforeUpdate, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> Abstract()
        {
            attributes.Set(x => x.Abstract, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public JoinedSubClassPart<TSubclass> EntityName(string entityName)
        {
            attributes.Set(x => x.EntityName, entityName);
            return this;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public JoinedSubClassPart<TSubclass> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        SubclassMapping ISubclassMappingProvider.GetSubclassMapping()
        {
            var mapping = new SubclassMapping(SubclassType.JoinedSubclass, attributes.CloneInner());

            mapping.Key = keyMapping;
            mapping.Name = typeof(TSubclass).AssemblyQualifiedName;
            mapping.Type = typeof(TSubclass);

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