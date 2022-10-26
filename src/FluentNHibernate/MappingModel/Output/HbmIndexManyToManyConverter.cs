using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIndexManyToManyConverter : HbmConverterBase<IndexManyToManyMapping, HbmIndexManyToMany>
    {
        private HbmIndexManyToMany hbmIndexManyToMany;

        public HbmIndexManyToManyConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmIndexManyToMany Convert(IndexManyToManyMapping indexManyToManyMapping)
        {
            indexManyToManyMapping.AcceptVisitor(this);
            return hbmIndexManyToMany;
        }

        public override void ProcessIndex(IndexManyToManyMapping indexManyToManyMapping)
        {
            hbmIndexManyToMany = new HbmIndexManyToMany();

            if (indexManyToManyMapping.IsSpecified("Class"))
                hbmIndexManyToMany.@class = indexManyToManyMapping.Class.ToString();

            if (indexManyToManyMapping.IsSpecified("ForeignKey"))
                hbmIndexManyToMany.foreignkey = indexManyToManyMapping.ForeignKey;

            if (indexManyToManyMapping.IsSpecified("EntityName"))
                hbmIndexManyToMany.entityname = indexManyToManyMapping.EntityName;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmIndexManyToMany.column, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }
    }
}