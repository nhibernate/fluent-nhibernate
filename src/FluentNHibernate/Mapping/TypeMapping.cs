using System;

namespace FluentNHibernate.Mapping
{
    public static class TypeMapping
    {
        public static string GetTypeString(Type type)
        {
            return type.Name;
        }
    }
}