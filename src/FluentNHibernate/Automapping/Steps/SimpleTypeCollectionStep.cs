using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Steps
{
    public class SimpleTypeCollectionStep : IAutomappingStep
    {
        readonly IAutomappingConfiguration cfg;
        readonly AutoKeyMapper keys;

        public SimpleTypeCollectionStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
            keys = new AutoKeyMapper(cfg);
        }

        public bool ShouldMap(Member member)
        {
            if (!member.PropertyType.IsGenericType)
                return false;
            if (member.PropertyType.ClosesInterface(typeof(IDictionary<,>)) || member.PropertyType.Closes(typeof(IDictionary<,>)))
                return false;

            var childType = member.PropertyType.GetGenericArguments()[0];

            return member.PropertyType.ClosesInterface(typeof(IEnumerable<>)) &&
                    (childType.IsPrimitive || childType.In(typeof(string), typeof(DateTime)));
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            if (member.DeclaringType != classMap.Type)
                return;

            var collectionType = CollectionTypeResolver.Resolve(member);
            var mapping = CollectionMapping.For(collectionType);

            mapping.ContainingEntityType = classMap.Type;
            mapping.Member = member;
            mapping.SetDefaultValue(x => x.Name, member.Name);
            SetDefaultAccess(member, mapping);

            keys.SetKey(member, classMap, mapping);
            SetElement(member, classMap, mapping);
        
            classMap.AddCollection(mapping);
        }

        void SetDefaultAccess(Member member, CollectionMapping mapping)
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess != Access.Property && resolvedAccess != Access.Unset)
            {
                // if it's a property or unset then we'll just let NH deal with it, otherwise
                // set the access to be whatever we determined it might be
                mapping.SetDefaultValue(x => x.Access, resolvedAccess.ToString());
            }

            if (member.IsProperty && !member.CanWrite)
                mapping.SetDefaultValue(x => x.Access, cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
        }

        private void SetElement(Member property, ClassMappingBase classMap, CollectionMapping mapping)
        {
            var element = new ElementMapping
            {
                ContainingEntityType = classMap.Type,
                Type = new TypeReference(property.PropertyType.GetGenericArguments()[0])
            };

            element.AddDefaultColumn(new ColumnMapping { Name = cfg.SimpleTypeCollectionValueColumn(property) });
            mapping.SetDefaultValue(x => x.Element, element);
        }
    }
}