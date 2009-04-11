using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Versioning.HbmExtensions
{
    public static class HbmBagExtensions
    {
        public static void SetLazy(this HbmBag hbmBag, bool isLazy)
        {
            hbmBag.lazy = isLazy ? HbmCollectionLazy.True : HbmCollectionLazy.False;
            hbmBag.lazySpecified = true;
        }

        public static void SetContents(this HbmBag hbmBag, object contentsHbm)
        {
            hbmBag.Item = contentsHbm;
        }
    }
}
