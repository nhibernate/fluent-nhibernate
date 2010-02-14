using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Overrides
{
    [TestFixture]
    public class HibernateMappingOverrides
    {
        [Test, Ignore]
        public void CanOverrideDefaultLazy()
        {
            var model = AutoMap.Source(new StubTypeSource(new[] { typeof(Parent) }))
               .Override<Parent>(o => o.HibernateMapping.Not.DefaultLazy());

            HibernateMapping hibernateMapping = model.BuildMappings().First();

            hibernateMapping.DefaultLazy.ShouldBeFalse();
        }

    }
}
