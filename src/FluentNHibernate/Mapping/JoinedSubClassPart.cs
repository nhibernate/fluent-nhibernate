using System;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class JoinedSubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, IJoinedSubclass
    {
        private readonly string keyColumn;
        private readonly Cache<string, string> unmigratedAttributes = new Cache<string, string>();
        private readonly JoinedSubclassMapping mapping;

        public JoinedSubClassPart(string keyColumn)
            : this(new JoinedSubclassMapping())
        {
            this.keyColumn = keyColumn;
        }

        public JoinedSubClassPart(JoinedSubclassMapping mapping)
        {
            this.mapping = mapping;
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

        public JoinedSubclassMapping GetJoinedSubclassMapping()
        {
            mapping.Key = new KeyMapping
            {
                Column = keyColumn
            };
            mapping.Name = typeof(TSubclass).AssemblyQualifiedName;

            foreach (var property in Properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var part in Parts)
                mapping.AddUnmigratedPart(part);

            unmigratedAttributes.ForEachPair(mapping.AddUnmigratedAttribute);

            return mapping;
        }

        void IJoinedSubclass.WithTableName(string tableName)
        {
            WithTableName(tableName);
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