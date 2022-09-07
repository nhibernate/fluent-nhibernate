using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmPropertyConverter : HbmConverterBase<PropertyMapping, HbmProperty>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmPropertyGeneration> genDict = new XmlLinkedEnumBiDictionary<HbmPropertyGeneration>();

        private HbmProperty hbmProperty;

        public HbmPropertyConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmProperty Convert(PropertyMapping propertyMapping)
        {
            propertyMapping.AcceptVisitor(this);
            return hbmProperty;
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            hbmProperty = new HbmProperty();

            if (propertyMapping.IsSpecified("Access"))
                hbmProperty.access = propertyMapping.Access;

            if (propertyMapping.IsSpecified("Generated"))
                hbmProperty.generated = LookupEnumValueIn(genDict, propertyMapping.Generated);

            if (propertyMapping.IsSpecified("Name"))
                hbmProperty.name = propertyMapping.Name;

            if (propertyMapping.IsSpecified("OptimisticLock"))
                hbmProperty.optimisticlock = propertyMapping.OptimisticLock;

            bool insertSpecified = propertyMapping.IsSpecified("Insert");
            hbmProperty.insertSpecified = insertSpecified;
            if (insertSpecified)
                hbmProperty.insert = propertyMapping.Insert;

            bool updateSpecified = propertyMapping.IsSpecified("Update");
            hbmProperty.updateSpecified = updateSpecified;
            if (updateSpecified)
                hbmProperty.update = propertyMapping.Update;

            if (propertyMapping.IsSpecified("Formula"))
                hbmProperty.formula = propertyMapping.Formula;

            if (propertyMapping.IsSpecified("Type"))
                hbmProperty.type1 = propertyMapping.Type.ToString();

            if (propertyMapping.IsSpecified("Lazy"))
                hbmProperty.lazy = propertyMapping.Lazy;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmProperty.Items, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }
    }
}