using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmListIndexConverter : HbmConverterBase<IndexMapping, HbmListIndex>
    {
        private HbmListIndex hbmListIndex;

        public HbmListIndexConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmListIndex Convert(IndexMapping indexMapping)
        {
            indexMapping.AcceptVisitor(this);
            return hbmListIndex;
        }

        public override void ProcessIndex(IndexMapping indexMapping)
        {
            hbmListIndex = new HbmListIndex();

            hbmListIndex.@base = indexMapping.Offset.ToString();
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            hbmListIndex.column = ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping);
        }
    }
}