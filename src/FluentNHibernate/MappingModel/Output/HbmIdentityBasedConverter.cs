using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Visitors;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIdentityBasedConverter : HbmConverterBase<IIdentityMapping, object>
    {
        /*
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;
        */
        private object hbm;

        public HbmIdentityBasedConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override object Convert(IIdentityMapping identityMapping)
        {
            hbm = new object(); // FIXME: Dummy to avoid build failure, remove!
            identityMapping.AcceptVisitor(this);
            return hbm;
        }

        /*
        public override void ProcessId(IdMapping mapping)
        {
            var writer = serviceLocator.GetWriter<IdMapping>();
            document = writer.Write(mapping);
        }

        public override void ProcessCompositeId(CompositeIdMapping idMapping)
        {
            var writer = serviceLocator.GetWriter<CompositeIdMapping>();
            document = writer.Write(idMapping);
        }
        */
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