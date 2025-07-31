using System.Text;

namespace FluentNHibernate.Cfg.Db;

public class MySQLConnectionStringBuilder : ConnectionStringBuilder
{
    string server;
    int? port;
    string database;
    string username;
    string password;

    public MySQLConnectionStringBuilder Server(string server)
    {
        this.server = server;
        IsDirty = true;
        return this;
    }

    public MySQLConnectionStringBuilder Server(string[] servers)
    {
        this.server = string.Join(", ", servers);
        IsDirty = true;
        return this;
    }

    public MySQLConnectionStringBuilder Port(int port)
    {
        this.port = port;
        IsDirty = true;
        return this;
    }

    public MySQLConnectionStringBuilder Database(string database)
    {
        this.database = database;
        IsDirty = true;
        return this;
    }

    public MySQLConnectionStringBuilder Username(string username)
    {
        this.username = username;
        IsDirty = true;
        return this;
    }

    public MySQLConnectionStringBuilder Password(string password)
    {
        this.password = password;
        IsDirty = true;
        return this;
    }

    protected internal override string Create()
    {
        var connectionString = base.Create();

        if (!string.IsNullOrEmpty(connectionString))
            return connectionString;

        var sb = new StringBuilder();

        sb.AppendFormat("Server={0};Database={1};User ID={2};Password={3}", server, database, username, password);
        if (port.HasValue)
            sb.AppendFormat(";Port={0}", port);

        return sb.ToString();
    }
}
