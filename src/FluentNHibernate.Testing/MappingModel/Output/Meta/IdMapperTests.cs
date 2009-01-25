using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output.Meta;
using FluentNHibernate.Testing.DomainModel;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output.Meta
{
	[TestFixture]
	public class IdMapperTests
	{
		[SetUp]
		public void SetUp()
		{
			this.dialect = MockRepository.GenerateMock<Dialect>();
			this.mappings = new Configuration().CreateMappings();
			this.idMapper = new IdMapper();
			this.classMapper = new ClassMapper(this.mappings, this.dialect, this.idMapper);
		}
		private IIdMapper idMapper;
		private Mappings mappings;
		private Dialect dialect;
		private ClassMapper classMapper;

		[Test]
		//TODO: need to have table name.
		public void Can_bind_identifier()
		{
			ClassMapping cm = new ClassMapping
			{
				Name = typeof(Artist).AssemblyQualifiedName,
			};
			var idMapping = new IdMapping();
			idMapping.Name = "ID";
			idMapping.Generator = new IdGeneratorMapping { ClassName = "native" };
			idMapping.AddIdColumn(new ColumnMapping {Name = "id", IsNotNullable = false });
			cm.Id = idMapping;
			this.classMapper.Bind(cm);
			
			PersistentClass clazz = mappings.GetClass(typeof(Artist).FullName);
			var identifier = clazz.Identifier as SimpleValue;
			var identifierColumn = identifier.ColumnIterator.First() as Column;
			Assert.That(identifier.Table,Is.Not.Null);
			Assert.That(identifierColumn.Name, Is.EqualTo("id"));
			Assert.That(identifier.IsNullable, Is.True);
			Assert.That(identifier.IdentifierGeneratorStrategy,Is.EqualTo("native"));
		}
	}
}
