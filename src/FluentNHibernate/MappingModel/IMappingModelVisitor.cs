using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel
{
    public interface IMappingModelVisitor
    {
        void ProcessIdentity(IIdentityMapping idMapping);
        void ProcessId(IdMapping idMapping);
        void ProcessCompositeId(CompositeIdMapping idMapping);
        void ProcessClass(ClassMapping classMapping);
        void ProcessHibernateMapping(HibernateMapping hibernateMapping);
        void ProcessProperty(PropertyMapping propertyMapping);
        void ProcessCollection(ICollectionMapping collectionMapping);
        void ProcessManyToOne(ManyToOneMapping  manyToOneMapping);
        void ProcessKey(KeyMapping keyMapping);
        void ProcessIdGenerator(IdGeneratorMapping generatorMapping);
        void ProcessColumn(ColumnMapping columnMapping);
        void ProcessBag(BagMapping bagMapping);
        void ProcessOneToMany(OneToManyMapping oneToManyMapping);
        void ProcessSet(SetMapping setMapping);
        void ProcessCollectionContents(ICollectionContentsMapping contentsMapping);
    }
}