using System.Data.Common;
using FluentNHibernate.Dialects.Schema;
using NHibernate.Dialect;
using NHibernate.Dialect.Schema;

namespace FluentNHibernate.Dialects;

public class MsSQLiteDialect : SQLiteDialect
{
    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection) =>
        new MsSQLiteDataBaseMetaData(connection, this);
}
