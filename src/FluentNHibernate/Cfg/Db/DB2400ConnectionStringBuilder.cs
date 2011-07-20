namespace FluentNHibernate.Cfg.Db
{
    public class DB2400ConnectionStringBuilder : ConnectionStringBuilder
    {
        string _dataSource;
        string _userID;
        string _password;

        protected internal override string Create()
        {
            var connectionString = base.Create();

            if (!string.IsNullOrEmpty(connectionString))
                return connectionString;

            return string.Format("DataSource={0};UserID={1};Password={2};DataCompression=True;", _dataSource, _userID, _password);
        }

        public DB2400ConnectionStringBuilder Password(string password)
        {
            _password = password;
            IsDirty = true;
            return this;
        }

        public DB2400ConnectionStringBuilder UserID(string userID)
        {
            _userID = userID;
            IsDirty = true;
            return this;
        }

        public DB2400ConnectionStringBuilder DataSource(string datasource)
        {
            _dataSource = datasource;
            IsDirty = true;
            return this;
        }


    }
}