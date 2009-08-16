using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Maps to the Join element in NH 2.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JoinPart<T> : ClasslikeMapBase<T>, IJoinMappingProvider
    {
        private readonly IList<string> columns = new List<string>();
        private readonly FetchTypeExpression<JoinPart<T>> fetch;
        private readonly AttributeStore<JoinMapping> attributes = new AttributeStore<JoinMapping>();
        private bool nextBool = true;

        public JoinPart(string tableName)
        {
            fetch = new FetchTypeExpression<JoinPart<T>>(this, value => attributes.Set(x => x.Fetch, value));

            attributes.SetDefault(x => x.TableName, tableName);
            attributes.Set(x => x.Key, new KeyMapping { ContainingEntityType = typeof(T) });
        }

        public JoinPart<T> KeyColumn(string column)
        {
            columns.Clear(); // only one supported currently
            columns.Add(column);
            return this;
        }

        public JoinPart<T> Schema(string schema)
        {
            attributes.Set(x => x.Schema, schema);
            return this;
        }

        public FetchTypeExpression<JoinPart<T>> Fetch
        {
            get { return fetch; }
        }

        public JoinPart<T> Inverse()
        {
            attributes.Set(x => x.Inverse, nextBool);
            nextBool = true;
            return this;
        }

        public JoinPart<T> Optional()
        {
            attributes.Set(x => x.Optional, nextBool);
            nextBool = true;
            return this;
        }

        public JoinPart<T> Catalog(string catalog)
        {
            attributes.Set(x => x.Catalog, catalog);
            return this;
        }

        public JoinPart<T> Subselect(string subselect)
        {
            attributes.Set(x => x.Subselect, subselect);
            return this;
        }

        public JoinPart<T> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
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

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in components)
                mapping.AddComponent(component.GetComponentMapping());

            foreach (var reference in references)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var any in anys)
                mapping.AddAny(any.GetAnyMapping());

            return mapping;
        }

        public void Table(string tableName)
        {
            attributes.Set(x => x.TableName, tableName);
        }
    }
}
