using System.Reflection;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapIdentity : IAutoMapper
    {
        private readonly AutoMappingExpressions expressions;

        public AutoMapIdentity(AutoMappingExpressions conventions)
        {
            this.expressions = conventions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            return expressions.FindIdentity(property);
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            if (classMap is AutoJoinedSubClassPart<T> || classMap is AutoSubClassPart<T>)
                return;

            classMap.Id(ExpressionBuilder.Create<T>(property));
        }
    }
}