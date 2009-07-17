using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output.Sorting;
using FluentNHibernate.Utils;
using FluentNHibernate.Xml;
using NHibernate.Mapping;

namespace FluentNHibernate.MappingModel.Output
{
    public class NHModelHibernateMappingWriter : NullMappingModelVisitor, INHModelWriter<HibernateMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private ConvertedClassesResult document;

        public NHModelHibernateMappingWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public object Write(HibernateMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return document;
        }

        public override void ProcessHibernateMapping(HibernateMapping mapping)
        {
            document = new ConvertedClassesResult();
            document.Classes = new List<PersistentClass>();
            document.Collections = new List<Collection>();
        }

        public override void Visit(ClassMapping classMapping)
        {
            var writer = serviceLocator.GetWriter<ClassMapping>();
            var hbmClass = writer.Write(classMapping);

            document.Classes.Add((PersistentClass)hbmClass);
        }
    }
}