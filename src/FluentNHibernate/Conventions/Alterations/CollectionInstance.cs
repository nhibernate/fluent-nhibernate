using System;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Alterations
{
    public class CollectionInstance : CollectionInspector, ICollectionInstance
    {
        private readonly ICollectionMapping mapping;

        public CollectionInstance(ICollectionMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IKeyInstance Key
        {
            get { return new KeyInstance(mapping.Key); }
        }
        IKeyAlteration ICollectionAlteration.Key
        {
            get { return Key; }
        }
        IOneToManyAlteration IOneToManyCollectionAlteration.OneToMany
        {
            get { return OneToMany; }
        }
        IManyToManyAlteration IManyToManyCollectionAlteration.ManyToMany
        {
            get { return ManyToMany; }
        }

        public new void SetTableName(string tableName)
        {
            mapping.TableName = tableName;
        }

        public void Name(string name)
        {
            mapping.Name = name;
        }

        IKeyAlteration IManyToManyCollectionAlteration.Key
        {
            get { return Key; }
        }
        public new IManyToManyInstance ManyToMany
        {
            get
            {
                if (mapping.Relationship is ManyToManyMapping)
                    return new ManyToManyInstance((ManyToManyMapping)mapping.Relationship);

                // dummy alteration, won't actually apply to anything
                return new ManyToManyInstance(new ManyToManyMapping());
            }
        }

        IKeyAlteration IOneToManyCollectionAlteration.Key
        {
            get { return Key; }
        }
        public new IOneToManyInstance OneToMany
        {
            get
            {
                if (mapping.Relationship is OneToManyMapping)
                    return new OneToManyInstance((OneToManyMapping)mapping.Relationship);

                // dummy alteration, won't actually apply to anything
                return new OneToManyInstance(new OneToManyMapping());
            }
        }
    }
}