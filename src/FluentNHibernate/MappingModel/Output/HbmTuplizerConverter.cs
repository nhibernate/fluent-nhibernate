using System.Collections.Generic;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmTuplizerConverter : HbmConverterBase<TuplizerMapping, HbmTuplizer>
    {
        public static readonly EnumBiDictionary<HbmTuplizerEntitymode, TuplizerMode> FluentHbmEntityModeBiDict = new EnumBiDictionary<HbmTuplizerEntitymode, TuplizerMode>(
            new Dictionary<HbmTuplizerEntitymode, TuplizerMode>() {
                { HbmTuplizerEntitymode.Poco, TuplizerMode.Poco },
                { HbmTuplizerEntitymode.DynamicMap, TuplizerMode.DynamicMap },
                //{ <does not exist>, TuplizerMode.Xml },
            }
        );

        private HbmTuplizer hbmTuplizer;

        public HbmTuplizerConverter() : base(null)
        {
        }

        public override HbmTuplizer Convert(TuplizerMapping tuplizerMapping)
        {
            tuplizerMapping.AcceptVisitor(this);
            return hbmTuplizer;
        }

        public override void ProcessTuplizer(TuplizerMapping tuplizerMapping)
        {
            hbmTuplizer = new HbmTuplizer();

            bool modeSpecified = tuplizerMapping.IsSpecified("Mode");
            hbmTuplizer.entitymodeSpecified = modeSpecified;
            if (modeSpecified)
                hbmTuplizer.entitymode = LookupEnumValueIn(FluentHbmEntityModeBiDict, tuplizerMapping.Mode);

            if (tuplizerMapping.IsSpecified("Type"))
                hbmTuplizer.@class = tuplizerMapping.Type.ToString();

            // The XML variant of this logic supports an EntityName attribute, but that does not appear in either HbmTuplizer or
            // the XSD, so it appears to be an error. Since we have nowhere to write it, just silently ignore it, even if it is
            // set.
        }
    }
}