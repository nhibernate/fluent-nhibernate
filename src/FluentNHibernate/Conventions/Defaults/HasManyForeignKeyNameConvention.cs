using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    public class HasManyForeignKeyNameConvention : IHasManyConvention
    {
        public bool Accept(IOneToManyPart target)
        {
            return string.IsNullOrEmpty(target.ColumnName);
        }

        public void Apply(IOneToManyPart target)
        {
            target.WithKeyColumn(target.ParentType.Name + "_id");
        }
    }
}