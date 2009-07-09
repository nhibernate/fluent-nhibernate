using System;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class RelationshipInstance : IRelationshipInstance
    {
        protected readonly ICollectionRelationshipMapping mapping;

        public RelationshipInstance(ICollectionRelationshipMapping mapping)
        {
            this.mapping = mapping;
        }

        public Type EntityType
        {
            get { throw new NotImplementedException(); }
        }
        public string StringIdentifierForModel
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsSet(PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        /*public IDefaultableEnumerable<IColumnInstance> Columns
        {
            get { throw new NotImplementedException(); }
        }*/
    }
}