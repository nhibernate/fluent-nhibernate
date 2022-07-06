using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmKeyConverter : HbmConverterBase<KeyMapping, HbmKey>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmOndelete> ondeleteDict = new XmlLinkedEnumBiDictionary<HbmOndelete>();

        private HbmKey hbmKey;

        public HbmKeyConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmKey Convert(KeyMapping keyMapping)
        {
            keyMapping.AcceptVisitor(this);
            return hbmKey;
        }

        public override void ProcessKey(KeyMapping keyMapping)
        {
            hbmKey = new HbmKey();

            if (keyMapping.IsSpecified("ForeignKey"))
                hbmKey.foreignkey = keyMapping.ForeignKey;

            if (keyMapping.IsSpecified("OnDelete"))
                hbmKey.ondelete = LookupEnumValueIn(ondeleteDict, keyMapping.OnDelete);

            if (keyMapping.IsSpecified("PropertyRef"))
                hbmKey.propertyref = keyMapping.PropertyRef;

            bool notnullSpecified = keyMapping.IsSpecified("NotNull");
            hbmKey.notnullSpecified = notnullSpecified;
            if (notnullSpecified)
                hbmKey.notnull = keyMapping.NotNull;

            bool updateSpecified = keyMapping.IsSpecified("Update");
            hbmKey.updateSpecified = updateSpecified;
            if (updateSpecified)
                hbmKey.update = keyMapping.Update;

            bool uniqueSpecified = keyMapping.IsSpecified("Unique");
            hbmKey.uniqueSpecified = uniqueSpecified;
            if (uniqueSpecified)
                hbmKey.unique = keyMapping.Unique;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmKey.column, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }
    }
}