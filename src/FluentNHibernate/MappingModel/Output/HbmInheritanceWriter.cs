namespace FluentNHibernate.MappingModel.Output
{
    public class HbmInheritanceWriter : NullMappingModelVisitor, IHbmWriter<ISubclassMapping>
    {
        private readonly IHbmWriter<JoinedSubclassMapping> _joinedSubClassWriter;
        private readonly IHbmWriter<SubclassMapping> _subClassWriter;
        private object _hbm;

        public HbmInheritanceWriter(IHbmWriter<JoinedSubclassMapping> joinedSubClassWriter, IHbmWriter<SubclassMapping> subClassWriter)
        {
            _joinedSubClassWriter = joinedSubClassWriter;
            _subClassWriter = subClassWriter;
        }

        public object Write(ISubclassMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping)
        {
            _hbm = _joinedSubClassWriter.Write(subclassMapping);
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            _hbm = _subClassWriter.Write(subclassMapping);
        }
    }
}