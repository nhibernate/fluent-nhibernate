namespace FluentNHibernate.MappingModel.Output
{
    public class HbmSubclassWriter : NullMappingModelVisitor, IHbmWriter<ISubclassMapping>
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
}