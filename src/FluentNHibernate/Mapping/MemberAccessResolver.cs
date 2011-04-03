namespace FluentNHibernate.Mapping
{
    public static class MemberAccessResolver
    {
        public static Access Resolve(Member member)
        {
            if (member.IsAutoProperty)
            {
                var property = (PropertyMember)member;
                
                if (property.Set != null && !(property.Set.IsPrivate || property.Set.IsInternal))
                    return Access.Property;

                return Access.BackField;
            }

            if (member.IsProperty)
            {
                var property = (PropertyMember)member;

                if (property.Set != null && !(property.Set.IsPrivate || property.Set.IsInternal))
                    return Access.Property;

                Member backingField;

                if (!property.TryGetBackingField(out backingField))
                    return Access.Property; // just use default, don't think we can do anything here

                return Access.ReadOnlyPropertyWithField(Naming.Determine(backingField.Name));
            }

            if (member.IsMethod)
            {
                var method = (MethodMember)member;

                Member backingField;

                if (!method.TryGetBackingField(out backingField))
                    return Access.Property; // just use default, don't think we can do anything here

                return Access.Field; // use a field because we just access methods backing fields directly
            }

            if (member.IsField)
                return Access.Field;

            return Access.Property; // just default it if we can't guess, NH will complain but oh well
        }
    }
}