using System;
using System.Reflection;
using FluentNHibernate.Conventions.Helpers.Prebuilt;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class ForeignKey
    {
        public static ForeignKeyConvention EndsWith(string suffix)
        {
            return new BuiltSuffixForeignKeyConvention(suffix);
        }

        public static ForeignKeyConvention Format(Func<PropertyInfo, Type, string> format)
        {
            return new BuiltFuncForeignKeyConvention(format);
        }
    }
}