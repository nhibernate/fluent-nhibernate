using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIndexConverter : HbmConverterBase<IndexMapping, HbmIndex>
    {
        private HbmIndex hbmIndex;

        public HbmIndexConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmIndex Convert(IndexMapping indexMapping)
        {
            indexMapping.AcceptVisitor(this);
            return hbmIndex;
        }

        public override void ProcessIndex(IndexMapping indexMapping)
        {
            hbmIndex = new HbmIndex();

            if (indexMapping.IsSpecified("Type"))
                hbmIndex.type = indexMapping.Type.ToString();
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmIndex.column, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }
    }
}