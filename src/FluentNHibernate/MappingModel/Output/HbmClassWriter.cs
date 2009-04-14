using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;
using System;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmClassWriter : NullMappingModelVisitor, IXmlWriter<ClassMapping>
    {
        private readonly IXmlWriter<IIdentityMapping> _identityWriter;        
        private readonly IXmlWriter<ISubclassMapping> _subclassWriter;
        private readonly IXmlWriter<DiscriminatorMapping> _discriminatorWriter;
        private readonly HbmMappedMemberWriterHelper _mappedMemberHelper;
        
        private XmlDocument document;

        public HbmClassWriter(IXmlWriter<IIdentityMapping> identityWriter, IXmlWriter<ICollectionMapping> collectionWriter, IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<ManyToOneMapping> manyToOneWriter, IXmlWriter<ISubclassMapping> subclassWriter, IXmlWriter<DiscriminatorMapping> discriminatorWriter, IXmlWriter<ComponentMapping> componentWriter)
        {
            _identityWriter = identityWriter;
            _discriminatorWriter = discriminatorWriter;
            _subclassWriter = subclassWriter;

            _mappedMemberHelper = new HbmMappedMemberWriterHelper(collectionWriter, propertyWriter, manyToOneWriter, componentWriter);
        }

        public object Write(ClassMapping mapping)
        {
            document = null;
            mapping.AcceptVisitor(this);
            return document;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            document = new XmlDocument();

            // create class node
            var classElement = CreateClassElement(classMapping);
            var sortedUnmigratedParts = new List<IMappingPart>(classMapping.UnmigratedParts);

            sortedUnmigratedParts.Sort(new MappingPartComparer());

            foreach (var part in sortedUnmigratedParts)
            {
                part.Write(classElement, null);
            }

            foreach (var attribute in classMapping.UnmigratedAttributes)
            {
                classElement.WithAtt(attribute.Key, attribute.Value); 
            }
        }

        protected virtual XmlElement CreateClassElement(ClassMapping classMapping)
        {
            var typeName = classMapping.Type.IsGenericType ? classMapping.Type.FullName : classMapping.Type.Name;

            var classElement = document.CreateElement("class");

            document.AppendChild(classElement);

            classElement.WithAtt("name", typeName)
                .WithAtt("table", classMapping.Tablename)
                .WithAtt("xmlns", "urn:nhibernate-mapping-2.2");

            //if (batchSize > 0)
            //    classElement.WithAtt("batch-size", batchSize.ToString());

            //classElement.WithProperties(Attributes);

            return classElement;
        }

        //public override void Visit(IIdentityMapping idMapping)
        //{
        //    object idHbm = _identityWriter.Write(idMapping);
        //    _hbm.SetId(idHbm);
        //}

        //public override void Visit(ICollectionMapping collectionMapping)
        //{
        //    object collectionHbm = _mappedMemberHelper.Write(collectionMapping);
        //    collectionHbm.AddTo(ref _hbm.Items);
        //}

        //public override void Visit(PropertyMapping propertyMapping)
        //{
        //    object propertyHbm = _mappedMemberHelper.Write(propertyMapping);
        //    propertyHbm.AddTo(ref _hbm.Items);
        //}

        //public override void Visit(ManyToOneMapping manyToOneMapping)
        //{
        //    object manyHbm = _mappedMemberHelper.Write(manyToOneMapping);
        //    manyHbm.AddTo(ref _hbm.Items);
        //}

        //public override void Visit(ComponentMapping componentMapping)
        //{
        //    object componentHbm = _mappedMemberHelper.Write(componentMapping);
        //    componentHbm.AddTo(ref _hbm.Items);
        //}

        //public override void Visit(ISubclassMapping subclassMapping)
        //{
        //    object subclassHbm = _subclassWriter.Write(subclassMapping);
        //    subclassHbm.AddTo(ref _hbm.Items1);
        //}

        //public override void Visit(DiscriminatorMapping discriminatorMapping)
        //{
        //    _hbm.discriminator = (HbmDiscriminator) _discriminatorWriter.Write(discriminatorMapping);
        //}
    }
}
