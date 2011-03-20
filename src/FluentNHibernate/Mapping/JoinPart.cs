using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Maps to the Join element in NH 2.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JoinPart<T> : ClasslikeMapBase<T>, IJoinMappingProvider
    {
        private readonly MappingProviderStore providers;
        private readonly IList<string> columns = new List<string>();
        private readonly FetchTypeExpression<JoinPart<T>> fetch;
        private readonly AttributeStore<JoinMapping> attributes = new AttributeStore<JoinMapping>();
        private bool nextBool = true;

        public JoinPart(string tableName)
            : this(tableName, new MappingProviderStore())
        {}

        protected JoinPart(string tableName, MappingProviderStore providers)
            : base(providers)
        {
            this.providers = providers;
            fetch = new FetchTypeExpression<JoinPart<T>>(this, value => attributes.Set(x => x.Fetch, value));

            attributes.SetDefault(x => x.TableName, tableName);
            attributes.Set(x => x.Key, new KeyMapping { ContainingEntityType = typeof(T) });
        }

        /// <summary>
        /// Specify the key column name
        /// </summary>
        /// <param name="column">Column name</param>
        public JoinPart<T> KeyColumn(string column)
        {
            columns.Clear(); // only one supported currently
            columns.Add(column);
            return this;
        }

        /// <summary>
        /// Specify the key column name
        /// </summary>
        /// <param name="columnNames">Column names</param>
        public JoinPart<T> KeyColumn(params string[] columnNames)
        {
            columns.Clear(); // only one supported currently
            columnNames.Each(columns.Add);
            return this;
        }

        /// <summary>
        /// Specify the schema
        /// </summary>
        /// <param name="schema">Schema name</param>
        public JoinPart<T> Schema(string schema)
        {
            attributes.Set(x => x.Schema, schema);
            return this;
        }

        /// <summary>
        /// Specify the fetching strategy
        /// </summary>
        public FetchTypeExpression<JoinPart<T>> Fetch
        {
            get { return fetch; }
        }

        /// <summary>
        /// Inverse the ownership of this relationship
        /// </summary>
        public JoinPart<T> Inverse()
        {
            attributes.Set(x => x.Inverse, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify this relationship as optional
        /// </summary>
        public JoinPart<T> Optional()
        {
            attributes.Set(x => x.Optional, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify the catalog
        /// </summary>
        /// <param name="catalog">Catalog</param>
        public JoinPart<T> Catalog(string catalog)
        {
            attributes.Set(x => x.Catalog, catalog);
            return this;
        }

        /// <summary>
        /// Specify a subselect for fetching this join
        /// </summary>
        /// <param name="subselect">Query</param>
        public JoinPart<T> Subselect(string subselect)
        {
            attributes.Set(x => x.Subselect, subselect);
            return this;
        }

        /// <summary>
        /// Invert the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public JoinPart<T> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        /// <summary>
        /// Specify the table name
        /// </summary>
        /// <param name="tableName">Table name</param>
        public void Table(string tableName)
        {
            attributes.Set(x => x.TableName, tableName);
        }

        JoinMapping IJoinMappingProvider.GetJoinMapping()
        {
            var mapping = new JoinMapping(attributes.CloneInner());

            mapping.ContainingEntityType = typeof(T);

            if (columns.Count == 0)
                mapping.Key.AddDefaultColumn(new ColumnMapping { Name = typeof(T).Name + "_id" });
            else
                foreach (var column in columns)
                    mapping.Key.AddColumn(new ColumnMapping { Name = column });

            foreach (var property in providers.Properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in providers.Components)
                mapping.AddComponent(component.GetComponentMapping());

            foreach (var reference in providers.References)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var any in providers.Anys)
                mapping.AddAny(any.GetAnyMapping());

            foreach (var collection in providers.Collections)
                mapping.AddCollection(collection.GetCollectionMapping());

            return mapping;
        }
    }
}
