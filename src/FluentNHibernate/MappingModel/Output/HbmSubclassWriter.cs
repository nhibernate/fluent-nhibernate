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
        private HbmJoinedSubclass _hbm;

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
    }
}