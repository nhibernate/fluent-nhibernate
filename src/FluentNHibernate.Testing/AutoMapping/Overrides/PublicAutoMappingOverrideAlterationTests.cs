﻿using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Testing.Automapping;
using FluentNHibernate.Testing.Fixtures.AutoMappingAlterations.Model;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Overrides
{
    [TestFixture]
    public class PublicAutoMappingOverrideAlterationTests
    {
        private AutoMappingOverrideAlteration alteration;

        [SetUp]
        public void CreateOverride()
        {
            alteration = new AutoMappingOverrideAlteration(typeof(ExampleClass).Assembly, false);
        }

        [Test]
        public void OverridesApplied()
        {
            var model = AutoMap.AssemblyOf<Baz>()
                .Where(t => t.Namespace == typeof(Baz).Namespace)
                .Alterations(x => x.Add(alteration));

            new AutoMappingTester<Baz>(model)
                .Element("class").HasAttribute("batch-size", "10");
        }

        [Test]
        public void InternalOverridesNotApplied()
        {
            var model = AutoMap.AssemblyOf<Baz>()
                .Where(t => t.Namespace == typeof(Baz).Namespace)
                .Alterations(x => x.Add(alteration));

            new AutoMappingTester<Baz>(model)
                .Element("class").HasAttribute("table", "`Baz`");
        }

        [Test]
        public void RegularAutoMappingsStillWorkWhenOverridesApplied()
        {
            var model = AutoMap.AssemblyOf<Baz>()
                .Where(t => t.Namespace == typeof(Baz).Namespace);

            alteration.Alter(model);

            new AutoMappingTester<Baz>(model)
                .Element("class/property[@name='Name']").Exists();
        }

		[Test]
		public void OverridesCanBeAbstract()
		{
			var model = AutoMap.AssemblyOf<Qux>()
				.Where(t => t.Namespace == typeof(Qux).Namespace);

			alteration.Alter(model);

			new AutoMappingTester<Qux>(model)
				.Element("class").HasAttribute("batch-size", "10");
		}
    }
}