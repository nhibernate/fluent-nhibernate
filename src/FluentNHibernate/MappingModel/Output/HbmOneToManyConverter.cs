using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmOneToManyConverter : HbmConverterBase<OneToManyMapping, HbmOneToMany>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmNotFoundMode> notFoundDict = new XmlLinkedEnumBiDictionary<HbmNotFoundMode>();

        private HbmOneToMany hbmOneToMany;

        public HbmOneToManyConverter() : base(null)
        {
        }

        public override HbmOneToMany Convert(OneToManyMapping oneToManyMapping)
        {
            oneToManyMapping.AcceptVisitor(this);
            return hbmOneToMany;
        }

        public override void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {
            hbmOneToMany = new HbmOneToMany();

            if (oneToManyMapping.IsSpecified("Class"))
                hbmOneToMany.@class = oneToManyMapping.Class.ToString();

            if (oneToManyMapping.IsSpecified("NotFound"))
                hbmOneToMany.notfound = LookupEnumValueIn(notFoundDict, oneToManyMapping.NotFound);

            if (oneToManyMapping.IsSpecified("EntityName"))
                hbmOneToMany.entityname = oneToManyMapping.EntityName;
        }
    }
}