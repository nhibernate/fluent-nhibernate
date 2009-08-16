using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
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

        public IIdConvention Is(Func<IIdentityInspector, string> nameFunc)
        {
            return new BuiltIdConvention(accept => { }, instance =>
            {
                var columnName = nameFunc(instance);

                instance.Column(columnName);
            });
        }
    }
}