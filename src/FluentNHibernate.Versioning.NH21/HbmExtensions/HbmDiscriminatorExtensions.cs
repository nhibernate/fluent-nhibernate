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
        }

        public static void SetColumn(this HbmDiscriminator hbmDiscriminator, HbmColumn column)
        {
            hbmDiscriminator.Item = column;
        }

        public static void SetColumn(this HbmDiscriminator hbmDiscriminator, string columnName)
        {
            hbmDiscriminator.column = columnName;
        }
    }
}
