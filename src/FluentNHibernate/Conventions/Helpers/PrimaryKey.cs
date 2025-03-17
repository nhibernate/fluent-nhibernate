using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers;

public static class PrimaryKey
{
    public static PrimaryKeyNameBuilder Name => new();
}

public class PrimaryKeyNameBuilder
{
    internal PrimaryKeyNameBuilder()
    {}

    public IIdConvention Is(Func<IIdentityInspector, string> nameFunc)
    {
        return new BuiltIdConvention(accept => { }, instance =>
        {
            var columnName = nameFunc(instance);

            instance.Column(columnName);
        });
    }
}
