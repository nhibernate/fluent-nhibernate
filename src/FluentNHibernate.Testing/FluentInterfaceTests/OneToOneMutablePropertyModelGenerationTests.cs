using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests;

[TestFixture]
public class OneToOneMutablePropertyModelGenerationTests : BaseModelFixture
{
    [Test]
    public void AccessShouldSetModelAccessPropertyToValue()
    {
        OneToOne()
            .Mapping(m => m.Access.Field())
            .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
    }

    [Test]
    public void ShouldSetModelClassPropertyToValue()
    {
        OneToOne()
            .Mapping(m => {})
            .ModelShouldMatch(x => x.Class.ShouldEqual(new TypeReference(typeof(PropertyReferenceTarget))));
    }

    [Test]
    public void ClassShouldSetModelClassPropertyToValue()
    {
        OneToOne()
            .Mapping(m => m.Class(typeof(int)))
            .ModelShouldMatch(x => x.Class.ShouldEqual(new TypeReference(typeof(int))));
    }

    [Test]
    public void ConstrainedShouldSetModelConstrainedPropertyToTrue()
    {
        OneToOne()
            .Mapping(m => m.Constrained())
            .ModelShouldMatch(x => x.Constrained.ShouldBeTrue());
    }

    [Test]
    public void NotConstrainedShouldSetModelConstrainedPropertyToFalse()
    {
        OneToOne()
            .Mapping(m => m.Not.Constrained())
            .ModelShouldMatch(x => x.Constrained.ShouldBeFalse());
    }

    [Test]
    public void CascadeSetsModelCascadePropertyToValue()
    {
        OneToOne()
            .Mapping(m => m.Cascade.All())
            .ModelShouldMatch(x => x.Cascade.ShouldEqual("all"));
    }

    [Test]
    public void CascadeAppendModelCascadePropertyToValue()
    {
        OneToOne()
            .Mapping(m => { m.Cascade.Merge(); m.Cascade.SaveUpdate(); })
            .ModelShouldMatch(x => x.Cascade.ShouldEqual("merge,save-update"));
    }

    [Test]
    public void FetchShouldSetModelFetchPropertyToValue()
    {
        OneToOne()
            .Mapping(m => m.Fetch.Select())
            .ModelShouldMatch(x => x.Fetch.ShouldEqual("select"));
    }

    [Test]
    public void ForeignKeyShouldSetModelForeignKeyPropertyToValue()
    {
        OneToOne()
            .Mapping(m => m.ForeignKey("fk"))
            .ModelShouldMatch(x => x.ForeignKey.ShouldEqual("fk"));
    }

    [Test]
    public void LazyLoadShouldSetModelLazyLoadPropertyToProxy()
    {
        OneToOne()
            .Mapping(m => m.LazyLoad())
            .ModelShouldMatch(x => x.Lazy.ShouldEqual(Laziness.Proxy.ToString()));
    }

    [Test]
    public void NotLazyLoadShouldSetModelLazyLoadPropertyToFalse()
    {
        OneToOne()
            .Mapping(m => m.Not.LazyLoad())
            .ModelShouldMatch(x => x.Lazy.ShouldEqual(Laziness.False.ToString()));
    }

    [Test]
    public void NoProxyLazyLoadShouldSetModelLazyLoadPropertyToNoProxy()
    {
        OneToOne()
            .Mapping(m => m.LazyLoad(Laziness.NoProxy))
            .ModelShouldMatch(x => x.Lazy.ShouldEqual(Laziness.NoProxy.ToString()));
    }

    [Test]
    public void ShouldSetModelNamePropertyToPropertyName()
    {
        OneToOne()
            .Mapping(m => {})
            .ModelShouldMatch(x => x.Name.ShouldEqual("Reference"));
    }

    [Test]
    public void PropertyRefShouldSetModelPropertyRefPropertyToValue()
    {
        OneToOne()
            .Mapping(m => m.PropertyRef(x => x.Name))
            .ModelShouldMatch(x => x.PropertyRef.ShouldEqual("Name"));
    }
}
