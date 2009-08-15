using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using NHibernate.Util;

namespace FluentNHibernate
{
    public static class ConfigurationHelper
    {
        public static Configuration AddMappingsFromAssembly(this Configuration configuration, Assembly assembly)
        {
            var models = new PersistenceModel();
            models.AddMappingsFromAssembly(assembly);
            models.Configure(configuration);

            return configuration;
        }

        public static Configuration AddAutoMappings(this Configuration configuration, AutoPersistenceModel model)
        {
            model.Configure(configuration);

            return configuration;
        }
    }

    public static class TypeExtensions
    {
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
    }

    public static class CollectionExtensions
    {
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> each)
        {
            foreach (var item in enumerable)
                each(item);
        }
    }
}