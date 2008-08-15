using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.QuickStart.Domain.Mapping
{
    public class GeneratorHelper
    {
        private const string GENERATOR_INTERFACE = "IMapGenerator";

        public static IList<IMapGenerator> GetMapGenerators()
        {
            IList<IMapGenerator> generators = new List<IMapGenerator>();
            Assembly assembly = Assembly.GetAssembly(typeof(IMapGenerator));
            foreach (Type type in assembly.GetTypes())
            {
                if (null == type.GetInterface(GENERATOR_INTERFACE)) continue;
                var instance = Activator.CreateInstance(type) as IMapGenerator;
                if (instance != null)
                    generators.Add(instance);
            }
            return generators;
        }
    }
}