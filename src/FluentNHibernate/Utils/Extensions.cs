using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Infrastructure;
using NHibernate.Util;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FluentNHibernate.Utils
{
    public static class Extensions
    {
        public static bool In<T>(this T instance, params T[] expected)
        {
            if (ReferenceEquals(instance, null))
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

        [Obsolete("Please do not use this method. It will be removed in a future version.")]
        public static T DeepClone<T>(this T obj)
        {
            using (var stream = new MemoryStream())
            {

#if NETFRAMEWORK
                var formatter = new BinaryFormatter();
#else
                var formatter = new BinaryFormatter(new NetStandardSerialization.SurrogateSelector(), new StreamingContext());
#endif

                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);

                return (T)formatter.Deserialize(stream);
            }
        }

        public static bool IsAutoMappingOverrideType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IAutoMappingOverride<>) && type.GetGenericArguments().Length > 0;
        }
    }
}