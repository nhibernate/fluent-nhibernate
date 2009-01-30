using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
namespace FluentNHibernate.MappingModel.Output
{
    public class HbmSubclassWriter : MappingModelVisitorBase, IHbmWriter<ISubclassMapping>
    {
        private readonly IHbmWriter<JoinedSubclassMapping> _joinedSubClassWriter;

        private object _hbm;

        public HbmSubclassWriter(IHbmWriter<JoinedSubclassMapping> joinedSubClassWriter)
        {
            _joinedSubClassWriter = joinedSubClassWriter;
        }

        public object Write(ISubclassMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping)
        {
            _hbm = _joinedSubClassWriter.Write(subclassMapping);
        }
    }

    public class HbmJoinedSubclassWriter : MappingModelVisitorBase, IHbmWriter<JoinedSubclassMapping>
    {
        private readonly IHbmWriter<ICollectionMapping> _collectionWriter;
        private readonly IHbmWriter<PropertyMapping> _propertyWriter;
        private readonly IHbmWriter<ManyToOneMapping> _manyToOneWriter;

        private HbmJoinedSubclass _hbm;

        public HbmJoinedSubclassWriter()
        {
            
        }

        public HbmJoinedSubclassWriter(IHbmWriter<ICollectionMapping> collectionWriter, IHbmWriter<PropertyMapping> propertyWriter, IHbmWriter<ManyToOneMapping> manyToOneWriter)
        {
            _collectionWriter = collectionWriter;
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

        //public override void Visit(JoinedSubclassMapping subclassMapping)
        //{
        //    var writer = new HbmJoinedSubclassWriter(_collectionWriter, _propertyWriter, _manyToOneWriter);
        //    var joinedSubclassHbm = (HbmJoinedSubclass) writer.Write(subclassMapping);
        //    joinedSubclassHbm.AddTo(ref _hbm.joinedsubclass1);
        //}
    }
}