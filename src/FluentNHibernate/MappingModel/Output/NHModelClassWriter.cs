using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Util;

namespace FluentNHibernate.MappingModel.Output
{
    public class NHModelClassWriter : XmlClassWriterBase, INHModelWriter<ClassMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private PersistentClass model;

        public NHModelClassWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public object Write(ClassMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return model;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            model = new RootClass();

            if (classMapping.HasValue(x => x.BatchSize))
                model.BatchSize = classMapping.BatchSize;

            if (classMapping.HasValue(x => x.DiscriminatorValue))
                model.DiscriminatorValue = classMapping.DiscriminatorValue.ToString();

            if (classMapping.HasValue(x => x.DynamicInsert))
                model.DynamicInsert = classMapping.DynamicInsert;

            if (classMapping.HasValue(x => x.DynamicUpdate))
                model.DynamicUpdate = classMapping.DynamicUpdate;

            if (classMapping.HasValue(x => x.Lazy))
                model.IsLazy = classMapping.Lazy == Laziness.True;

            if (classMapping.HasValue(x => x.Mutable))
                model.IsMutable = classMapping.Mutable;

            if (classMapping.HasValue(x => x.Persister))
                model.EntityPersisterClass = ReflectHelper.ClassForName(classMapping.Persister);

            if (classMapping.HasValue(x => x.Where))
                model.Where = classMapping.Where;

            //if (classMapping.HasValue(x => x.OptimisticLock))
            //    classElement.WithAtt("optimistic-lock", classMapping.OptimisticLock);

            //if (classMapping.HasValue(x => x.Check))
            //    classElement.WithAtt("check", classMapping.Check);

            if (classMapping.HasValue(x => x.Name))
                model.EntityName = classMapping.Name;

            if (classMapping.HasValue(x => x.TableName))
                model.Table.Name = classMapping.TableName;

            if (classMapping.HasValue(x => x.Proxy))
                model.ProxyInterfaceName = classMapping.Proxy;

            if (classMapping.HasValue(x => x.SelectBeforeUpdate))
                model.SelectBeforeUpdate = classMapping.SelectBeforeUpdate;

            if (classMapping.HasValue(x => x.Abstract))
                model.IsAbstract = classMapping.Abstract;
        }

        //public override void Visit(DiscriminatorMapping discriminatorMapping)
        //{
        //    var writer = serviceLocator.GetWriter<DiscriminatorMapping>();
        //    var discriminatorXml = writer.Write(discriminatorMapping);

        //    document.ImportAndAppendChild(discriminatorXml);
        //}

        //public override void Visit(ISubclassMapping subclassMapping)
        //{
        //    var writer = serviceLocator.GetWriter<ISubclassMapping>();
        //    var subclassXml = writer.Write(subclassMapping);

        //    document.ImportAndAppendChild(subclassXml);
        //}

        //public override void Visit(IComponentMapping componentMapping)
        //{
        //    var writer = serviceLocator.GetWriter<IComponentMapping>();
        //    var componentXml = writer.Write(componentMapping);

        //    document.ImportAndAppendChild(componentXml);
        //}

        //public override void Visit(JoinMapping joinMapping)
        //{
        //    var writer = serviceLocator.GetWriter<JoinMapping>();
        //    var joinXml = writer.Write(joinMapping);

        //    document.ImportAndAppendChild(joinXml);
        //}

        //public override void Visit(IIdentityMapping mapping)
        //{
        //    var writer = serviceLocator.GetWriter<IIdentityMapping>();
        //    var idXml = writer.Write(mapping);

        //    document.ImportAndAppendChild(idXml);
        //}

        //public override void Visit(CacheMapping mapping)
        //{
        //    var writer = serviceLocator.GetWriter<CacheMapping>();
        //    var cacheXml = writer.Write(mapping);

        //    document.ImportAndAppendChild(cacheXml);
        //}

        //public override void Visit(ManyToOneMapping manyToOneMapping)
        //{
        //    var writer = serviceLocator.GetWriter<ManyToOneMapping>();
        //    var manyToOneXml = writer.Write(manyToOneMapping);

        //    document.ImportAndAppendChild(manyToOneXml);
        //}
    }
}
