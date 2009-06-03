using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IJoin : IClasslike, IMappingPart
    {
        void WithKeyColumn(string column);
        JoinMapping GetJoinMapping();
    }
    /// <summary>
    /// Maps to the Join element in NH 2.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JoinPart<T> : ClasslikeMapBase<T>, IJoin
    {
        private readonly IList<string> columns = new List<string>();
        private readonly JoinMapping mapping = new JoinMapping();
        private readonly FetchTypeExpression<JoinPart<T>> fetch;
        private bool nextBool = true;

        public JoinPart(string tableName)
        {
            fetch = new FetchTypeExpression<JoinPart<T>>(this, value => mapping.Fetch = value);
            mapping.TableName = tableName;
            mapping.Key = new KeyMapping();

            columns.Add(GetType().GetGenericArguments()[0].Name + "ID");
        }

        public JoinPart<T> WithKeyColumn(string column)
        {
            columns.Clear(); // only one supported currently
            columns.Add(column);
            return this;
        }

        void IJoin.WithKeyColumn(string column)
        {
            WithKeyColumn(column);
        }

        public JoinPart<T> SchemaIs(string schema)
        {
            mapping.Schema = schema;
            return this;
        }

        public FetchTypeExpression<JoinPart<T>> Fetch
        {
            get { return fetch; }
        }

        public JoinPart<T> Inverse()
        {
            mapping.Inverse = nextBool;
            nextBool = true;
            return this;
        }

        public JoinPart<T> Optional()
        {
            mapping.Optional = nextBool;
            nextBool = true;
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

        public JoinMapping GetJoinMapping()
        {
            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in components)
                mapping.AddComponent(component.GetComponentMapping());

            foreach (var reference in references)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var any in anys)
                mapping.AddAny(any.GetAnyMapping());

            foreach (var column in columns)
                mapping.Key.AddColumn(new ColumnMapping { Name = column });

            return mapping;
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
