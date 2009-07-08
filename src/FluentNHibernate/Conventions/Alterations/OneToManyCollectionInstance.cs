using System;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Alterations
{
    public class OneToManyCollectionInstance : OneToManyCollectionInspector, IOneToManyCollectionInstance
    {
        private readonly ICollectionMapping mapping;

        public OneToManyCollectionInstance(ICollectionMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        IKeyAlteration ICollectionAlteration.Key
        {
            get { return Key; }
        }
        public IOneToManyInstance OneToMany
        {
            get { throw new NotImplementedException(); }
        }
        public new IKeyInstance Key
        {
            get { return new KeyInstance(mapping.Key); }
        }
        public IOneToManyAlteration Relationship
        {
            get { throw new NotImplementedException(); }
        }
        IRelationshipAlteration ICollectionAlteration.Relationship
        {
            get { return Relationship; }
        }
        public void SetTableName(string tableName)
        {
            throw new NotImplementedException();
        }

        public void Name(string name)
        {
            mapping.Name = name;
        }
    }
}