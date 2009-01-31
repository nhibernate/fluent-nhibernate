using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.MappingModel.Conventions;
using FluentNHibernate.MetaMapping.Tests.IntegrationTests.SimpleClassTests.Domain;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace FluentNHibernate.MetaMapping.Tests.IntegrationTests.SimpleClassTests
{
	[TestFixture]
	public class SimpleClassTests
	{
		[SetUp]
		public void SetUp()
		{
			var cfg = MsSqlConfiguration.MsSql2005
				.ShowSql()
				.ConnectionString.Is("Server=(local);initial catalog=nhibernate;Integrated Security=SSPI")
				.ShowSql()
				.ConfigureProperties(new Configuration());
			var mappings = cfg.CreateMappings();
			var dialect = new SQLiteDialect();
			var model = new PersistenceModel();
			model.Add(new PersonMap());
			model.AddConvention(new NamingConvention());
			var hibernateMapping = model.BuildHibernateMapping();
			var classBinder = new RootClassBinder(mappings, dialect);
			foreach (var mapping in hibernateMapping.Classes)
			{
				classBinder.Bind(mapping);
			}
			cfg.BuildMapping();
			schemaExport = new SchemaExport(cfg);

			
		}
		[TearDown]
		public void TearDown()
		{

		}
		private  SchemaExport schemaExport;



		[Test]
		public void Class_with_no_relation_should_be_bound()
		{
			schemaExport.Execute(true, true, false, true);
			schemaExport.Execute(true, true, true, true);
		}


	}
}
