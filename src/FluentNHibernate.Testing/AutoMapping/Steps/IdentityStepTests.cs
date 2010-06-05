using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils.Reflection;

namespace FluentNHibernate.Testing.AutoMapping.Steps
{
    [TestFixture]
    public class IdentityStepTests
    {
        [Test]
        public void IdentityStepShouldProvideAnAccessValueThatIsADefaultOnly()
        {
            IdentityStep step = new IdentityStep(new DefaultAutomappingConfiguration());

            var classMapping = new ClassMapping() {Type = typeof(ClassWithIdFieldAndGetter)};
            step.Map(classMapping, ReflectionHelper.GetMember<ClassWithIdFieldAndGetter>(x => x.Id));

            var idMapping = (IdMapping) classMapping.Id;
            idMapping.HasValue("Access").ShouldBeTrue();
            idMapping.IsSpecified(x => x.Access).ShouldBeFalse();
        }

        private class ClassWithIdFieldAndGetter
        {
#pragma warning disable 649
            int _id;
#pragma warning restore 649
            public int Id { get { return _id; } }
        }
    }
}
