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
            hbmList.lazy = isLazy ? HbmCollectionLazy.True : HbmCollectionLazy.False;
            hbmList.lazySpecified = true;
        }

        public static void SetContents(this HbmList hbmList, object contentsHbm)
        {
            hbmList.Item1 = contentsHbm;
        }

        public static void SetIndex(this HbmList hbmList, HbmIndex indexHbm)
        {
            hbmList.Item = indexHbm;
        }
    }

}
