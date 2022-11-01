using NHibernate;
using NHibernate.Driver;
using NHibernate.Engine;
using System.Data;
using System.Data.Common;

namespace FluentNHibernate.Driver;

/// <summary>
/// NHibernate driver for the Microsoft.Data.Sqlite data provider for .NETStandard2.0.
/// </summary>
/// <remarks>
/// <para>
/// In order to use this driver you must have the Microsoft.Data.Sqlite.dll assembly available
/// for NHibernate to load.
/// </para>
/// <para>
/// You can get the Microsoft.Data.Sqlite.dll assembly from
/// <a href="https://github.com/aspnet/Microsoft.Data.Sqlite">https://github.com/aspnet/Microsoft.Data.Sqlite</a>
/// </para>
/// <para>
/// Please check <a href="https://github.com/aspnet/Microsoft.Data.Sqlite">https://github.com/aspnet/Microsoft.Data.Sqlite</a> for more information regarding SQLite.
/// </para>
/// </remarks>
public class MsSQLiteDriver : ReflectionBasedDriver
{
    /// <summary>
    /// Initializes a new instance of <see cref="MsSQLiteDriver"/>.
    /// </summary>
    /// <exception cref="HibernateException">
    /// Thrown when the <c>Sqlite.NET</c> assembly can not be loaded.
    /// </exception>
    public MsSQLiteDriver() : base(
        "Microsoft.Data.Sqlite",
        "Microsoft.Data.Sqlite",
        "Microsoft.Data.Sqlite.SqliteConnection",
        "Microsoft.Data.Sqlite.SqliteCommand")
    {
    }

    public override DbConnection CreateConnection()
    {
        var connection = base.CreateConnection();
        connection.StateChange += Connection_StateChange;
        return connection;
    }

    private static void Connection_StateChange(object sender, StateChangeEventArgs e)
    {
        if ((e.OriginalState == ConnectionState.Broken || e.OriginalState == ConnectionState.Closed || e.OriginalState == ConnectionState.Connecting) &&
            e.CurrentState == ConnectionState.Open)
        {
            var connection = (DbConnection)sender;
            using (var command = connection.CreateCommand())
            {
                // Activated foreign keys if supported by SQLite.  Unknown pragmas are ignored.
                command.CommandText = "PRAGMA foreign_keys = ON";
                command.ExecuteNonQuery();
            }
        }
    }

    public override IResultSetsCommand GetResultSetsCommand(ISessionImplementor session)
    {
        return new BasicResultSetsCommand(session);
    }

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => "@";

    public override bool SupportsMultipleOpenReaders => false;

    public override bool SupportsMultipleQueries => true;

    public override bool SupportsNullEnlistment => false;

    public override bool HasDelayedDistributedTransactionCompletion => true;
}
