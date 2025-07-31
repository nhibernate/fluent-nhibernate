using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Visitors;
using NUnit.Framework;

namespace FluentNHibernate.Testing.PersistenceModelTests;

[TestFixture]
public class SeparateSubclassVisitorFixture
{
    IIndeterminateSubclassMappingProviderCollection providers;
    ClassMapping fooMapping;

    [SetUp]
    public void SetUp()
    {
        providers = new IndeterminateSubclassMappingProviderCollection();
    }

    [Test]
    public void Should_add_subclass_that_implements_the_parent_interface()
    {
        /* The Parent is the IFoo interface the desired results 
         * of this test is the inclusion of the Foo<T> through the
         * GenericFooMap<T> subclass mapping.
         */

        fooMapping = ((IMappingProvider)new FooMap()).GetClassMapping();

        providers.Add(new StringFooMap());
        var sut = CreateSut();
        sut.ProcessClass(fooMapping);
        Assert.That(fooMapping.Subclasses.Count(), Is.EqualTo(1));
        Assert.That(fooMapping.Subclasses.Count(sub => sub.Type == typeof(Foo<string>)), Is.EqualTo(1));
    }

    [Test]
    public void Should_add_subclass_that_implements_the_parent_base()
    {
        /* The Parent is the FooBase class the desired results 
         * of this test is the inclusion of the Foo<T> through the
         * GenericFooMap<T> subclass mapping.
         */

        fooMapping = ((IMappingProvider)new BaseMap()).GetClassMapping();

        providers.Add(new StringFooMap());
        var sut = CreateSut();
        sut.ProcessClass(fooMapping);
        Assert.That(fooMapping.Subclasses.Count(), Is.EqualTo(1));
        Assert.That(fooMapping.Subclasses.Count(sub => sub.Type == typeof(Foo<string>)), Is.EqualTo(1));
    }

    [Test]
    public void Should_not_add_subclassmap_that_does_not_implement_parent_interface()
    {
        /* The Parent is the IFoo interface the desired results 
         * of this test is the exclusion of the StandAlone class 
         * since it does not implement the interface.
         */

        fooMapping = ((IMappingProvider)new FooMap()).GetClassMapping();

        providers.Add(new StandAloneMap());
        var sut = CreateSut();
        sut.ProcessClass(fooMapping);
        Assert.That(fooMapping.Subclasses.Count(), Is.EqualTo(0));
    }

    [Test]
    public void Should_not_add_subclassmap_that_does_not_implement_parent_base()
    {
        /* The Parent is the FooBase class the desired results 
         * of this test is the exclusion of the StandAlone class 
         * since it does not implement the interface.
         */

        fooMapping = ((IMappingProvider)new BaseMap()).GetClassMapping();

        providers.Add(new StandAloneMap());
        var sut = CreateSut();
        sut.ProcessClass(fooMapping);
        Assert.That(fooMapping.Subclasses.Count(), Is.EqualTo(0));
    }

    [Test]
    public void Should_not_add_subclassmap_that_implements_a_subclass_of_the_parent_interface()
    {
        /* The Parent is the IFoo interface the desired results 
         * of this test is the inclusion of the BaseImpl class and 
         * the exclusion of the Foo<T> class since it implements 
         * the BaseImpl class which already implements FooBase.
         */

        fooMapping = ((IMappingProvider)new FooMap()).GetClassMapping();

        providers.Add(new BaseImplMap());
        providers.Add(new StringFooMap());
        var sut = CreateSut();
        sut.ProcessClass(fooMapping);
        Assert.That(fooMapping.Subclasses.Count(), Is.EqualTo(1));
        Assert.That(fooMapping.Subclasses.Count(sub => sub.Type == typeof(BaseImpl)), Is.EqualTo(1));
    }

    [Test]
    public void Should_not_add_subclassmap_that_implements_a_subclass_of_the_parent_base()
    {
        /* The Parent is the FooBase class the desired results 
         * of this test is the inclusion of the BaseImpl class and 
         * the exclusion of the Foo<T> class since it implements 
         * the BaseImpl class which already implements FooBase.
         */

        fooMapping = ((IMappingProvider)new BaseMap()).GetClassMapping();

        providers.Add(new BaseImplMap());
        providers.Add(new StringFooMap());
        var sut = CreateSut();
        sut.ProcessClass(fooMapping);
        Assert.That(fooMapping.Subclasses.Count(), Is.EqualTo(1));
        Assert.That(fooMapping.Subclasses.Count(sub => sub.Type == typeof(BaseImpl)), Is.EqualTo(1));
    }

    [Test]
    public void Should_add_explicit_extend_subclasses_to_their_parent()
    {
        fooMapping = ((IMappingProvider)new ExtendsParentMap()).GetClassMapping();

        providers.Add(new ExtendsChildMap());
        var sut = CreateSut();
        sut.ProcessClass(fooMapping);
        Assert.That(fooMapping.Subclasses.Count(), Is.EqualTo(1));
        Assert.That(fooMapping.Subclasses.Count(sub => sub.Type == typeof(ExtendsChild)), Is.EqualTo(1));
    }

    [Test]
    public void Should_choose_UnionSubclass_when_the_class_mapping_IsUnionSubclass_is_true()
    {
        fooMapping = ((IMappingProvider)new BaseMap()).GetClassMapping();
        fooMapping.Set(x => x.IsUnionSubclass, Layer.Defaults, true);

        providers.Add(new StringFooMap());

        var sut = CreateSut();

        sut.ProcessClass(fooMapping);

        fooMapping.Subclasses.First().SubclassType.ShouldEqual(SubclassType.UnionSubclass);
    }

    SeparateSubclassVisitor CreateSut()
    {
        return new SeparateSubclassVisitor(providers);
    }


    interface IFoo
    { }

    class Base : IFoo
    { }

    abstract class BaseImpl : Base
    { }

    class Foo<T> : BaseImpl, IFoo
    { }

    class FooMap : ClassMap<IFoo>
    { }

    class BaseMap : ClassMap<Base>
    { }

    class BaseImplMap : SubclassMap<BaseImpl>
    { }

    abstract class GenericFooMap<T> : SubclassMap<Foo<T>>
    { }

    class StringFooMap : GenericFooMap<string>
    { }


    interface IStand
    { }

    class StandAlone : IStand
    { }

    class StandAloneMap : SubclassMap<StandAlone>
    { }

    class ExtendsParent
    {}

    class ExtendsChild
    {}

    class ExtendsParentMap : ClassMap<ExtendsParent>
    {}

    class ExtendsChildMap : SubclassMap<ExtendsChild>
    {
        public ExtendsChildMap()
        {
            Extends<ExtendsParent>();
        }
    }
}
