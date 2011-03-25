using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Util;

namespace FluentNHibernate.Utils
{
    public static class Extensions
    {
        public static bool In<T>(this T instance, params T[] expected)
        {
            if(ReferenceEquals(instance, null))
                return false;
            return expected.Any(x => instance.Equals(x));
        }

        public static string ToLowerInvariantString(this object value)
        {
            return value.ToString().ToLowerInvariant();
        }

        public static bool Closes(this Type type, Type openGenericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType;
        }

        public static bool ClosesInterface(this Type type, Type openGenericInterface)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == openGenericInterface);
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

        public static T DeepClone<T>(this T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                
                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }
    }
}