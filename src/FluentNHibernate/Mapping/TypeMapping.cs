using System;

namespace FluentNHibernate.Mapping
{
    public static class TypeMapping
    {
        public static string GetTypeString(Type type)
        {
            if (type.Assembly == typeof(string).Assembly)
                return type.IsGenericType ? type.FullName : type.Name;

            return type.AssemblyQualifiedName;
        }
    }
}