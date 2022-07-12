using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;
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
            hbm = ConvertFluentSubobjectToHibernateNative<IndexMapping, HbmIndex>(indexMapping);
        }

        public override void ProcessIndex(IndexManyToManyMapping indexMapping)
        {
            hbm = ConvertFluentSubobjectToHibernateNative<IndexManyToManyMapping, HbmIndexManyToMany>(indexMapping);
        }
    }
}