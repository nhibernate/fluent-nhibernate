using FluentNHibernate.Mapping;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmDiscriminatorConverter : HbmConverterBase<DiscriminatorMapping, HbmDiscriminator>
    {
        private HbmDiscriminator hbmDiscriminator;

        public HbmDiscriminatorConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmDiscriminator Convert(DiscriminatorMapping discriminatorMapping)
        {
            discriminatorMapping.AcceptVisitor(this);
            return hbmDiscriminator;
        }

        public override void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping)
        {
            hbmDiscriminator = new HbmDiscriminator();

            if (discriminatorMapping.IsSpecified("Type"))
                hbmDiscriminator.type = TypeMapping.GetTypeString(discriminatorMapping.Type.GetUnderlyingSystemType());

            if (discriminatorMapping.IsSpecified("Force"))
                hbmDiscriminator.force = discriminatorMapping.Force;

            if (discriminatorMapping.IsSpecified("Formula"))
                hbmDiscriminator.formula = discriminatorMapping.Formula;

            if (discriminatorMapping.IsSpecified("Insert"))
                hbmDiscriminator.insert = discriminatorMapping.Insert;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            hbmDiscriminator.Item = ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping);
        }
    }
}