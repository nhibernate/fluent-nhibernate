using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using Is=FluentNHibernate.Conventions.AcceptanceCriteria.Is;

namespace FluentNHibernate.Testing.ConventionsTests.AcceptanceCriteria
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaAnyTests
    {
        private IAcceptanceCriteria<IPropertyInspector> acceptance;

        [SetUp]
        public void CreateAcceptanceCriteria()
        {
            acceptance = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        }

        [Test]
        public void AnyShouldValidateToTrueIfOneMatches()
        {
            acceptance
                .Any(c => c.Expect(x => x.Name, Is.Set), 
                     c => c.Expect(x => x.Type, Is.Set), 
                     c => c.Expect(x => x.Insert, Is.Set));

            acceptance
               .Matches(new PropertyInspector(new PropertyMapping { Name="Property1"}))
               .ShouldBeTrue();
        }

        [Test]
        public void AnyShouldValidateToTrueIfAllMatch()
        {
            acceptance
                .Any(c => c.Expect(x => x.Name, Is.Set), 
                     c => c.Expect(x => x.Type, Is.Set), 
                     c => c.Expect(x => x.Insert, Is.Set));

            acceptance
               .Matches(new PropertyInspector(new PropertyMapping { Name = "Property1", Type= new TypeReference(typeof(string)), Insert = true}))
               .ShouldBeTrue();
        }

        [Test]
        public void AnyShouldValidateToFalseIfNoneMatch()
        {
            acceptance
                .Any(c => c.Expect(x => x.Name, Is.Set), 
                     c => c.Expect(x => x.Type, Is.Set), 
                     c => c.Expect(x => x.Insert, Is.Set));

            acceptance
               .Matches(new PropertyInspector(new PropertyMapping()))
               .ShouldBeFalse();
        }

        [Test]
        public void CanPassSubCriteriaToAny()
        {
            var subCriteria1 = new ConcreteAcceptanceCriteria<IPropertyInspector>();
            subCriteria1.Expect(x => x.Name, Is.Set);

            var subCriteria2 = new ConcreteAcceptanceCriteria<IPropertyInspector>();
            subCriteria2.Expect(x => x.Type, Is.Set);

            acceptance.Any(subCriteria1, subCriteria2);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping { Name = "Property1"}))
                .ShouldBeTrue();
        }

        [Test]
        public void EitherIsAnyWithTwoParameters()
        {
            acceptance
                .Either(c => c.Expect(x => x.Name, Is.Set), 
                        c => c.Expect(x => x.Type, Is.Set));

            acceptance
               .Matches(new PropertyInspector(new PropertyMapping { Name = "Property1" }))
               .ShouldBeTrue();
        }

        [Test]
        public void CanPassSubCriteriaToEither()
        {
            var subCriteria1 = new ConcreteAcceptanceCriteria<IPropertyInspector>();
            subCriteria1.Expect(x => x.Name, Is.Set);

            var subCriteria2 = new ConcreteAcceptanceCriteria<IPropertyInspector>();
            subCriteria2.Expect(x => x.Type, Is.Set);

            acceptance.Either(subCriteria1, subCriteria2);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping { Name = "Property1" }))
                .ShouldBeTrue();
        }
    }
}
