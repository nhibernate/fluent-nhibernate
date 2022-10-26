using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIndexBasedConverter : HbmConverterBase<IIndexMapping, object>
    {
        private object hbm;

        public HbmIndexBasedConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override object Convert(IIndexMapping indexMapping)
        {
            indexMapping.AcceptVisitor(this);
            return hbm;
        }

        public override void ProcessIndex(IndexMapping indexMapping)
        {
            if (indexMapping.IsSpecified("Offset"))
            {
                hbm = ConvertFluentSubobjectToHibernateNative<IndexMapping, HbmListIndex>(indexMapping);
            }
            else
            {
                hbm = ConvertFluentSubobjectToHibernateNative<IndexMapping, HbmIndex>(indexMapping);
            }
        }

        public override void ProcessIndex(IndexManyToManyMapping indexMapping)
        {
            hbm = ConvertFluentSubobjectToHibernateNative<IndexManyToManyMapping, HbmIndexManyToMany>(indexMapping);
        }
    }
}