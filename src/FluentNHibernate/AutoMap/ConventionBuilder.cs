using System;
using System.Reflection;
using NHibernate.Cfg;

namespace FluentNHibernate.AutoMap
{
    public class ConventionBuilder
    {
        private readonly AutoPersistenceModel model;

        public ConventionBuilder(AutoPersistenceModel model)
        {
            this.model = model;
        }

        public ConventionBuilder SetPrimaryKey(Func<PropertyInfo, string> columnName)
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
            model.Conventions.GetForeignKeyNameOfParent = columnName;
            return this;
        }
    }
}