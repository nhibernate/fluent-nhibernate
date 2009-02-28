using System;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmSubclassWriter : NullMappingModelVisitor, IHbmWriter<SubclassMapping>
    {
        private readonly IHbmWriter<ICollectionMapping> _collectionWriter;
        private readonly IHbmWriter<PropertyMapping> _propertyWriter;
        private readonly IHbmWriter<ManyToOneMapping> _manyToOneWriter;

        private HbmSubclass _hbm;

        public HbmSubclassWriter(IHbmWriter<ICollectionMapping> collectionWriter, IHbmWriter<PropertyMapping> propertyWriter, IHbmWriter<ManyToOneMapping> manyToOneWriter)
        {
            _collectionWriter = collectionWriter;
            _manyToOneWriter = manyToOneWriter;
            _propertyWriter = propertyWriter;
        }

        public object Write(SubclassMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            _hbm = new HbmSubclass();
            _hbm.name = subclassMapping.Name;

            if (subclassMapping.Attributes.IsSpecified(x => x.DiscriminatorValue))
                _hbm.discriminatorvalue = subclassMapping.DiscriminatorValue.ToString();
        }

        public override void Visit(SubclassMapping subclassMapping)
        {
            var writer = new HbmSubclassWriter(_collectionWriter, _propertyWriter, _manyToOneWriter);
            var subclassHbm = (HbmSubclass)writer.Write(subclassMapping);
            subclassHbm.AddTo(ref _hbm.subclass1);
        }

        public override void Visit(ICollectionMapping collectionMapping)
        {
            object collectionHbm = _collectionWriter.Write(collectionMapping);
            collectionHbm.AddTo(ref _hbm.Items);
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            object propertyHbm = _propertyWriter.Write(propertyMapping);
            propertyHbm.AddTo(ref _hbm.Items);
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            object manyHbm = _manyToOneWriter.Write(manyToOneMapping);
            manyHbm.AddTo(ref _hbm.Items);
        }
    }
}