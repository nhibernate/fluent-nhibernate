using System.Data.Common;
using NHibernate.Dialect.Schema;

namespace FluentNHibernate.Dialects.Schema;

public class MsSQLiteDataBaseMetaData(DbConnection connection, NHibernate.Dialect.Dialect dialect)
    : SQLiteDataBaseMetaData(connection, dialect)
{
    public override bool IncludeDataTypesInReservedWords => false;
}
