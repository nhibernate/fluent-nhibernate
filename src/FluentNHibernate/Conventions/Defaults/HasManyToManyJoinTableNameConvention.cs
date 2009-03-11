using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    public class HasManyToManyJoinTableNameConvention : IHasManyToManyConvention
    {
        public bool Accept(IManyToManyPart target)
        {
            return string.IsNullOrEmpty(target.TableName);
        }

        public void Apply(IManyToManyPart target)
        {
            target.WithTableName(target.ChildType.Name + "To" + target.ParentType.Name);
        }
    }
}