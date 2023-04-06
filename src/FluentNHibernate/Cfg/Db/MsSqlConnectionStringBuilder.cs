using System.Text;

namespace FluentNHibernate.Cfg.Db;

public class MsSqlConnectionStringBuilder : ConnectionStringBuilder
{
    private string server;
    private string database;
    private string username;
    private string password;
    private bool trustedConnection;

    public MsSqlConnectionStringBuilder Server(string server)
    {
        this.server = server;
        IsDirty = true;
        return this;
    }

    public MsSqlConnectionStringBuilder Database(string database)
    {
        this.database = database;
        IsDirty = true;
        return this;
    }

    public MsSqlConnectionStringBuilder Username(string username)
    {
        this.username = username;
        IsDirty = true;
        return this;
    }

    public MsSqlConnectionStringBuilder Password(string password)
    {
        this.password = password;
        IsDirty = true;
        return this;
    }

    public MsSqlConnectionStringBuilder TrustedConnection()
    {
        trustedConnection = true;
        IsDirty = true;
        return this;
    }

    protected internal override string Create()
    {
        var connectionString = base.Create();

        if (!string.IsNullOrEmpty(connectionString))
            return connectionString;

        var sb = new StringBuilder();

        if (server.Contains(' '))
            sb.AppendFormat("Data Source=\"{0}\"", server);
        else
            sb.AppendFormat("Data Source={0}", server);

        if (database.Contains(' '))
            sb.AppendFormat(";Initial Catalog=\"{0}\"", database);
        else
            sb.AppendFormat(";Initial Catalog={0}", database);

        sb.AppendFormat(";Integrated Security={0}", trustedConnection);

        if (!trustedConnection)
        {
            if (username.Contains(' '))
                sb.AppendFormat(";User ID=\"{0}\"", username);
            else
                sb.AppendFormat(";User ID={0}", username);

            if (password.Contains(' '))
                sb.AppendFormat(";Password=\"{0}\"", password);
            else
                sb.AppendFormat(";Password={0}", password);
        }

        return sb.ToString();
    }
}
