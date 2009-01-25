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
using List=NHibernate.Mapping.List;

namespace FluentNHibernate.Testing.MappingModel.Output.Meta
{
	[TestFixture]
	public class ClassMapperTests
	{
		[SetUp]
		public void SetUp()
		{
			this.dialect = MockRepository.GenerateMock<Dialect>();
			this.mappings = new Configuration().CreateMappings();
			this.idMapper = MockRepository.GenerateMock<IIdMapper>();
			this.classMapper = new ClassMapper(this.mappings, this.dialect,this.idMapper);
		}

		private IIdMapper idMapper;
		private Mappings mappings;
		private Dialect dialect;
		private ClassMapper classMapper;

		[Test]
		//TODO: need to have table name.
		public void Can_bind_default_properties()
		{
			ClassMapping cm = new ClassMapping {Name = typeof (Artist).AssemblyQualifiedName};
			this.classMapper.Bind(cm);
			PersistentClass clazz = mappings.GetClass(typeof(Artist).FullName);
			Table tbl = clazz.Table;
			Assert.That(clazz.IsLazy);
			Assert.That(clazz.EntityName,Is.EqualTo(typeof(Artist).FullName));
			Assert.That(clazz.ClassName,Is.EqualTo(typeof(Artist).AssemblyQualifiedName));
			Assert.That(clazz.ProxyInterfaceName, Is.EqualTo(typeof (Artist).AssemblyQualifiedName));
			Assert.That(clazz.IsMutable);
			Assert.That(tbl,Is.Not.Null);
			Assert.That(tbl.Name, Is.EqualTo(typeof(Artist).Name));
		}

		[Test]
		//TODO: need to have table name.
		public void Can_bind_flat_identifier()
		{
			ClassMapping cm = new ClassMapping
			                  	{
			                  		Name = typeof (Artist).AssemblyQualifiedName,
			                  	};
            cm.Id = new IdMapping();
			this.classMapper.Bind(cm);

			PersistentClass clazz = mappings.GetClass(typeof(Artist).FullName);
			idMapper.AssertWasCalled(x=>x.Bind(cm.Id.As<IdMapping>(),clazz));
		}


	}
}
