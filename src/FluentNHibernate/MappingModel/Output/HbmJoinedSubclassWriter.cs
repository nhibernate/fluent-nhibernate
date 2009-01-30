using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmJoinedSubclassWriter : NullMappingModelVisitor, IHbmWriter<JoinedSubclassMapping>
    {
        private readonly IHbmWriter<ICollectionMapping> _collectionWriter;
        private readonly IHbmWriter<PropertyMapping> _propertyWriter;
        private readonly IHbmWriter<ManyToOneMapping> _manyToOneWriter;
        private readonly IHbmWriter<KeyMapping> _keyWriter;

        private HbmJoinedSubclass _hbm;

        public HbmJoinedSubclassWriter(IHbmWriter<ICollectionMapping> collectionWriter, IHbmWriter<PropertyMapping> propertyWriter, IHbmWriter<ManyToOneMapping> manyToOneWriter, IHbmWriter<KeyMapping> keyWriter)
        {
            _collectionWriter = collectionWriter;
            _keyWriter = keyWriter;
            _manyToOneWriter = manyToOneWriter;
            _propertyWriter = propertyWriter;
        }

        public object Write(JoinedSubclassMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping)
        {
            _hbm = new HbmJoinedSubclass();
            _hbm.name = subclassMapping.Name;
        }

        public override void Visit(KeyMapping keyMapping)
        {
             _hbm.key = (HbmKey) _keyWriter.Write(keyMapping);
        }

        public override void Visit(JoinedSubclassMapping subclassMapping)
        {
            var writer = new HbmJoinedSubclassWriter(_collectionWriter, _propertyWriter, _manyToOneWriter, _keyWriter);
            var joinedSubclassHbm = (HbmJoinedSubclass)writer.Write(subclassMapping);
            joinedSubclassHbm.AddTo(ref _hbm.joinedsubclass1);
        }
    }
}