using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.MetaMapping.Tests.IntegrationTests.SimpleClassTests.Domain
{
	public class Person
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual int Age { get; set; }
	}
}
