using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;
using System;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmClassWriter : MappingModelVisitorBase, IHbmWriter<ClassMapping>
    {
        private readonly IHbmWriter<IIdentityMapping> _identityWriter;
        private readonly IHbmWriter<ICollectionMapping> _collectionWriter;
        private readonly IHbmWriter<PropertyMapping> _propertyWriter;
        private readonly IHbmWriter<ManyToOneMapping> _manyToOneWriter;
        private HbmClass _hbmClass;

        public HbmClassWriter(
            IHbmWriter<IIdentityMapping> identityWriter,
            IHbmWriter<ICollectionMapping> collectionWriter,
            IHbmWriter<PropertyMapping> propertyWriter,
            IHbmWriter<ManyToOneMapping> manyToOneWriter
            )
        {
            _identityWriter = identityWriter;
            _collectionWriter = collectionWriter;
            _propertyWriter = propertyWriter;
            _manyToOneWriter = manyToOneWriter;
        }

        public object Write(ClassMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return _hbmClass;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            _hbmClass = new HbmClass();
            _hbmClass.name = classMapping.Name;
        	_hbmClass.table = classMapping.Tablename;
        }

        public override void ProcessIdentity(IIdentityMapping idMapping)
        {
            object idHbm = _identityWriter.Write(idMapping);
            _hbmClass.SetId(idHbm);
        }

        public override void ProcessCollection(ICollectionMapping collectionMapping)
        {            
            object collectionHbm = _collectionWriter.Write(collectionMapping);
            collectionHbm.AddTo(ref _hbmClass.Items);
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            object propertyHbm = _propertyWriter.Write(propertyMapping);
            propertyHbm.AddTo(ref _hbmClass.Items);
        }

        public override void ProcessManyToOne(ManyToOneMapping manyToOneMapping)
        {
            object manyHbm = _manyToOneWriter.Write(manyToOneMapping);
            manyHbm.AddTo(ref _hbmClass.Items);
        }
    }
}