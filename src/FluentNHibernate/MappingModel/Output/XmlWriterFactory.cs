using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using StructureMap;
using StructureMap.Graph;

namespace FluentNHibernate.MappingModel.Output
{
    public static class XmlWriterFactory
    {
        private static bool _initialized = false;

        public static IXmlWriter<HibernateMapping> CreateHibernateMappingWriter()
        {
            if (!_initialized)
                Initialize();

            return ObjectFactory.GetInstance<IXmlWriter<HibernateMapping>>();
        }

        private class XmlWriterScanner : ITypeScanner
        {
            public void Process(Type type, PluginGraph graph)
            {
                Type interfaceType = type.FindInterfaceThatCloses(typeof(IXmlWriter<>));
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
                    s.With(new XmlWriterScanner());
                });
            });

            //Debug.Write(ObjectFactory.WhatDoIHave());
            _initialized = true;
        }
    }
}
