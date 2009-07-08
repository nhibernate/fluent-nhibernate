using System;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Alterations
{
    public class ManyToManyCollectionInstance : ManyToManyCollectionInspector, IManyToManyCollectionInstance
    {
        private readonly ICollectionMapping mapping;

        public ManyToManyCollectionInstance(ICollectionMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        IKeyAlteration ICollectionAlteration.Key
        {
            get { return Key; }
        }
        IManyToManyAlteration IManyToManyCollectionAlteration.Relationship
        {
            get { return Relationship; }
        }
        IRelationshipAlteration ICollectionAlteration.Relationship
        {
            get { return Relationship; }
        }
        public IManyToManyInstance Relationship
        {
            get { return new ManyToManyInstance((ManyToManyMapping)mapping.Relationship); }
        }
        public new IKeyInstance Key
        {
            get { return new KeyInstance(mapping.Key); }
        }
        public void SetTableName(string tableName)
        {
            mapping.TableName = tableName;
        }
        public void Name(string name)
        {
            throw new NotImplementedException();
        }
    }
}