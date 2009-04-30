using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    public abstract class BuiltForeignKeyConvention : ForeignKeyConvention, IRelationshipConvention
    {
        public bool Accept(IRelationship target)
        {
            return true;
        }

        public void Apply(IRelationship target)
        {
            if (target is IOneToManyPart) base.Apply((IOneToManyPart)target);
            if (target is IManyToManyPart) base.Apply((IManyToManyPart)target);
            if (target is IManyToOnePart) base.Apply((IManyToOnePart)target);
        }
    }
}