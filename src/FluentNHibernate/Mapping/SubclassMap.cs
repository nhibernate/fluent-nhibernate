using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class SubclassMap<T> : ClasslikeMapBase<T>, IIndeterminateSubclassMappingProvider
    {
        private readonly AttributeStore<SubclassMapping> attributes = new AttributeStore<SubclassMapping>();

        // this is a bit weird, but we need a way of delaying the generation of the subclass mappings until we know
        // what the parent subclass type is...
        private readonly IDictionary<Type, IIndeterminateSubclassMappingProvider> indetermineateSubclasses = new Dictionary<Type, IIndeterminateSubclassMappingProvider>();
        private bool nextBool = true;
        private IList<JoinMapping> joins = new List<JoinMapping>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public SubclassMap<T> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        SubclassMapping IIndeterminateSubclassMappingProvider.GetSubclassMapping(SubclassMapping mapping)
        {
            GenerateNestedSubclasses(mapping);

            attributes.SetDefault(x => x.Type, typeof(T));
            attributes.SetDefault(x => x.Name, typeof(T).AssemblyQualifiedName);
            attributes.SetDefault(x => x.DiscriminatorValue, typeof(T).Name);

            // TODO: un-hardcode this
            var key = new KeyMapping();
            key.AddDefaultColumn(new ColumnMapping { Name = typeof(T).BaseType.Name + "_id" });

            attributes.SetDefault(x => x.TableName, GetDefaultTableName());
            attributes.SetDefault(x => x.Key, key);

            // TODO: this is nasty, we should find a better way
            mapping.OverrideAttributes(attributes.CloneInner());

            foreach (var join in joins)
                mapping.AddJoin(join);

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

        Type IIndeterminateSubclassMappingProvider.Extends
        {
            get { return attributes.Get(x => x.Extends); }
        }

        private void GenerateNestedSubclasses(SubclassMapping mapping)
        {
            foreach (var subclassType in indetermineateSubclasses.Keys)
            {
                var emptySubclassMapping = new SubclassMapping(mapping.SubclassType);
                var subclassMapping = indetermineateSubclasses[subclassType].GetSubclassMapping(emptySubclassMapping);

                mapping.AddSubclass(subclassMapping);
            }
        }

        private string GetDefaultTableName()
        {
            var tableName = EntityType.Name;

            if (EntityType.IsGenericType)
            {
                // special case for generics: GenericType_GenericParameterType
                tableName = EntityType.Name.Substring(0, EntityType.Name.IndexOf('`'));

                foreach (var argument in EntityType.GetGenericArguments())
                {
                    tableName += "_";
                    tableName += argument.Name;
                }
            }

            return "`" + tableName + "`";
        }

        public void Abstract()
        {
            attributes.Set(x => x.Abstract, nextBool);
            nextBool = true;
        }

        public void DynamicInsert()
        {
            attributes.Set(x => x.DynamicInsert, nextBool);
            nextBool = true;
        }

        public void DynamicUpdate()
        {
            attributes.Set(x => x.DynamicUpdate, nextBool);
            nextBool = true;
        }

        public void LazyLoad()
        {
            attributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
        }

        public void Proxy<TProxy>()
        {
            Proxy(typeof(TProxy));
        }

        public void Proxy(Type proxyType)
        {
            attributes.Set(x => x.Proxy, proxyType.AssemblyQualifiedName);
        }

        public void SelectBeforeUpdate()
        {
            attributes.Set(x => x.SelectBeforeUpdate, nextBool);
            nextBool = true;
        }

        public void Subclass<TSubclass>(Action<SubclassMap<TSubclass>> subclassDefinition)
        {
            var subclass = new SubclassMap<TSubclass>();

            subclassDefinition(subclass);

            indetermineateSubclasses[typeof(TSubclass)] = subclass;
        }

        public void DiscriminatorValue(object discriminatorValue)
        {
            attributes.Set(x => x.DiscriminatorValue, discriminatorValue);
        }

        public void Table(string table)
        {
            attributes.Set(x => x.TableName, table);
        }

        public void Schema(string schema)
        {
            attributes.Set(x => x.Schema, schema);
        }

        public void Check(string constraint)
        {
            attributes.Set(x => x.Check, constraint);
        }

        public void KeyColumn(string column)
        {
            KeyMapping key;

            if (attributes.IsSpecified(x => x.Key))
                key = attributes.Get(x => x.Key);
            else
                key = new KeyMapping();

            key.AddColumn(new ColumnMapping { Name = column });

            attributes.Set(x => x.Key, key);
        }

        public void Subselect(string subselect)
        {
            attributes.Set(x => x.Subselect, subselect);
        }

        public void Persister<TPersister>()
        {
            attributes.Set(x => x.Persister, new TypeReference(typeof(TPersister)));
        }

        public void Persister(Type type)
        {
            attributes.Set(x => x.Persister, new TypeReference(type));
        }

        public void Persister(string type)
        {
            attributes.Set(x => x.Persister, new TypeReference(type));
        }

        public void BatchSize(int batchSize)
        {
            attributes.Set(x => x.BatchSize, batchSize);
        }

        public void EntityName(string entityname)
        {
            attributes.Set(x => x.EntityName, entityname);
        }

        /// <summary>
        /// Sets additional tables for the class via the NH 2.0 Join element, this only works if
        /// the hierarchy you're mapping has a discriminator.
        /// </summary>
        /// <param name="tableName">Joined table name</param>
        /// <param name="action">Joined table mapping</param>
        public void Join(string tableName, Action<JoinPart<T>> action)
        {
            var join = new JoinPart<T>(tableName);

            action(join);

            joins.Add(((IJoinMappingProvider)join).GetJoinMapping());
        }

        /// <summary>
        /// (optional) Specifies the entity from which this subclass descends/extends.
        /// </summary>
        /// <typeparam name="TOther">Type of the entity to extend</typeparam>
        public void Extends<TOther>()
        {
            Extends(typeof(TOther));
        }

        /// <summary>
        /// (optional) Specifies the entity from which this subclass descends/extends.
        /// </summary>
        /// <param name="type">Type of the entity to extend</param>
        public void Extends(Type type)
        {
            attributes.Set(x => x.Extends, type);
        }
    }
}