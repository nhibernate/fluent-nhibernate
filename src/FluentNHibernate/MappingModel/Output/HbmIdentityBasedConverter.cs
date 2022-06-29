using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Visitors;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIdentityBasedConverter : HbmConverterBase<IIdentityMapping, object>
    {
        private object hbm;

        public HbmIdentityBasedConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override object Convert(IIdentityMapping identityMapping)
        {
            identityMapping.AcceptVisitor(this);
            return hbm;
        }

        public override void ProcessId(IdMapping idMapping)
        {
            hbm = ConvertFluentSubobjectToHibernateNative<IdMapping, HbmId>(idMapping);
        }

        public override void ProcessCompositeId(CompositeIdMapping compositeIdMapping)
        {
            hbm = ConvertFluentSubobjectToHibernateNative<CompositeIdMapping, HbmCompositeId>(compositeIdMapping);
        }
    }
}