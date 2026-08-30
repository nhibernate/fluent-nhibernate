using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm.Conventions;

[TestFixture]
public class OverriddenIdConventionTests
{
    [Test]
    public void OverriddenIdPassesTheMemberToIdConventions()
    {
        CapturingIdConvention.Reset();

        AutoMap.Source(new StubTypeSource(typeof(OverriddenIdEntity)))
            .Override<OverriddenIdEntity>(m => m.Id(x => x.Id))
            .Conventions.Add<CapturingIdConvention>()
            .BuildMappings();

        CapturingIdConvention.WasApplied.ShouldBeTrue();
        CapturingIdConvention.PropertyName.ShouldEqual("Id");
    }

    class CapturingIdConvention : IIdConvention
    {
        public static bool WasApplied;
        public static string PropertyName;

        public static void Reset()
        {
            WasApplied = false;
            PropertyName = null;
        }

        public void Apply(IIdentityInstance instance)
        {
            WasApplied = true;
            PropertyName = instance.Property?.Name;
        }
    }
}

public abstract class OverriddenIdBase
{
    public virtual int Id { get; set; }
}

public class OverriddenIdEntity : OverriddenIdBase
{
    public virtual string SomeField { get; set; }
}
