using System;
using System.Collections.Generic;
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
        readonly AutoCollectionCreator collections;

        public SimpleTypeCollectionStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
            keys = new AutoKeyMapper(cfg);
            collections = new AutoCollectionCreator();
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

            var mapping = collections.CreateCollectionMapping(member.PropertyType);

            mapping.ContainingEntityType = classMap.Type;
            mapping.Member = member;
            mapping.SetDefaultValue(x => x.Name, member.Name);

            if (member.IsProperty && !member.CanWrite)
                mapping.Access = cfg.GetAccessStrategyForReadOnlyProperty(member).ToString();

            keys.SetKey(member, classMap, mapping);
            SetElement(member, classMap, mapping);
        
            classMap.AddCollection(mapping);
        }

        private void SetElement(Member property, ClassMappingBase classMap, ICollectionMapping mapping)
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