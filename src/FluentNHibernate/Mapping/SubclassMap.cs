using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class SubclassMap<T> : ClasslikeMapBase<T>, IIndeterminateSubclassMappingProvider
    {
        private readonly AttributeStore<SubclassMapping> subclassAttributes = new AttributeStore<SubclassMapping>();
        private readonly AttributeStore<JoinedSubclassMapping> joinedSubclassAttributes = new AttributeStore<JoinedSubclassMapping>();

        // this is a bit weird, but we need a way of delaying the generation of the subclass mappings until we know
        // what the parent subclass type is...
        private readonly IDictionary<Type, IIndeterminateSubclassMappingProvider> indetermineateSubclasses = new Dictionary<Type, IIndeterminateSubclassMappingProvider>();
        private bool nextBool = true;
        private IList<JoinMapping> joins = new List<JoinMapping>();

        public SubclassMap<T> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        ISubclassMapping IIndeterminateSubclassMappingProvider.GetSubclassMapping(ISubclassMapping mapping)
        {
            GenerateNestedSubclasses(mapping);

            subclassAttributes.SetDefault(x => x.Type, typeof(T));
            subclassAttributes.SetDefault(x => x.Name, typeof(T).AssemblyQualifiedName);
            subclassAttributes.SetDefault(x => x.DiscriminatorValue, typeof(T).Name);

            // TODO: un-hardcode this
            var key = new KeyMapping();
            key.AddDefaultColumn(new ColumnMapping { Name = typeof(T).BaseType.Name + "_id" });

            joinedSubclassAttributes.SetDefault(x => x.Type, typeof(T));
            joinedSubclassAttributes.SetDefault(x => x.Name, typeof(T).AssemblyQualifiedName);
            joinedSubclassAttributes.SetDefault(x => x.TableName, GetDefaultTableName());
            joinedSubclassAttributes.SetDefault(x => x.Key, key);

            // TODO: this is nasty, we should find a better way
            if (mapping is JoinedSubclassMapping)
                mapping.OverrideAttributes(joinedSubclassAttributes.CloneInner());
            else
            {
                mapping.OverrideAttributes(subclassAttributes.CloneInner());

                foreach (var join in joins)
                    ((SubclassMapping)mapping).AddJoin(join);
            }

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

        private void GenerateNestedSubclasses(ISubclassMapping mapping)
        {
            foreach (var subclassType in indetermineateSubclasses.Keys)
            {
                var emptySubclassMapping = (ISubclassMapping)mapping.GetType().InstantiateUsingParameterlessConstructor();
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
            subclassAttributes.Set(x => x.Abstract, nextBool);
            joinedSubclassAttributes.Set(x => x.Abstract, nextBool);
            nextBool = true;
        }

        public void DynamicInsert()
        {
            subclassAttributes.Set(x => x.DynamicInsert, nextBool);
            joinedSubclassAttributes.Set(x => x.DynamicInsert, nextBool);
            nextBool = true;
        }

        public void DynamicUpdate()
        {
            subclassAttributes.Set(x => x.DynamicUpdate, nextBool);
            joinedSubclassAttributes.Set(x => x.DynamicUpdate, nextBool);
            nextBool = true;
        }

        public void LazyLoad()
        {
            subclassAttributes.Set(x => x.Lazy, nextBool);
            joinedSubclassAttributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
        }

        public void Proxy<TProxy>()
        {
            Proxy(typeof(TProxy));
        }

        public void Proxy(Type proxyType)
        {
            subclassAttributes.Set(x => x.Proxy, proxyType.AssemblyQualifiedName);
            joinedSubclassAttributes.Set(x => x.Proxy, proxyType.AssemblyQualifiedName);
        }

        public void SelectBeforeUpdate()
        {
            subclassAttributes.Set(x => x.SelectBeforeUpdate, nextBool);
            joinedSubclassAttributes.Set(x => x.SelectBeforeUpdate, nextBool);
            nextBool = true;
        }

        public void Subclass<TSubclass>(Action<SubclassMap<TSubclass>> subclassDefinition)
        {
            var subclass = new SubclassMap<TSubclass>();

            subclassDefinition(subclass);

            indetermineateSubclasses[typeof(TSubclass)] = subclass;
        }

        public void DiscriminatorValue(string discriminatorValue)
        {
            subclassAttributes.Set(x => x.DiscriminatorValue, discriminatorValue);
        }

        public void Table(string table)
        {
            joinedSubclassAttributes.Set(x => x.TableName, table);
        }

        public void Schema(string schema)
        {
            joinedSubclassAttributes.Set(x => x.Schema, schema);
        }

        public void Check(string constraint)
        {
            joinedSubclassAttributes.Set(x => x.Check, constraint);
        }

        public void KeyColumn(string column)
        {
            KeyMapping key;

            if (joinedSubclassAttributes.IsSpecified(x => x.Key))
                key = joinedSubclassAttributes.Get(x => x.Key);
            else
                key = new KeyMapping();

            key.AddColumn(new ColumnMapping { Name = column });

            joinedSubclassAttributes.Set(x => x.Key, key);
        }

        public void Subselect(string subselect)
        {
            joinedSubclassAttributes.Set(x => x.Subselect, subselect);
        }

        public void Persister<TPersister>()
        {
            joinedSubclassAttributes.Set(x => x.Persister, new TypeReference(typeof(TPersister)));
        }

        public void Persister(Type type)
        {
            joinedSubclassAttributes.Set(x => x.Persister, new TypeReference(type));
        }

        public void Persister(string type)
        {
            joinedSubclassAttributes.Set(x => x.Persister, new TypeReference(type));
        }

        public void BatchSize(int batchSize)
        {
            joinedSubclassAttributes.Set(x => x.BatchSize, batchSize);
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
    }
}