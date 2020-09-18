using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class DefaultAutoMappingConfigurationTests
    {
        IAutomappingConfiguration configuration;

        [SetUp]
        public void FixtureSetUp()
        {
            configuration = new DefaultAutomappingConfiguration();
        }

        [Test]
        public void ShouldNotMapCompilerGeneratedClasses()
        {
            var anonymous = new { id = 5, title = "Whatever happening"};
            configuration.ShouldMap(anonymous.GetType()).ShouldBeFalse();
        }
    }
}
