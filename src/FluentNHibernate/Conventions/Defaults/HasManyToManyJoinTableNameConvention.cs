using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default HasManyToMany table name convention
    /// </summary>
    public class HasManyToManyJoinTableNameConvention : IHasManyToManyConvention
    {
        public bool Accept(IManyToManyPart target)
        {
            return string.IsNullOrEmpty(target.TableName);
        }

        public void Apply(IManyToManyPart target)
        {
            target.WithTableName(target.ChildType.Name + "To" + target.EntityType.Name);
        }

        public void Accept(IAcceptanceCriteria<IManyToManyCollectionInspector> acceptance)
        {
            acceptance.Expect(x => x.TableName, Is.Not.Set);
        }

        public void Apply(ICollectionAlteration alteration, IManyToManyCollectionInspector inspector)
        {
            alteration.TableName(inspector.ChildType.Name + "To" + inspector.EntityType.Name);
        }
    }
}