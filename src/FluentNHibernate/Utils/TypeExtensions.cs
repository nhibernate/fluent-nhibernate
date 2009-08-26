using System;
using System.Linq;
using NHibernate.Util;

namespace FluentNHibernate.Utils
{
    public static class TypeExtensions
    {
        public static bool Closes(this Type type, Type openGenericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType;
        }

        public static bool IsEnum(this Type type)
        {
            if (type.IsNullable())
                return type.GetGenericArguments()[0].IsEnum;

            return type.IsEnum;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    
        public static T InstantiateUsingParameterlessConstructor<T>(this Type type)
        {
            return (T)type.InstantiateUsingParameterlessConstructor();
        }

        public static object InstantiateUsingParameterlessConstructor(this Type type)
        {
            var constructor = ReflectHelper.GetDefaultConstructor(type);

            if (constructor == null)
                throw new MissingConstructorException(type);

            return constructor.Invoke(null);
        }

        public static bool HasInterface(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Contains(interfaceType);
        }

        public static bool IsTopLevel(this Type type)
        {
            return type.BaseType == typeof(object);
        }
    }
}