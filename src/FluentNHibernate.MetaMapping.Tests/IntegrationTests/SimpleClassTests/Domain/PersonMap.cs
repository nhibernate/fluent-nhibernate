using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.BackwardCompatibility;

namespace FluentNHibernate.MetaMapping.Tests.IntegrationTests.SimpleClassTests.Domain
{
	public class PersonMap : ClassMap<Person>
	{
		public PersonMap()
		{
			Id(x => x.Id);
			Map(x => x.Name)
				.WithLengthOf(30)
				.CanNotBeNull();
			Map(x => x.Age)
				.CanNotBeNull();
		}
	}
}
