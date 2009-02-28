using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmComponentWriter : NullMappingModelVisitor, IHbmWriter<ComponentMapping>
    {
        private readonly IHbmWriter<IIdentityMapping> _identityWriter;
        private readonly IHbmWriter<ICollectionMapping> _collectionWriter;
        private readonly IHbmWriter<PropertyMapping> _propertyWriter;
        private readonly IHbmWriter<ManyToOneMapping> _manyToOneWriter;
        private HbmComponent _hbm;

        public HbmComponentWriter(IHbmWriter<IIdentityMapping> identityWriter, IHbmWriter<ICollectionMapping> collectionWriter, IHbmWriter<PropertyMapping> propertyWriter, IHbmWriter<ManyToOneMapping> manyToOneWriter)
        {
            _identityWriter = identityWriter;
            _manyToOneWriter = manyToOneWriter;
            _propertyWriter = propertyWriter;
            _collectionWriter = collectionWriter;
        }

        public object Write(ComponentMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessComponent(ComponentMapping componentMapping)
        {
            _hbm = new HbmComponent();
            _hbm.name = componentMapping.PropertyName;

            if(componentMapping.Attributes.IsSpecified(x => x.ClassName))
                _hbm.@class = componentMapping.ClassName;

        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            object hbmProperty = _propertyWriter.Write(propertyMapping);
            hbmProperty.AddTo(ref _hbm.Items);
        }

        public override void Visit(ICollectionMapping collectionMapping)
        {
            object hbmCollection = _collectionWriter.Write(collectionMapping);
            hbmCollection.AddTo(ref _hbm.Items);
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            object hbmManyToOne = _manyToOneWriter.Write(manyToOneMapping);
            hbmManyToOne.AddTo(ref _hbm.Items);
        }
    }
}
