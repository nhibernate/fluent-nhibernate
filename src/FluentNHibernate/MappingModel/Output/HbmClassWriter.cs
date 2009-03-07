using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;
using System;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmClassWriter : NullMappingModelVisitor, IHbmWriter<ClassMapping>
    {
        private readonly IHbmWriter<IIdentityMapping> _identityWriter;        
        private readonly IHbmWriter<ISubclassMapping> _subclassWriter;
        private readonly IHbmWriter<DiscriminatorMapping> _discriminatorWriter;
        private readonly HbmMappedMemberWriterHelper _mappedMemberHelper;
        
        private HbmClass _hbm;

        public HbmClassWriter(IHbmWriter<IIdentityMapping> identityWriter, IHbmWriter<ICollectionMapping> collectionWriter, IHbmWriter<PropertyMapping> propertyWriter, IHbmWriter<ManyToOneMapping> manyToOneWriter, IHbmWriter<ISubclassMapping> subclassWriter, IHbmWriter<DiscriminatorMapping> discriminatorWriter, IHbmWriter<ComponentMapping> componentWriter)
        {
            _identityWriter = identityWriter;
            _discriminatorWriter = discriminatorWriter;
            _subclassWriter = subclassWriter;

            _mappedMemberHelper = new HbmMappedMemberWriterHelper(collectionWriter, propertyWriter, manyToOneWriter, componentWriter);
        }

        public object Write(ClassMapping mapping)
        {
            _hbm = null;
            mapping.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            _hbm = new HbmClass();
            _hbm.name = classMapping.Name;

            if (classMapping.Attributes.IsSpecified(x => x.Tablename))
                _hbm.table = classMapping.Tablename;
        }

        public override void Visit(IIdentityMapping idMapping)
        {
            object idHbm = _identityWriter.Write(idMapping);
            _hbm.SetId(idHbm);
        }

        public override void Visit(ICollectionMapping collectionMapping)
        {
            object collectionHbm = _mappedMemberHelper.Write(collectionMapping);
            collectionHbm.AddTo(ref _hbm.Items);
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            object propertyHbm = _mappedMemberHelper.Write(propertyMapping);
            propertyHbm.AddTo(ref _hbm.Items);
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            object manyHbm = _mappedMemberHelper.Write(manyToOneMapping);
            manyHbm.AddTo(ref _hbm.Items);
        }

        public override void Visit(ComponentMapping componentMapping)
        {
            object componentHbm = _mappedMemberHelper.Write(componentMapping);
            componentHbm.AddTo(ref _hbm.Items);
        }

        public override void Visit(ISubclassMapping subclassMapping)
        {
            object subclassHbm = _subclassWriter.Write(subclassMapping);
            subclassHbm.AddTo(ref _hbm.Items1);
        }

        public override void Visit(DiscriminatorMapping discriminatorMapping)
        {
            _hbm.discriminator = (HbmDiscriminator) _discriminatorWriter.Write(discriminatorMapping);
        }

        

    }
}
