using System;
using System.Reflection;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    public class BuiltSuffixForeignKeyConvention : ForeignKeyConvention
    {
        private readonly string suffix;

        public BuiltSuffixForeignKeyConvention(string suffix)
        {
            this.suffix = suffix;
        }

        protected override string GetKeyName(PropertyInfo property, Type type)
        {
            return (property != null ? property.Name : type.Name) + suffix;
        }
    }
}