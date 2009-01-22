using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Versioning.HbmExtensions
{
    public static class HbmManyToOneExtensions
    {
        public static void SetNotNull(this HbmManyToOne hbmManyToOne, bool isNotNullable)
        {
            hbmManyToOne.notnull = isNotNullable;
            hbmManyToOne.notnullSpecified = true;
        }
    }
}
