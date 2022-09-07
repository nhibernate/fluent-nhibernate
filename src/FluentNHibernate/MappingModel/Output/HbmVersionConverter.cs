using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmVersionConverter : HbmConverterBase<VersionMapping, HbmVersion>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmVersionGeneration> genDict = new XmlLinkedEnumBiDictionary<HbmVersionGeneration>();

        private HbmVersion hbmVersion;

        public HbmVersionConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmVersion Convert(VersionMapping versionMapping)
        {
            versionMapping.AcceptVisitor(this);
            return hbmVersion;
        }

        public override void ProcessVersion(VersionMapping versionMapping)
        {
            hbmVersion = new HbmVersion();

            if (versionMapping.IsSpecified("Access"))
                hbmVersion.access = versionMapping.Access;

            if (versionMapping.IsSpecified("Generated"))
                hbmVersion.generated = LookupEnumValueIn(genDict, versionMapping.Generated);

            if (versionMapping.IsSpecified("Name"))
                hbmVersion.name = versionMapping.Name;

            if (versionMapping.IsSpecified("Type"))
                hbmVersion.type = versionMapping.Type.ToString();

            if (versionMapping.IsSpecified("UnsavedValue"))
                hbmVersion.unsavedvalue = versionMapping.UnsavedValue;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmVersion.column, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }
    }
}