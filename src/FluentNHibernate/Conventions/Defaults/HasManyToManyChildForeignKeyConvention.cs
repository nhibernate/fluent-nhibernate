using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default HasMany child foreign key naming convention
    /// </summary>
    public class HasManyToManyChildForeignKeyConvention : IHasManyToManyConvention
    {
        public bool Accept(IManyToManyPart target)
        {
            return string.IsNullOrEmpty(target.ChildKeyColumn);
        }

        public void Apply(IManyToManyPart target)
        {
            target.WithChildKeyColumn(target.ChildType.Name + "_id");
        }
    }
}