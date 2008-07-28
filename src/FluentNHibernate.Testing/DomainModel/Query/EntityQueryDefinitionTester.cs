using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using FluentNHibernate;
using FluentNHibernate.Query;

namespace FluentNHibernate.Testing.DomainModel.Query
{
    public class FooClass
    {
        public string foo { get; set; }
    }

    [TestFixture]
    public class EntityQueryDefinitionTester
    {
        [Test]
        public void AddQuickFilterFor_should_add_new_FilterableProperty_to_the_quick_filters_collection_as_well_as_the_general_filter_collection()
        {
            var query = new EntityQueryDefinition();
            var filterProp = FilterableProperty.Build<FooClass, string>(x => x.foo);

            query.AddQuickFilterProperty(filterProp);

            query.QuickFilterProperties.ShouldHaveCount(1);
            query.FilterableProperties.ShouldHaveCount(1);
        }

        [Test]
        public void AllowFilterOn_should_add_new_FilterableProperty_to_the_query_filter_collection()
        {
            var query = new EntityQueryDefinition();
            var filterProp = FilterableProperty.Build<FooClass, string>(x => x.foo);

            query.AddFilterProperty(filterProp);

            query.FilterableProperties.ShouldHaveCount(1);
        }
    }

    [TestFixture]
    public class EntityQueryDefinitionBuilderTester
    {
        [Test]
        public void
            AddQuickFilterFor_should_add_new_FilterableProperty_to_the_quick_filters_collection_as_well_as_the_general_filter_collection
            ()
        {
            var query = new EntityQueryDefinitionBuilder<QueryDefTestEntity>();

            query.AddQuickFilterFor(t => t.Name);

            query.QueryDefinition.QuickFilterProperties.ShouldHaveCount(1);
            query.QueryDefinition.FilterableProperties.ShouldHaveCount(1);
        }

        [Test]
        public void AllowFilterOn_should_add_new_FilterableProperty_to_the_query_filter_collection()
        {
            var query = new EntityQueryDefinitionBuilder<QueryDefTestEntity>();

            query.AllowFilterOn(t => t.Name);

            query.QueryDefinition.FilterableProperties.ShouldHaveCount(1);
        }
    }

    [TestFixture]
    public class EntityQueryBuilderTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _queryDef = new EntityQueryDefinition();
            _query = new EntityQueryBuilder<QueryDefTestEntity>(_queryDef);

            _nameProp = FilterableProperty.Build<QueryDefTestEntity, string>(t => t.Name);
            _nameProp.PropertyType.ShouldEqual(typeof (string));

            _ageProp = FilterableProperty.Build<QueryDefTestEntity, int>(t => t.Age);

            FilterTypeRegistry.ResetAll();
        }

        #endregion

        private EntityQueryBuilder<QueryDefTestEntity> _query;
        private FilterableProperty _nameProp;
        private FilterableProperty _ageProp;
        private EntityQueryDefinition _queryDef;

        [Test]
        public void EntityQueryBuilder_AddFilter_should_add_a_new_filter_and_add_it_to_the_previous_filter_using_the_AndAlso_operator()
        {
            _query.AddFilter(new StringFilterType {StringMethod = s => s.StartsWith("")}, _nameProp, "Fo");
            _query.AddFilter(new BinaryFilterType {FilterExpressionType = ExpressionType.Equal}, _ageProp, "35");

            _query.FilterExpression.ToString().Contains("&&").ShouldBeTrue();
        }

        [Test]
        public void EntityQueryBuilder_AddFilter_should_change_the_type_if_the_value_type_is_not_a_string()
        {
            _query.AddFilter(new BinaryFilterType {FilterExpressionType = ExpressionType.Equal}, _ageProp, "35");

            _query.FilterExpression.ToString().Contains("ChangeType").ShouldBeTrue();
        }

        [Test]
        public void EntityQueryBuilder_AddFilter_should_not_change_the_type_if_the_value_type_is_already_a_string()
        {
            _query.AddFilter(new StringFilterType {StringMethod = s => s.StartsWith("")}, _nameProp, "Fo");

            _query.FilterExpression.ToString().Contains("ChangeType").ShouldBeFalse();
        }

        [Test]
        public void EntityQueryBuilder_AddFilter_SmokeTest()
        {
            _query.AddFilter(new StringFilterType {StringMethod = s => s.StartsWith("")}, _nameProp, "Fo");
            _query.AddFilter(new BinaryFilterType {FilterExpressionType = ExpressionType.Equal}, _ageProp, "35");

            var itemList = new List<QueryDefTestEntity>
                               {
                                   new QueryDefTestEntity {Name = "Bar", Age = 99},
                                   new QueryDefTestEntity {Name = "Foo", Age = 35},
                                   new QueryDefTestEntity {Name = "Baz", Age = 1}
                               };

            QueryDefTestEntity[] values = itemList.AsQueryable().Where(_query.FilterExpression).ToArray();

            values.Length.ShouldEqual(1);
            values[0].Name.ShouldEqual("Foo");
            values[0].Age.ShouldEqual(35);
        }

        [Test]
        public void EntityQueryBuilder_PrepareQuery_should_loop_through_each_criterion_and_set_up_a_filter()
        {
            var repo = new InMemoryRepository();
            repo.Save(new QueryDefTestEntity {Name = "Bar", Age = 99});
            repo.Save(new QueryDefTestEntity {Name = "Foo", Age = 35});
            repo.Save(new QueryDefTestEntity {Name = "Baz", Age = 1});

            FilterTypeRegistry.ClearAll();

            FilterTypeRegistry.RegisterFilter(
                new StringFilterType
                    {
                        Key = "StringFoo",
                        StringMethod = s => s.StartsWith("")
                    })
                .ForType<string>();

            _queryDef.AddFilterProperty(FilterableProperty.Build<QueryDefTestEntity, string>(t => t.Name));


            var crit = new Criteria
                           {property = "Name", op = "StringFoo", value = "Fo"};

            QueryDefTestEntity[] values =
                _query.PrepareQuery(new[] {crit}, repo).Cast<QueryDefTestEntity>().ToArray();

            values.Length.ShouldEqual(1);
            values[0].Name.ShouldEqual("Foo");
            values[0].Age.ShouldEqual(35);
        }
    }

    public class QueryDefTestEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}