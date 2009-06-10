using System;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class PropertyAlteration : IPropertyAlteration
    {
        private readonly PropertyMapping mapping;
        private bool nextBool = true;

        public PropertyAlteration(PropertyMapping mapping)
        {
            this.mapping = mapping;
        }

        public void Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;
        }

        public void Update()
        {
            mapping.Update = nextBool;
            nextBool = true;
        }

        public void ReadOnly()
        {
            mapping.Insert = mapping.Update = nextBool;
            nextBool = true;
        }

        public void Nullable()
        {
            foreach (var column in mapping.Columns)
                column.NotNull = !nextBool;

            nextBool = true;
        }

        public IAccessStrategyBuilder Access
        {
            get { return new AccessMappingAlteration(mapping); }
        }

        public void CustomTypeIs<T>()
        {
            mapping.Type = new TypeReference(typeof(T));
        }

        public void CustomTypeIs(TypeReference type)
        {
            mapping.Type = type;
        }

        public void CustomTypeIs(Type type)
        {
            mapping.Type = new TypeReference(type);
        }

        public void CustomTypeIs(string type)
        {
            mapping.Type = new TypeReference(type);
        }

        public void CustomSqlTypeIs(string sqlType)
        {
            foreach (var column in mapping.Columns)
                column.SqlType = sqlType;
        }

        public void Unique()
        {
            foreach (var column in mapping.Columns)
                column.Unique = nextBool;

            nextBool = true;
        }

        public void UniqueKey(string keyName)
        {
            foreach (var column in mapping.Columns)
                column.UniqueKey = keyName;
        }

        public IPropertyAlteration Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }
    }
}