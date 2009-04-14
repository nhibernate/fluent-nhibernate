namespace FluentNHibernate.MappingModel.Output
{
    public class HbmInheritanceWriter : NullMappingModelVisitor, IXmlWriter<ISubclassMapping>
    {
        private readonly IXmlWriter<JoinedSubclassMapping> _joinedSubClassWriter;
        private readonly IXmlWriter<SubclassMapping> _subClassWriter;
        private object _hbm;

        public HbmInheritanceWriter(IXmlWriter<JoinedSubclassMapping> joinedSubClassWriter, IXmlWriter<SubclassMapping> subClassWriter)
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