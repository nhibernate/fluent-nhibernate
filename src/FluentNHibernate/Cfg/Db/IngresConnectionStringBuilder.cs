using System.Text;

namespace FluentNHibernate.Cfg.Db;

public class IngresConnectionStringBuilder : ConnectionStringBuilder
{
    string server;
    string database;
    string port;
    string username;
    string password;

    public IngresConnectionStringBuilder Server(string server)
    {
        this.server = server;
        IsDirty = true;
        return this;
    }

    public IngresConnectionStringBuilder Database(string database)
    {
        this.database = database;
        IsDirty = true;
        return this;
    }

    public IngresConnectionStringBuilder Port(string port)
    {
        this.port = port;
        IsDirty = true;
        return this;
    }

    public IngresConnectionStringBuilder Username(string username)
    {
        this.username = username;
        IsDirty = true;
        return this;
    }

    public IngresConnectionStringBuilder Password(string password)
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

        sb.AppendFormat("Host={0};Database={1};Port={2};User ID={3};PWD={4}", server, database, port, username, password);

        return sb.ToString();
    }
}
