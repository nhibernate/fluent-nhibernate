using System;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

public class BuiltFuncForeignKeyConvention(Func<Member, Type, string> format) : ForeignKeyConvention
{
    protected override string GetKeyName(Member property, Type type)
    {
        return format(property, type);
    }
}
