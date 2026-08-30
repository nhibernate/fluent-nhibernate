using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Steps;

[TestFixture]
public class IdentityStepTests
{
    IdentityStep mapper;

    [SetUp]
    public void CreateMapper()
    {
        mapper = new IdentityStep(new DefaultAutomappingConfiguration());
    }

    [Test]
    public void ShouldMapIdOnClassThatHasNoIdYet()
    {
        mapper.ShouldMap(new ClassMapping(), MemberFor("Id")).ShouldBeTrue();
    }

    [Test]
    public void ShouldNotMapNonIdMember()
    {
        mapper.ShouldMap(new ClassMapping(), MemberFor("Name")).ShouldBeFalse();
    }

    [Test]
    public void ShouldNotMapIdOnClassThatAlreadyHasAnId()
    {
        var mapping = new ClassMapping();
        mapping.Set(x => x.Id, Layer.UserSupplied, new IdMapping());

        mapper.ShouldMap(mapping, MemberFor("Id")).ShouldBeFalse();
    }

    [Test]
    public void ShouldNotMapIdOnSubclass()
    {
        // Map does nothing for anything that isn't a ClassMapping, so an id member must not
        // be claimed here for a subclass; otherwise it is added to the mapped members (and
        // the loop stops trying other steps) yet never actually mapped.
        var subclass = new SubclassMapping(SubclassType.Subclass);

        mapper.ShouldMap(subclass, MemberFor("Id")).ShouldBeFalse();
    }

    static Member MemberFor(string name)
    {
        return typeof(Target).GetProperty(name).ToMember();
    }

    class Target
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
