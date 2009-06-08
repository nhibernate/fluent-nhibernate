using System.Reflection;
using FluentNHibernate.Utils.Reflection;

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
            if (classMap is AutoJoinedSubClassPart<T>)
                return;

            classMap.Id(ExpressionBuilder.Create<T>(property));
        }
    }
}