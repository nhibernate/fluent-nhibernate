using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;

namespace FluentNHibernate.Utils
{
    public static class HbmExtensions
    {
        public static HbmType ToHbmType(this TypeReference typeReference)
        {
            return new HbmType()
            {
                name = typeReference.Name,
            };
        }

        public static HbmFilterParam ToHbmFilterParam(this KeyValuePair<string, IType> parameterPair)
        {
            return new HbmFilterParam()
            {
                name = parameterPair.Key,
                type = parameterPair.Value.Name
            };
        }
    }
}