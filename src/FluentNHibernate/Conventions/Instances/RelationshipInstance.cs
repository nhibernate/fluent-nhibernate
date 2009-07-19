using System;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class RelationshipInstance : RelationshipInspector, IRelationshipInstance
    {
        private readonly ICollectionRelationshipMapping mapping;

        public RelationshipInstance(ICollectionRelationshipMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }
    }
}