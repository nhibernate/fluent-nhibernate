using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;
using System.Linq;

namespace FluentNHibernate.Testing.Visitors
{
    [TestFixture]
    public class when_there_is_any_unpaired_collection_property_alphabetically_before_collection_property_which_has_corresponding_back_reference_from_child : RelationshipPairingSpec
    {
        CollectionMapping collectionWithoutBackrefAndAlphabeticallyFirst;
        CollectionMapping collectionWithoutBackrefAndAlphabeticallyLast;
        CollectionMapping collectionWithCorrespondingBackref;
        ManyToOneMapping referenceFromChildSecondTypeToParent;

        public override void establish_context()
        {
            var autoPersistenceModel = AutoMap.Source(new Types(), new AutomappingConfiguration());
            var hibernateMappings = autoPersistenceModel.BuildMappings();

            var classMapping = hibernateMappings.SelectMany(h => h.Classes).Single(c => c.Type == typeof(Parent));
            collectionWithoutBackrefAndAlphabeticallyFirst = classMapping.Collections.Single(c => c.ChildType == typeof(ChildFirstType));
            collectionWithCorrespondingBackref = classMapping.Collections.Single(c => c.ChildType == typeof(ChildSecondType));
            collectionWithoutBackrefAndAlphabeticallyLast = classMapping.Collections.Single(c => c.ChildType == typeof(ChildThirdType));
        }

        public override void because()
        {

        }

        [Test]
        public void should_pair_with_corresponding_property()
        {
            collectionWithoutBackrefAndAlphabeticallyFirst.OtherSide.ShouldBeNull();
            collectionWithCorrespondingBackref.OtherSide.ShouldNotBeNull();
        }
    }

    [TestFixture]
    public class when_relation_has_two_ends : RelationshipPairingSpec
    {
        CollectionMapping collectionWithCorrespondingBackref;
        ManyToOneMapping referenceFromChildSecondTypeToParent;

        public override void establish_context()
        {
            var autoPersistenceModel = AutoMap.Source(new Types(), new AutomappingConfiguration());
            var hibernateMappings = autoPersistenceModel.BuildMappings();

            var classMapping = hibernateMappings.SelectMany(h => h.Classes).Single(c => c.Type == typeof(Parent));
            collectionWithCorrespondingBackref = classMapping.Collections.Single(c => c.ChildType == typeof(ChildSecondType));
        }

        public override void because()
        {

        }

        [Test]
        public void should_use_single_foreign_key_column()
        {
            var columnMappingFromParentSide = collectionWithCorrespondingBackref.Key.Columns.Single();
            var columnMappingFromChildSide = ((ManyToOneMapping)collectionWithCorrespondingBackref.OtherSide).Columns.Single();

            columnMappingFromParentSide.Name.ShouldEqual(columnMappingFromChildSide.Name);
        }
    }

    public abstract class RelationshipPairingSpec : Specification
    {
        protected class Types : ITypeSource
        {
            public IEnumerable<Type> GetTypes()
            {
                return new[] { typeof(Parent), typeof(ChildFirstType), typeof(ChildSecondType), typeof(ChildThirdType) };
            }

            public void LogSource(IDiagnosticLogger logger)
            { }

            public string GetIdentifier()
            {
                return "3C423683-08F3-4FD8-9B9C-A043B11C52B5";
            }
        }

        protected class AutomappingConfiguration : DefaultAutomappingConfiguration
        {
            public override bool ShouldMap(Type type)
            {
                return true;
            }
        }

        protected class Parent
        {
            public int Id { get; set; }
            public string Property { get; set; }
            public IList<ChildFirstType> AChildrenOfFirstType { get; set; }
            public IList<ChildSecondType> BChildrenOfSecondType { get; set; }
            public IList<ChildThirdType> CChildrenOfThirdType { get; set; }
        }

        protected class ChildFirstType
        {
            public int Id { get; set; }
            public string Property { get; set; }
        }

        protected class ChildSecondType
        {
            public int Id { get; set; }
            public string Property { get; set; }
            public Parent MyParent { get; set; }
        }

        protected class ChildThirdType
        {
            public int Id { get; set; }
            public string Property { get; set; }
        }

    }
}