using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default HasManyToMany table name convention
    /// </summary>
    public class HasManyToManyJoinTableNameConvention : IHasManyToManyConvention
    {
        public void Accept(IAcceptanceCriteria<IManyToManyCollectionInspector> acceptance)
        {
            acceptance.Expect(x => x.TableName, Is.Not.Set);
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.SetTableName(instance.ChildType.Name + "To" + instance.EntityType.Name);
        }
    }
}