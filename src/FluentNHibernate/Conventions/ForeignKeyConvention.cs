using System;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    //public abstract class ForeignKeyConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention
    //{
    //    private bool acceptParent = true;
    //    private bool acceptChild = true;

    //    protected abstract string GetKeyName(PropertyInfo property, Type type);

    //    public bool Accept(IManyToOnePart target)
    //    {
    //        return string.IsNullOrEmpty(target.GetColumnName());
    //    }

    //    public void Apply(IManyToOnePart target)
    //    {
    //        target.ColumnName(GetKeyName(target.Property, target.Property.PropertyType));
    //    }

    //    public bool Accept(IOneToManyPart target)
    //    {
    //        return target.KeyColumnNames.List().Count == 0;
    //    }

    //    public void Apply(IOneToManyPart target)
    //    {
    //        target.KeyColumnNames.Clear();
    //        target.KeyColumnNames.Add(GetKeyName(null, target.EntityType));
    //    }

    //    public bool Accept(IManyToManyPart target)
    //    {
    //        acceptParent = string.IsNullOrEmpty(target.ParentKeyColumn);
    //        acceptChild = string.IsNullOrEmpty(target.ChildKeyColumn);

    //        return acceptParent || acceptChild;
    //    }

    //    public void Apply(IManyToManyPart target)
    //    {
    //        if (acceptParent && target.EntityType != null)
    //            target.WithParentKeyColumn(GetKeyName(null, target.EntityType));

    //        if (acceptChild && target.ChildType != null)
    //            target.WithChildKeyColumn(GetKeyName(null, target.ChildType));
    //    }
    //}
}