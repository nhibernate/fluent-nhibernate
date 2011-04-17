using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Steps
{
    public class HasManyToManyStep : IAutomappingStep
    {
        private readonly IAutomappingConfiguration cfg;

        public HasManyToManyStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public bool ShouldMap(Member member)
        {
            var type = member.PropertyType;
            if (type.Namespace != "Iesi.Collections.Generic" &&
                type.Namespace != "System.Collections.Generic")
                return false;
            if (type.HasInterface(typeof(IDictionary)) || type.ClosesInterface(typeof(IDictionary<,>)) || type.Closes(typeof(System.Collections.Generic.IDictionary<,>)))
                return false;

            var hasInverse = GetInverseProperty(member) != null;
            return hasInverse;
        }

        private static Member GetInverseProperty(Member member)
        {
            var type = member.PropertyType;
            var expectedInversePropertyType = type.GetGenericTypeDefinition()
                .MakeGenericType(member.DeclaringType);

            var argument = type.GetGenericArguments()[0];
            return argument.GetProperties()
                .Select(x => x.ToMember())
                .Where(x => x.PropertyType == expectedInversePropertyType && x != member)
                .FirstOrDefault();
        }

        static CollectionMapping GetCollection(Member property)
        {
            var collectionType = CollectionTypeResolver.Resolve(property);

            return CollectionMapping.For(collectionType);
        }

        private void ConfigureModel(Member member, CollectionMapping mapping, ClassMappingBase classMap, Type parentSide)
        {
            // TODO: Make the child type safer
            mapping.ContainingEntityType = classMap.Type;
            mapping.Set(x => x.Name, Layer.Defaults, member.Name);
            mapping.Set(x => x.Relationship, Layer.Defaults, CreateManyToMany(member, member.PropertyType.GetGenericArguments()[0], classMap.Type));
            mapping.Set(x => x.ChildType, Layer.Defaults, member.PropertyType.GetGenericArguments()[0]);
            mapping.Member = member;

            SetDefaultAccess(member, mapping);
            SetKey(member, classMap, mapping);

            if (parentSide != member.DeclaringType)
                mapping.Set(x => x.Inverse, Layer.Defaults, true);
        }

        void SetDefaultAccess(Member member, CollectionMapping mapping)
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess != Access.Property && resolvedAccess != Access.Unset)
            {
                // if it's a property or unset then we'll just let NH deal with it, otherwise
                // set the access to be whatever we determined it might be
                mapping.Set(x => x.Access, Layer.Defaults, resolvedAccess.ToString());
            }

            if (member.IsProperty && !member.CanWrite)
                mapping.Set(x => x.Access, Layer.Defaults, cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
        }

        static ICollectionRelationshipMapping CreateManyToMany(Member property, Type child, Type parent)
        {
            var mapping = new ManyToManyMapping
            {
                ContainingEntityType = parent
            };
            mapping.Set(x => x.Class, Layer.Defaults, new TypeReference(property.PropertyType.GetGenericArguments()[0]));

            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, child.Name + "_id");
            mapping.AddColumn(Layer.Defaults, columnMapping);

            return mapping;
        }

        static void SetKey(Member property, ClassMappingBase classMap, CollectionMapping mapping)
        {
            var columnName = property.DeclaringType.Name + "_id";
            var key = new KeyMapping
            {
                ContainingEntityType = classMap.Type
            };

            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, columnName);
            key.AddColumn(Layer.Defaults, columnMapping);

            mapping.Set(x => x.Key, Layer.Defaults, key);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            var inverseProperty = GetInverseProperty(member);
            var parentSide = cfg.GetParentSideForManyToMany(member.DeclaringType, inverseProperty.DeclaringType);
            var mapping = GetCollection(member);

            ConfigureModel(member, mapping, classMap, parentSide);

            classMap.AddCollection(mapping);
        }
    }
}
