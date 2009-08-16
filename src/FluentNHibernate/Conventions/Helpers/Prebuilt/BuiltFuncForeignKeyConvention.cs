using System;
using System.Reflection;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    public class BuiltFuncForeignKeyConvention : ForeignKeyConvention
    {
        private readonly Func<PropertyInfo, Type, string> format;

        public BuiltFuncForeignKeyConvention(Func<PropertyInfo, Type, string> format)
        {
            this.format = format;
        }

        protected override string GetKeyName(PropertyInfo property, Type type)
        {
            return format(property, type);
        }
    }
}