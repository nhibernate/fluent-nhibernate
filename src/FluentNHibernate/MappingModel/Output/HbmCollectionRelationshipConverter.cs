using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCollectionRelationshipConverter : HbmConverterBase<ICollectionRelationshipMapping, object>
    {
        private object hbmCollectionRelationship;

        public HbmCollectionRelationshipConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override object Convert(ICollectionRelationshipMapping collectionRelationshipMapping)
        {
            collectionRelationshipMapping.AcceptVisitor(this);
            return hbmCollectionRelationship;
        }

        public override void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {
            hbmCollectionRelationship = ConvertFluentSubobjectToHibernateNative<OneToManyMapping, HbmOneToMany>(oneToManyMapping);
        }

        public override void ProcessManyToMany(ManyToManyMapping manyToManyMapping)
        {
            hbmCollectionRelationship = ConvertFluentSubobjectToHibernateNative<ManyToManyMapping, HbmManyToMany>(manyToManyMapping);
        }
    }
}