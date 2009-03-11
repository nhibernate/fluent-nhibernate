using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
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