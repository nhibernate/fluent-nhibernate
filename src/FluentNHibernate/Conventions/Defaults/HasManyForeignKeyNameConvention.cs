using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default foreign key naming convention
    /// </summary>
    public class HasManyForeignKeyNameConvention : IHasManyConvention
    {
        public bool Accept(IOneToManyPart target)
        {
            return target.KeyColumnNames.List().Count == 0;
        }

        public void Apply(IOneToManyPart target)
        {
            target.KeyColumnNames.Add(target.ParentType.Name + "_id");
        }
    }
}