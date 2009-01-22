using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Versioning.HbmExtensions
{
    public static class HbmKeyExtensions
    {
        public static void SetCascadeOnDelete(this HbmKey hbmKey, bool cascadeOnDelete)
        {
            hbmKey.ondelete = cascadeOnDelete ? HbmOndelete.Cascade : HbmOndelete.Noaction;            
        }
    }
}
