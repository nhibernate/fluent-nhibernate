using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmParentConverter : HbmConverterBase<ParentMapping, HbmParent>
    {
        private HbmParent hbmParent;

        public HbmParentConverter() : base(null)
        {
        }

        public override HbmParent Convert(ParentMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmParent;
        }

        public override void ProcessParent(ParentMapping parentMapping)
        {
            hbmParent = new HbmParent();

            if (parentMapping.IsSpecified("Name"))
                hbmParent.name = parentMapping.Name;
            
            if (parentMapping.IsSpecified("Access"))
                hbmParent.access = parentMapping.Access;
        }
    }
}