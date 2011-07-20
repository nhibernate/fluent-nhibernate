using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
namespace FluentNHibernate.Mapping
{
    public class KeyPropertyPart
    {
        private readonly KeyPropertyMapping mapping;

        public KeyPropertyPart(KeyPropertyMapping mapping)
        {
            this.mapping = mapping;
            Access = new AccessStrategyBuilder<KeyPropertyPart>(this, value => mapping.Access = value);
        }

        public KeyPropertyPart ColumnName(string columnName)
        {
            mapping.AddColumn(new ColumnMapping { Name = columnName });
            return this;
        }

        public KeyPropertyPart Type(Type type)
        {
            mapping.Type = new TypeReference(type);
            return this;
        }

        public KeyPropertyPart Type(string type)
        {
            mapping.Type = new TypeReference(type);
            return this;
        }

        public KeyPropertyPart Length(int length)
        {
            mapping.Length = length;
            return this;
        }

        public AccessStrategyBuilder<KeyPropertyPart> Access { get; private set; }
    }
}