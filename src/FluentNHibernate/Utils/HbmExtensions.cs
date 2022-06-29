using FluentNHibernate.MappingModel;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Utils
{
    internal static class HbmExtensions
    {
        public static HbmType ToHbmType(this TypeReference typeReference)
        {
            return new HbmType()
            {
                name = typeReference.Name,
            };
        }
    }
}