using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default HasManyToMany foreign key naming convention
    /// </summary>
    public class HasManyToManyParentForeignKeyConvention : IHasManyToManyConvention
    {
        public bool Accept(IManyToManyPart target)
        {
            return string.IsNullOrEmpty(target.ParentKeyColumn);
        }

        public void Apply(IManyToManyPart target)
        {
            target.WithParentKeyColumn(target.EntityType.Name + "_id");
        }
    }
}