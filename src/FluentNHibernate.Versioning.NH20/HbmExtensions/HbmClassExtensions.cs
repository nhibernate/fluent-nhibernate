using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Versioning.HbmExtensions
{
    public static class HbmClassExtensions
    {
        public static void SetId(this HbmClass hbmClass, object hbmId)
        {
            hbmClass.Item1 = hbmId;
        }
    }
}
