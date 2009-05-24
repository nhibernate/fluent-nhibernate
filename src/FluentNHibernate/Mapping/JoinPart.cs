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
        private readonly Cache<string, string> unmigratedAttributes = new Cache<string, string>();
        private readonly IList<string> columns = new List<string>();
        private readonly JoinMapping joinMapping = new JoinMapping();

        public JoinPart(string tableName)
        {
            joinMapping.TableName = tableName;
            joinMapping.Key = new KeyMapping();

            columns.Add(GetType().GetGenericArguments()[0].Name + "ID");
        }

        public void SetAttribute(string name, string value)
        {
            unmigratedAttributes.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public int LevelWithinPosition
        {
            get { return 3; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.Last; }
        }

        public void WithKeyColumn(string column)
        {
            columns.Clear(); // only one supported currently
            columns.Add(column);
        }

        public JoinMapping GetJoinMapping()
        {
            foreach (var property in properties)
                joinMapping.AddProperty(property.GetPropertyMapping());

            foreach (var column in columns)
                joinMapping.Key.AddColumn(new ColumnMapping { Name = column });

            foreach (var part in Parts)
                joinMapping.AddUnmigratedPart(part);

            unmigratedAttributes.ForEachPair(joinMapping.AddUnmigratedAttribute);

            return joinMapping;
        }

        void IMappingPart.Write(XmlElement classElement, IMappingVisitor visitor)
        {
            throw new NotSupportedException("Obsolete");
        }
    }
}
