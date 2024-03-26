using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Visitors;
using NUnit.Framework;
using Is=FluentNHibernate.Conventions.AcceptanceCriteria.Is;

namespace FluentNHibernate.Testing.ConventionsTests;

[TestFixture]
public class OptionalAcceptTests
{
    DefaultConventionFinder conventions;
    ConventionVisitor visitor;

    [SetUp]
    public void CreateVisitor()
    {
        conventions = new DefaultConventionFinder();
        visitor = new ConventionVisitor(conventions);
    }

    [Test]
    public void ShouldNotApplyConventionWithFailingAccept()
    {
        conventions.Add<ConventionWithFailingAccept>();

        var mapping = new ClassMapping();
            
        visitor.ProcessClass(mapping);

        mapping.IsSpecified("TableName").ShouldBeFalse();
    }

    [Test]
    public void ShouldApplyConventionWithSuccessfulAccept()
    {
        conventions.Add<ConventionWithSuccessfulAccept>();

        var mapping = new ClassMapping();

        visitor.ProcessClass(mapping);

        mapping.IsSpecified("TableName").ShouldBeTrue();
    }

    [Test]
    public void ShouldApplyConventionWithNoAccept()
    {
        conventions.Add<ConventionWithNoAccept>();

        var mapping = new ClassMapping();

        visitor.ProcessClass(mapping);

        mapping.IsSpecified("TableName").ShouldBeTrue();
    }

    class ConventionWithFailingAccept : IClassConvention, IConventionAcceptance<IClassInspector>
    {
        public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
        {
            criteria.Expect(x => x.TableName == "test"); // never true
        }

        public void Apply(IClassInstance instance)
        {
            instance.Table("xxx");
        }
    }

    class ConventionWithSuccessfulAccept : IClassConvention, IConventionAcceptance<IClassInspector>
    {
        public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
        {
            criteria.Expect(x => x.TableName, Is.Not.Set); // always true
        }

        public void Apply(IClassInstance instance)
        {
            instance.Table("xxx");
        }
    }

    class ConventionWithNoAccept : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table("xxx");
        }
    }
}
