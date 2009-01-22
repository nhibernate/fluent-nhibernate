using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Versioning.HbmExtensions
{
    public static class HbmPropertyExtensions
    {
        public static void SetNotNull(this HbmProperty hbmProperty, bool isNotNullable)
        {
            hbmProperty.notnull = isNotNullable;
            hbmProperty.notnullSpecified = true;
        }
    }
}
