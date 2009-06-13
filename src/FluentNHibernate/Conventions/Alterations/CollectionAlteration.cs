using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Alterations
{
    public class CollectionAlteration : ICollectionAlteration
    {
        private readonly ICollectionMapping mapping;

        public CollectionAlteration(ICollectionMapping mapping)
        {
            this.mapping = mapping;
        }

        public IKeyAlteration Key
        {
            get { return new KeyAlteration(mapping.Key); }
        }

        public void TableName(string tableName)
        {
            mapping.TableName = tableName;
        }

        public IManyToManyAlteration ManyToMany
        {
            get
            {
                if (mapping.Relationship is ManyToManyMapping)
                    return new ManyToManyAlteration((ManyToManyMapping)mapping.Relationship);

                // dummy alteration, won't actually apply to anything
                return new ManyToManyAlteration(new ManyToManyMapping());
            }
        }

        public IOneToManyAlteration OneToMany
        {
            get
            {
                if (mapping.Relationship is OneToManyMapping)
                    return new OneToManyAlteration((OneToManyMapping)mapping.Relationship);

                // dummy alteration, won't actually apply to anything
                return new OneToManyAlteration(new OneToManyMapping());
            }
        }
    }
}