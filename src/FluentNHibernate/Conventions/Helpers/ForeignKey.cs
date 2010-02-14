using System;
using System.Reflection;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class ForeignKey
    {
        public static ForeignKeyConvention EndsWith(string suffix)
        {
            return new BuiltSuffixForeignKeyConvention(suffix);
        }

        public static ForeignKeyConvention Format(Func<Member, Type, string> format)
        {
            return new BuiltFuncForeignKeyConvention(format);
        }
    }
}