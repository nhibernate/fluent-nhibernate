using System.Data.Common;
using NHibernate.Dialect.Schema;

namespace FluentNHibernate.Dialects.Schema;

public class MsSQLiteDataBaseMetaData : SQLiteDataBaseMetaData
{
    public MsSQLiteDataBaseMetaData(DbConnection connection, NHibernate.Dialect.Dialect dialect)
        : base(connection, dialect)
    {
    }

    public override bool IncludeDataTypesInReservedWords => false;
}
