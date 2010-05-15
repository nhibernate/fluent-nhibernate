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
        readonly AutoCollectionCreator collections;

        public CollectionStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
            keys = new AutoKeyMapper(cfg);
            collections = new AutoCollectionCreator();
        }

        public bool ShouldMap(Member member)
        {
            return member.PropertyType.Namespace.In("System.Collections.Generic", "Iesi.Collections.Generic");
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            if (member.DeclaringType != classMap.Type)
                return;

            var mapping = collections.CreateCollectionMapping(member.PropertyType);

            mapping.ContainingEntityType = classMap.Type;
            mapping.Member = member;
            mapping.SetDefaultValue(x => x.Name, member.Name);
            mapping.ChildType = member.PropertyType.GetGenericArguments()[0];

            if (member.IsProperty && !member.CanWrite)
                mapping.Access = cfg.GetAccessStrategyForReadOnlyProperty(member).ToString();

            SetRelationship(member, classMap, mapping);
            keys.SetKey(member, classMap, mapping);

            classMap.AddCollection(mapping);  
        }

        private void SetRelationship(Member property, ClassMappingBase classMap, ICollectionMapping mapping)
        {
            var relationship = new OneToManyMapping
            {
                Class = new TypeReference(property.PropertyType.GetGenericArguments()[0]),
                ContainingEntityType = classMap.Type
            };

            mapping.SetDefaultValue(x => x.Relationship, relationship);
        }
    }
}