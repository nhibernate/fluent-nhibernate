using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class PrimaryKey
    {
        public static PrimaryKeyNameBuilder Name
        {
            get { return new PrimaryKeyNameBuilder(); }
        }
    }

    public class PrimaryKeyNameBuilder
    {
        internal PrimaryKeyNameBuilder()
        {}

        public IIdConvention Is(Func<IIdentityPart, string> nameFunc)
        {
            return new BuiltIdConvention(id =>
            {
                var columnName = nameFunc(id);
                id.TheColumnNameIs(columnName);
            });
        }
    }
}