using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmJoinedSubclassWriter : NullMappingModelVisitor, IXmlWriter<JoinedSubclassMapping>
    {
        private readonly IXmlWriter<KeyMapping> _keyWriter;
        private readonly HbmMappedMemberWriterHelper _mappedMemberHelper;

        private HbmJoinedSubclass _hbm;

        private HbmJoinedSubclassWriter(HbmMappedMemberWriterHelper helper, IXmlWriter<KeyMapping> keyWriter)
        {
            _mappedMemberHelper = helper;
            _keyWriter = keyWriter;
        }

        public HbmJoinedSubclassWriter(IXmlWriter<ICollectionMapping> collectionWriter, IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<ManyToOneMapping> manyToOneWriter, IXmlWriter<ComponentMapping> componentWriter, IXmlWriter<KeyMapping> keyWriter)
            : this(new HbmMappedMemberWriterHelper(collectionWriter, propertyWriter, manyToOneWriter, componentWriter), keyWriter)
        { }

        public object Write(JoinedSubclassMapping mappingModel)
        {
            _hbm = null;
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
            _hbm.key = (HbmKey)_keyWriter.Write(keyMapping);
        }

        public override void Visit(JoinedSubclassMapping subclassMapping)
        {
            var writer = new HbmJoinedSubclassWriter(_mappedMemberHelper,_keyWriter);
            var joinedSubclassHbm = (HbmJoinedSubclass)writer.Write(subclassMapping);
            joinedSubclassHbm.AddTo(ref _hbm.joinedsubclass1);
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
    }
}