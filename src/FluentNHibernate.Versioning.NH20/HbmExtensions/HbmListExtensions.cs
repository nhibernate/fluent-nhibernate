using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Versioning.HbmExtensions
{
    public static class HbmListExtensions
    {
        public static void SetLazy(this HbmList hbmList, bool isLazy)
        {
            hbmList.lazy = isLazy;
            hbmList.lazySpecified = true;
        }

        public static void SetContents(this HbmList hbmList, object contentsHbm)
        {
            hbmList.Item2 = contentsHbm;
        }
    }

}
