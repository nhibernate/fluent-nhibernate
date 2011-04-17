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
            keys = new AutoKeyMapper();
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
            mapping.Set(x => x.Name, Layer.Defaults, member.Name);
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
                mapping.Set(x => x.Access, Layer.Defaults, resolvedAccess.ToString());
            }

            if (member.IsProperty && !member.CanWrite)
                mapping.Set(x => x.Access, Layer.Defaults, cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
        }

        private void SetElement(Member property, ClassMappingBase classMap, CollectionMapping mapping)
        {
            var element = new ElementMapping
            {
                ContainingEntityType = classMap.Type,
            };
            element.Set(x => x.Type, Layer.Defaults, new TypeReference(property.PropertyType.GetGenericArguments()[0]));

            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, cfg.SimpleTypeCollectionValueColumn(property));
            element.AddColumn(Layer.Defaults, columnMapping);
            mapping.Set(x => x.Element, Layer.Defaults, element);
        }
    }
}