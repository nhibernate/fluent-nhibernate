using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using StructureMap;
using StructureMap.Graph;

namespace FluentNHibernate.MappingModel.Output
{
    public static class HbmWriterFactory
    {
        private static bool _initialized = false;

        public static IHbmWriter<HibernateMapping> CreateHibernateMappingWriter()
        {
            if (!_initialized)
                Initialize();

            return ObjectFactory.GetInstance<IHbmWriter<HibernateMapping>>();
        }

        private class HbmWriterScanner : ITypeScanner
        {
            public void Process(Type type, PluginGraph graph)
            {
                Type interfaceType = type.FindInterfaceThatCloses(typeof(IHbmWriter<>));
                if (interfaceType != null)
                {
                    graph.AddType(interfaceType, type);
                } 
            }
        }

        private static void Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(s =>
                {
                    s.TheCallingAssembly();
                    s.With(new HbmWriterScanner());
                });
            });

            Debug.Write(ObjectFactory.WhatDoIHave());
            _initialized = true;
        }
    }
}
