using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Versioning.HbmExtensions
{

    public static class HbmSetExtensions
    {
        public static void SetLazy(this HbmSet hbmSet, bool isLazy)
        {
            hbmSet.lazy = isLazy;
            hbmSet.lazySpecified = true;
        }

        public static void SetContents(this HbmSet hbmSet, object contentsHbm)
        {
            hbmSet.Item1 = contentsHbm;
        }
    }
}
