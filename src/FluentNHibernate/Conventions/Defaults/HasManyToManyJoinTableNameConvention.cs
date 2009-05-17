using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    ///// <summary>
    ///// Default HasManyToMany table name convention
    ///// </summary>
    //public class HasManyToManyJoinTableNameConvention : IHasManyToManyConvention
    //{
    //    public bool Accept(IManyToManyPart target)
    //    {
    //        return string.IsNullOrEmpty(target.TableName);
    //    }

    //    public void Apply(IManyToManyPart target)
    //    {
    //        target.WithTableName(target.ChildType.Name + "To" + target.EntityType.Name);
    //    }
    //}
}