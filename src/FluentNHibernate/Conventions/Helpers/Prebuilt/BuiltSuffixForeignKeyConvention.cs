using System;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

public class BuiltSuffixForeignKeyConvention(string suffix) : ForeignKeyConvention
{
    protected override string GetKeyName(Member property, Type type)
    {
        return (property is not null ? property.Name : type.Name) + suffix;
    }
}
