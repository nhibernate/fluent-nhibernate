using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Versioning.HbmExtensions
{
    public static class HbmDiscriminatorExtensions
    {
        public static void SetInsert(this HbmDiscriminator hbmDiscriminator, bool insert)
        {
            hbmDiscriminator.insert = insert;
            hbmDiscriminator.insertSpecified = true;
        }

        public static void SetColumn(this HbmDiscriminator hbmDiscriminator, HbmColumn column )
        {
            hbmDiscriminator.column = column;
        }

        public static void SetColumn(this HbmDiscriminator hbmDiscriminator, string columnName)
        {
            hbmDiscriminator.column1 = columnName;
        }
    }
}
