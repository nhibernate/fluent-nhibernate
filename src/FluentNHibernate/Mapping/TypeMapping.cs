using System;

namespace ShadeTree.DomainModel.Mapping
{
    public static class TypeMapping
    {
        public static string GetTypeString(Type type)
        {
            return type.Name;
        }
    }
}