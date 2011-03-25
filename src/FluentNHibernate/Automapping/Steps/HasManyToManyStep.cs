using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (property.PropertyType.FullName.Contains("ISet"))
                return CollectionMapping.Set();

            return CollectionMapping.Bag();
        }

        private void ConfigureModel(Member member, CollectionMapping mapping, ClassMappingBase classMap, Type parentSide)
        {
            // TODO: Make the child type safer
            mapping.SetDefaultValue(x => x.Name, member.Name);
            mapping.Relationship = CreateManyToMany(member, member.PropertyType.GetGenericArguments()[0], classMap.Type);
            mapping.ContainingEntityType = classMap.Type;
            mapping.ChildType = member.PropertyType.GetGenericArguments()[0];
            mapping.Member = member;

            if (member.IsProperty && !member.CanWrite)
                mapping.Access = cfg.GetAccessStrategyForReadOnlyProperty(member).ToString();

            SetKey(member, classMap, mapping);

            if (parentSide != member.DeclaringType)
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

        private void SetKey(Member property, ClassMappingBase classMap, CollectionMapping mapping)
        {
            var columnName = property.DeclaringType.Name + "_id";
            var key = new KeyMapping();

            key.ContainingEntityType = classMap.Type;
            key.AddDefaultColumn(new ColumnMapping { Name = columnName });

            mapping.SetDefaultValue(x => x.Key, key);
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
