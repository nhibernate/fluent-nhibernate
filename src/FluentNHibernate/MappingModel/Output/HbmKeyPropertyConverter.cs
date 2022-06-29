using System.Linq;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmKeyPropertyConverter : HbmConverterBase<KeyPropertyMapping, HbmKeyProperty>
    {
        /*
        private static readonly XmlLinkedEnumBiDictionary<HbmUnsavedValueType> unsavedDict = new XmlLinkedEnumBiDictionary<HbmUnsavedValueType>();
        */

        private HbmKeyProperty hbmKeyProperty;

        public HbmKeyPropertyConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmKeyProperty Convert(KeyPropertyMapping keyPropertyMapping)
        {
            keyPropertyMapping.AcceptVisitor(this);
            return hbmKeyProperty;
        }

        public override void ProcessKeyProperty(KeyPropertyMapping keyPropertyMapping)
        {
            hbmKeyProperty = new HbmKeyProperty();

            if (keyPropertyMapping.IsSpecified("Name"))
                hbmKeyProperty.name = keyPropertyMapping.Name;


            if (keyPropertyMapping.IsSpecified("Access"))
                hbmKeyProperty.access = keyPropertyMapping.Access;

            if (keyPropertyMapping.IsSpecified("Type"))
                hbmKeyProperty.type = ToHbmType(keyPropertyMapping.Type);

            if (keyPropertyMapping.IsSpecified("Length"))
            {
                if (keyPropertyMapping.Columns.Any())
                {
                    foreach (var columnMapping in keyPropertyMapping.Columns.Where(column => !column.IsSpecified("Length")))
                    {
                        columnMapping.Set(map => map.Length, Layer.Defaults, keyPropertyMapping.Length);
                    }
                }
                else
                {
                    hbmKeyProperty.length = keyPropertyMapping.Length.ToString();
                }
            }
        }
        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmKeyProperty.column, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }

        private static HbmType ToHbmType(TypeReference typeReference)
        {
            return new HbmType()
            {
                name = typeReference.Name,
            };
        }
    }
}