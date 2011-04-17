using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Steps
{
    public class CollectionStep : IAutomappingStep
    {
        readonly IAutomappingConfiguration cfg;
        readonly AutoKeyMapper keys;

        public CollectionStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
            keys = new AutoKeyMapper();
        }

        public bool ShouldMap(Member member)
        {
            return member.PropertyType.Namespace.In("System.Collections.Generic", "Iesi.Collections.Generic") &&
                !member.PropertyType.HasInterface(typeof(IDictionary)) &&
                !member.PropertyType.ClosesInterface(typeof(IDictionary<,>)) &&
                !member.PropertyType.Closes(typeof(IDictionary<,>));
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
            mapping.Set(x => x.ChildType, Layer.Defaults, member.PropertyType.GetGenericArguments()[0]);

            SetDefaultAccess(member, mapping);
            SetRelationship(member, classMap, mapping);
            keys.SetKey(member, classMap, mapping);

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

        static void SetRelationship(Member property, ClassMappingBase classMap, CollectionMapping mapping)
        {
            var relationship = new OneToManyMapping
            {
                ContainingEntityType = classMap.Type
            };
            relationship.Set(x => x.Class, Layer.Defaults, new TypeReference(property.PropertyType.GetGenericArguments()[0]));

            mapping.Set(x => x.Relationship, Layer.Defaults, relationship);
        }
    }
}