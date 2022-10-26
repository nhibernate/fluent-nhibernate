using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmElementConverter : HbmConverterBase<ElementMapping, HbmElement>
    {
        private HbmElement hbmElement;

        public HbmElementConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmElement Convert(ElementMapping elementMapping)
        {
            elementMapping.AcceptVisitor(this);
            return hbmElement;
        }

        public override void ProcessElement(ElementMapping elementMapping)
        {
            hbmElement = new HbmElement();

            if (elementMapping.IsSpecified("Type"))
                hbmElement.type1 = elementMapping.Type.ToString();

            if (elementMapping.IsSpecified("Formula"))
                hbmElement.formula = elementMapping.Formula;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmElement.Items, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }
    }
}