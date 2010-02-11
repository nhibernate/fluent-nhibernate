using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public class AutoMapOneToMany : IAutoMapper
    {
        readonly AutoSimpleTypeCollection simpleTypeCollectionStep;
        readonly AutoEntityCollection entityCollectionStep;

        public AutoMapOneToMany(AutoMappingExpressions expressions)
        {
            simpleTypeCollectionStep = new AutoSimpleTypeCollection(expressions);
            entityCollectionStep = new AutoEntityCollection(expressions);
        }

        public bool MapsProperty(Member property)
        {
            return simpleTypeCollectionStep.MapsProperty(property) ||
                   entityCollectionStep.MapsProperty(property);
        }

        public void Map(ClassMappingBase classMap, Member property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            if (simpleTypeCollectionStep.MapsProperty(property))
                simpleTypeCollectionStep.Map(classMap, property);
            else if (entityCollectionStep.MapsProperty(property))
                entityCollectionStep.Map(classMap, property);
        }
    }
}
