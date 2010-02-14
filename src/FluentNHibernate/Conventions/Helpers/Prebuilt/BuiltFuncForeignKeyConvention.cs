using System;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    public class BuiltFuncForeignKeyConvention : ForeignKeyConvention
    {
        private readonly Func<Member, Type, string> format;

        public BuiltFuncForeignKeyConvention(Func<Member, Type, string> format)
        {
            this.format = format;
        }

        protected override string GetKeyName(Member property, Type type)
        {
            return format(property, type);
        }
    }
}