using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm;

[TestFixture]
public class SubclassIdentityTests
{
    [Test]
    public void SubclassMemberMatchingIdConventionIsMappedAsPropertyNotSwallowed()
    {
        var model = AutoMap.Source(new StubTypeSource(typeof(Vehicle), typeof(Car)), new PerTypeIdConvention())
            .Override<Vehicle>(m => m.DiscriminateSubClassesOnColumn("type"));

        var root = (ClassMapping)model.BuildMappings()
            .SelectMany(x => x.Classes)
            .Single(c => c.Type == typeof(Vehicle));
        var subclass = root.Subclasses.Single();

        // Car.CarId matches the id convention, but a subclass has no id of its own so the
        // identity step's Map is a no-op for it. It must not be claimed (and thereby
        // swallowed) by the identity step — it has to fall through and be mapped as a
        // regular property.
        subclass.Properties.Select(x => x.Name).ShouldContain("CarId");
    }
}

// Treats "Id" and "<TypeName>Id" as the identifier — a common convention that also makes a
// subclass expose a member matching the rule.
class PerTypeIdConvention : DefaultAutomappingConfiguration
{
    public override bool IsId(Member member)
    {
        return member.Name == "Id" || member.Name == member.DeclaringType.Name + "Id";
    }
}

class Vehicle
{
    public int Id { get; set; }
    public string Make { get; set; }
}

class Car : Vehicle
{
    public int CarId { get; set; }
    public string Model { get; set; }
}
