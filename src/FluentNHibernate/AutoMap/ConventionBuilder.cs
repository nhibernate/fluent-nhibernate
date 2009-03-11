using System;
using System.Reflection;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;

namespace FluentNHibernate.AutoMap
{
    // what's this? it doesn't seem to be used anywhere
    public class ConventionBuilder
    {
        private readonly AutoPersistenceModel model;

        public ConventionBuilder(AutoPersistenceModel model)
        {
            this.model = model;
        }

        public ConventionBuilder SetPrimaryKey(Func<IIdentityPart, string> columnName)
        {
            model.Conventions.GetPrimaryKeyName = columnName;
            return this;
        }

        public ConventionBuilder SetIdentityAs(Func<PropertyInfo, bool> findIdentity)
        {
            model.Conventions.FindIdentity = findIdentity;
            return this;
        }

        public void Configure(Configuration cfg)
        {
            model.Configure(cfg);
        }

        public ConventionBuilder SetManyToOneKey(Func<PropertyInfo, string> columnName)
        {
            model.Conventions.GetForeignKeyName = columnName;
            return this;
        }

        public ConventionBuilder SetOneToManyKey(Func<Type, string> columnName)
        {
            model.Conventions.GetForeignKeyNameForType = columnName;
            return this;
        }
    }
}