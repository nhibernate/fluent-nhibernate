using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIdConverter : HbmConverterBase<IdMapping, HbmId>
    {
        private HbmId hbmId;

        public HbmIdConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmId Convert(IdMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmId;
        }

        public override void ProcessId(IdMapping idMapping)
        {
            hbmId = new HbmId();

            if (idMapping.IsSpecified("Access"))
                hbmId.access = idMapping.Access;

            if (idMapping.IsSpecified("Name"))
                hbmId.name = idMapping.Name;

            if (idMapping.IsSpecified("Type"))
                hbmId.type = idMapping.Type.ToHbmType();

            if (idMapping.IsSpecified("UnsavedValue"))
                hbmId.unsavedvalue = idMapping.UnsavedValue;
        }

        public override void Visit(GeneratorMapping generatorMapping)
        {
            hbmId.generator = ConvertFluentSubobjectToHibernateNative<GeneratorMapping, HbmGenerator>(generatorMapping);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmId.column, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }
    }
}