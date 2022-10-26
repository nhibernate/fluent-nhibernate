using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmImportConverter : HbmConverterBase<ImportMapping, HbmImport>
    {
        private HbmImport hbmImport;

        public HbmImportConverter() : base(null)
        {
        }

        public override HbmImport Convert(ImportMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmImport;
        }

        public override void ProcessImport(ImportMapping mapping)
        {
            hbmImport = new HbmImport();

            if (mapping.IsSpecified("Class"))
                hbmImport.@class = mapping.Class.ToString();

            if (mapping.IsSpecified("Rename"))
                hbmImport.rename = mapping.Rename;
        }
    }
}