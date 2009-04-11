using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Versioning.HbmExtensions
{
    public static class HbmOneToManyExtensions
    {
        public static void SetNotFound(this HbmOneToMany hbmOneToMany, bool exceptionOnNotFound)
        {
            hbmOneToMany.notfound = exceptionOnNotFound ? HbmNotFoundMode.Exception : HbmNotFoundMode.Ignore;
            hbmOneToMany.notfoundSpecified = true;
        }
    }
}
