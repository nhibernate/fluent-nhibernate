using FluentNHibernate.MappingModel.ClassBased;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmHibernateMappingConverter : HbmConverterBase<HibernateMapping, HbmMapping>
    {
        private HbmMapping hbmMapping;

        public HbmHibernateMappingConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmMapping Convert(HibernateMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmMapping;
        }

        public override void ProcessHibernateMapping(HibernateMapping mapping)
        {
            hbmMapping = new HbmMapping();

            if (mapping.IsSpecified("DefaultAccess"))
                hbmMapping.defaultaccess = mapping.DefaultAccess;

            if (mapping.IsSpecified("AutoImport"))
                hbmMapping.autoimport = mapping.AutoImport;

            if (mapping.IsSpecified("Schema"))
                hbmMapping.schema = mapping.Schema;

            if (mapping.IsSpecified("DefaultCascade"))
                hbmMapping.defaultcascade = mapping.DefaultCascade;

            if (mapping.IsSpecified("DefaultLazy"))
                hbmMapping.defaultlazy = mapping.DefaultLazy;

            if (mapping.IsSpecified("Catalog"))
                hbmMapping.catalog = mapping.Catalog;

            if (mapping.IsSpecified("Namespace"))
                hbmMapping.@namespace = mapping.Namespace;

            if (mapping.IsSpecified("Assembly"))
                hbmMapping.assembly = mapping.Assembly;
        }

        public override void Visit(ImportMapping importMapping)
        {
            AddToNullableArray(ref hbmMapping.import, ConvertFluentSubobjectToHibernateNative<ImportMapping, HbmImport>(importMapping));
        }

        public override void Visit(ClassMapping classMapping)
        {
            AddToNullableArray(ref hbmMapping.Items, ConvertFluentSubobjectToHibernateNative<ClassMapping, HbmClass>(classMapping));
        }

        public override void Visit(FilterDefinitionMapping filterDefinitionMapping)
        {
            AddToNullableArray(ref hbmMapping.filterdef, ConvertFluentSubobjectToHibernateNative<FilterDefinitionMapping, HbmFilterDef>(filterDefinitionMapping));
        }
    }
}