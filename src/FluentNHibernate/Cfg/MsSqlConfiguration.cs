using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg
{
	public class MsSqlConfiguration : PersistenceConfiguration<MsSqlConfiguration>
	{
		protected MsSqlConfiguration()
		{
			Driver<SqlClientDriver>();
		}

		public static MsSqlConfiguration MsSql7
		{
			get
			{
				return new MsSqlConfiguration().Dialect<MsSql7Dialect>();
			}
		}

		public static MsSqlConfiguration MsSql2000
		{
			get
			{
				return new MsSqlConfiguration().Dialect<MsSql2000Dialect>();
			}
		}

		public static MsSqlConfiguration MsSql2005
		{
			get
			{
				return new MsSqlConfiguration().Dialect<MsSql2005Dialect>();
			}
		}

		public static MsSqlConfiguration MsSqlCe
		{
			get
			{
				return new MsSqlConfiguration().Dialect<MsSqlCeDialect>();
			}
		}
	}
}