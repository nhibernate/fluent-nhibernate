using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class VersionPart : IVersionMappingProvider
    {
        private readonly Type entity;
        private readonly PropertyInfo property;
        private readonly AccessStrategyBuilder<VersionPart> access;
        private readonly VersionGeneratedBuilder<IVersionMappingProvider> generated;
        private readonly AttributeStore<VersionMapping> attributes = new AttributeStore<VersionMapping>();
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private readonly List<string> columns = new List<string>();
        private bool nextBool = true;

        public VersionPart(Type entity, PropertyInfo property)
        {
            this.entity = entity;
            this.property = property;
            access = new AccessStrategyBuilder<VersionPart>(this, value => attributes.Set(x => x.Access, value));
            generated = new VersionGeneratedBuilder<IVersionMappingProvider>(this, value => attributes.Set(x => x.Generated, value));
        }

        VersionMapping IVersionMappingProvider.GetVersionMapping()
        {
            var mapping = new VersionMapping(attributes.CloneInner());

            mapping.ContainingEntityType = entity;

            mapping.SetDefaultValue("Name", property.Name);
            mapping.SetDefaultValue("Type", property.PropertyType == typeof(DateTime) ? new TypeReference("timestamp") : new TypeReference(property.PropertyType));
            mapping.AddDefaultColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = property.Name });

            columns.ForEach(column => mapping.AddColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = column }));

            return mapping;
        }

        public VersionGeneratedBuilder<IVersionMappingProvider> Generated
        {
            get { return generated; }
        }

        public AccessStrategyBuilder<VersionPart> Access
        {
            get { return access; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public VersionPart Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public VersionPart Column(string name)
        {
            columns.Add(name);
            return this;
        }

        public VersionPart UnsavedValue(string value)
        {
            attributes.Set(x => x.UnsavedValue, value);
            return this;
        }

        public VersionPart Length(int length)
        {
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        public VersionPart Precision(int precision)
        {
            columnAttributes.Set(x => x.Precision, precision);
            return this;
        }

        public VersionPart Scale(int scale)
        {
            columnAttributes.Set(x => x.Scale, scale);
            return this;
        }

        public VersionPart Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        public VersionPart Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        public VersionPart UniqueKey(string keyColumns)
        {
            columnAttributes.Set(x => x.UniqueKey, keyColumns);
            return this;
        }

        public VersionPart Index(string index)
        {
            columnAttributes.Set(x => x.Index, index);
            return this;
        }

        public VersionPart Check(string constraint)
        {
            columnAttributes.Set(x => x.Check, constraint);
            return this;
        }

        public VersionPart Default(object value)
        {
            columnAttributes.Set(x => x.Default, value.ToString());
            return this;
        }

        public VersionPart CustomType<T>()
        {
            attributes.Set(x => x.Type, new TypeReference(typeof(T)));
            return this;
        }

        public VersionPart CustomType(Type type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }

        public VersionPart CustomType(string type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }

        public VersionPart CustomSqlType(string sqlType)
        {
            columnAttributes.Set(x => x.SqlType, sqlType);
            return this;
        }
    }
}