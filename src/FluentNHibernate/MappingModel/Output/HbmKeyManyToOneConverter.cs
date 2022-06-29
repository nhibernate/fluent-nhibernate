using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmKeyManyToOneConverter : HbmConverterBase<KeyManyToOneMapping, HbmKeyManyToOne>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmNotFoundMode> notFoundDict = new XmlLinkedEnumBiDictionary<HbmNotFoundMode>();

        private HbmKeyManyToOne hbmKeyManyToOne;

        public HbmKeyManyToOneConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmKeyManyToOne Convert(KeyManyToOneMapping keyManyToOneMapping)
        {
            keyManyToOneMapping.AcceptVisitor(this);
            return hbmKeyManyToOne;
        }

        public override void ProcessKeyManyToOne(KeyManyToOneMapping keyManyToOneMapping)
        {
            hbmKeyManyToOne = new HbmKeyManyToOne();

            if (keyManyToOneMapping.IsSpecified("Access"))
                hbmKeyManyToOne.access = keyManyToOneMapping.Access;

            if (keyManyToOneMapping.IsSpecified("Name"))
                hbmKeyManyToOne.name = keyManyToOneMapping.Name;

            if (keyManyToOneMapping.IsSpecified("Class"))
                hbmKeyManyToOne.@class = keyManyToOneMapping.Class.ToString();

            if (keyManyToOneMapping.IsSpecified("ForeignKey"))
                hbmKeyManyToOne.foreignkey = keyManyToOneMapping.ForeignKey;

            bool lazySpecified = keyManyToOneMapping.IsSpecified("Lazy");
            hbmKeyManyToOne.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmKeyManyToOne.lazy = keyManyToOneMapping.Lazy ? HbmRestrictedLaziness.Proxy : HbmRestrictedLaziness.False;

            if (keyManyToOneMapping.IsSpecified("NotFound"))
                hbmKeyManyToOne.notfound = LookupEnumValueIn(notFoundDict, keyManyToOneMapping.NotFound);

            if (keyManyToOneMapping.IsSpecified("EntityName"))
                hbmKeyManyToOne.entityname = keyManyToOneMapping.EntityName;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmKeyManyToOne.column, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }
    }
}