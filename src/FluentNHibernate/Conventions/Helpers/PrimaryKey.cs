using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
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
            return new BuiltIdConvention(x => true, id =>
            {
                var columnName = nameFunc(id);
                id.TheColumnNameIs(columnName);
            });
        }
    }
}