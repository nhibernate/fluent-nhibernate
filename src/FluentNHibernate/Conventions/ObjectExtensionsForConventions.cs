using System.Linq;

namespace FluentNHibernate.Conventions
{
    public static class ObjectExtensionsForConventions
    {
        public static bool IsAny<T>(this T target, params T[] possibleValues)
        {
            return possibleValues.Contains(target);
        }

        public static bool IsNotAny<T>(this T target, params T[] possibleValues)
        {
            return !target.IsAny(possibleValues);
        }
    }
}