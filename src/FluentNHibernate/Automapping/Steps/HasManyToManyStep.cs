using System;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

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

            var hasInverse = GetInverseProperty(member) != null;
            return hasInverse;
        }

        private static Member GetInverseProperty(Member property)
        {
            Type type = property.PropertyType;
            var inverseSide = type.GetGenericTypeDefinition()
                .MakeGenericType(property.DeclaringType);

            var argument = type.GetGenericArguments()[0];
            return argument.GetProperties()
                .Where(x => x.PropertyType == inverseSide)
                .Select(x => x.ToMember())
                .FirstOrDefault();
        }

        private ICollectionMapping GetCollection(Member property)
        {
            if (property.PropertyType.FullName.Contains("ISet"))
                return new SetMapping();

            return new BagMapping();
        }

        private void ConfigureModel(Member property, ICollectionMapping mapping, ClassMappingBase classMap, Type parentSide)
        {
            // TODO: Make the child type safer
            mapping.SetDefaultValue(x => x.Name, property.Name);
            mapping.Relationship = CreateManyToMany(property, property.PropertyType.GetGenericArguments()[0], classMap.Type);
            mapping.ContainingEntityType = classMap.Type;
            mapping.ChildType = property.PropertyType.GetGenericArguments()[0];
            mapping.Member = property;

            SetKey(property, classMap, mapping);

            if (parentSide != property.DeclaringType)
                mapping.Inverse = true;
        }

        private ICollectionRelationshipMapping CreateManyToMany(Member property, Type child, Type parent)
        {
            var mapping = new ManyToManyMapping
            {
                Class = new TypeReference(property.PropertyType.GetGenericArguments()[0]),
                ContainingEntityType = parent
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = child.Name + "_id" });

            return mapping;
        }

        private void SetKey(Member property, ClassMappingBase classMap, ICollectionMapping mapping)
        {
            var columnName = property.DeclaringType.Name + "_id";

            if (classMap is ComponentMapping)
                columnName = cfg.GetComponentColumnPrefix(((ComponentMapping)classMap).Member) + columnName;

            var key = new KeyMapping();

            key.ContainingEntityType = classMap.Type;
            key.AddDefaultColumn(new ColumnMapping { Name = columnName });

            mapping.SetDefaultValue(x => x.Key, key);
        }

        public void Map(ClassMappingBase classMap, Member property)
        {
            var inverseProperty = GetInverseProperty(property);
            var parentSide = cfg.GetParentSideForManyToMany(property.DeclaringType, inverseProperty.DeclaringType);
            var mapping = GetCollection(property);

            ConfigureModel(property, mapping, classMap, parentSide);

            classMap.AddCollection(mapping);
        }
    }
}
